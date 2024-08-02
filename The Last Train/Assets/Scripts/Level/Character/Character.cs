using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using Unity.Cinemachine;
using Zenject;
using Cysharp.Threading.Tasks;
using DG.Tweening;

using TLT.Input;
using TLT.HealthManager;
using TLT.Interfaces;
using TLT.Weapons;
using TLT.CameraManager;
using TLT.Save;
using TLT.Data;

namespace TLT.CharacterManager
{
  public class Character : MonoBehaviour, IDamageable, ISaveLoad
  {
    [SerializeField] private Health _health;

    [SerializeField] private WeaponController _weaponController;

    [SerializeField] private CharacterDialog _characterDialog;

    //-----------------------------------

    private Interaction interaction;

    private CameraController cameraController;

    private LevelManager levelManager;

    private int direction = 1;

    //===================================

    public Camera MainCamera { get; set; }

    public InputHandler InputHandler { get; private set; }

    public Animator Animator { get; private set; }

    public CharacterDialog CharacterDialog { get => _characterDialog; private set => _characterDialog = value; }

    public CinemachineCamera CinemachineCamera { get; set; }
    public CinemachinePositionComposer CinemachinePositionComposer { get; set; }

    public WeaponController WeaponController { get => _weaponController; set => _weaponController = value; }

    public Health Health { get => _health; private set => _health = value; }

    public bool IsTakeDamage { get; set; }

    public int Direction
    {
      get => direction;
      set
      {
        direction = value;
        cameraController.ChangeDirection(value);
      }
    }

    public List<GameObject> CollectingBalls { get; set; } = new();

    //===================================

    public event Action<int> OnChangeDirection;

    //===================================

    [Inject]
    private void Construct(InputHandler parInputHandler,
      CameraController parCameraController,
      CinemachineCamera parCinemachineCamera,
      CinemachinePositionComposer parCinemachinePositionComposer,
      LevelManager parLevelManager)
    {
      InputHandler = parInputHandler;
      cameraController = parCameraController;
      CinemachineCamera = parCinemachineCamera;
      CinemachinePositionComposer = parCinemachinePositionComposer;
      levelManager = parLevelManager;
    }

    //===================================

    private void Awake()
    {
      MainCamera = Camera.main;

      Animator = GetComponent<Animator>();

      CharacterDialog = GetComponent<CharacterDialog>();

      interaction = GetComponent<Interaction>();
    }

    private void Start()
    {
      Direction = 1;

      Time.timeScale = 1;
    }

    private void OnEnable()
    {
      InputHandler.AI_Player.Player.Select.performed += Select_performed;

      InputHandler.AI_Player.Player.Shooting.performed += Shooting_performed;

      InputHandler.AI_Player.Player.Recharge.performed += Recharge_performed;

      Health.OnTakeHealth += Health_OnTakeHealth;

      Health.OnInstantlyKill += OnInstantlyKill;
    }

    private void OnDisable()
    {
      InputHandler.AI_Player.Player.Select.performed -= Select_performed;

      InputHandler.AI_Player.Player.Shooting.performed -= Shooting_performed;

      InputHandler.AI_Player.Player.Recharge.performed -= Recharge_performed;

      Health.OnTakeHealth -= Health_OnTakeHealth;

      Health.OnInstantlyKill -= OnInstantlyKill;
    }

    private void Update()
    {
      IsTakeDamage = false;
    }

    //===================================

    public void ApplyDamage(int parDamage)
    {
      Health.TakeHealth(parDamage);
    }

    //===================================

    private void OnInstantlyKill()
    {
      levelManager.StartRestartScene();

      gameObject.SetActive(false);
    }

    //===================================

    private void Select_performed(UnityEngine.InputSystem.InputAction.CallbackContext parContext)
    {
      if (interaction == null)
        return;

      ObjectInteraction objectInteraction = interaction.ObjectInteraction;

      if (objectInteraction == null)
        return;

      if (objectInteraction.TryGetComponent(out IPickupable parIPickupable))
      {
        parIPickupable.Pickup(this);
      }

      objectInteraction.Interact();
      objectInteraction.InteractCharacter(this);
    }

    private void Shooting_performed(UnityEngine.InputSystem.InputAction.CallbackContext parContext)
    {
      if (IsTakeDamage)
        return;

      if (Animator != null)
        Animator.SetTrigger("IsShoot");

      WeaponController.CurrentWeapon.Shoot();
    }

    private void Recharge_performed(UnityEngine.InputSystem.InputAction.CallbackContext parContext)
    {
      if (IsTakeDamage)
        return;

      WeaponController.CurrentWeapon.PerformRecharge();
    }

    private void Health_OnTakeHealth(int obj)
    {
      if (Animator != null)
        Animator.SetTrigger("IsTakeDamage");
    }    

    //===================================

    public ObjectData SaveData()
    {
      string objectName = transform.name;
      Vector3 rotation = transform.rotation.eulerAngles;
      Vector3 position = transform.position;
      Vector3 scale = transform.localScale;

      ObjectData data = new(objectName);
      data.Parameters["Position"] = new float[] { position.x, position.y, position.z };
      data.Parameters["Rotation"] = new float[] { rotation.x, rotation.y, rotation.z };
      data.Parameters["Scale"] = new float[] { scale.x, scale.y, scale.z };

      return data;
    }

    public void LoadData(ObjectData parObjectData)
    {
      if (parObjectData.ObjectName == gameObject.name)
      {
        LoadTransform(parObjectData);
      }
    }

    //===================================

    private void LoadTransform(ObjectData parData)
    {
      if (parData.Parameters.TryGetValue("Position", out var position) && position is JArray positionArray)
      {
        transform.position = new Vector3(positionArray[0].ToObject<float>(), positionArray[1].ToObject<float>(), positionArray[2].ToObject<float>());
      }

      if (parData.Parameters.TryGetValue("Rotation", out var rotation) && rotation is JArray rotationArray)
      {
        transform.rotation = Quaternion.Euler(rotationArray[0].ToObject<float>(), rotationArray[1].ToObject<float>(), rotationArray[2].ToObject<float>());
      }

      if (parData.Parameters.TryGetValue("Scale", out var scale) && scale is JArray scaleArray)
      {
        transform.localScale = new Vector3(scaleArray[0].ToObject<float>(), scaleArray[1].ToObject<float>(), scaleArray[2].ToObject<float>());

        Direction = scaleArray[0].ToObject<int>();
      }
    }

    //===================================
  }
}
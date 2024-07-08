using System;
using UnityEngine;
using Unity.Cinemachine;
using Zenject;

using TLT.Input;
using TLT.HealthManager;
using TLT.Interfaces;
using TLT.Weapons;
using TLT.CameraManager;
using TLT.Save;
using System.Collections.Generic;
using TLT.Json;

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

    public void SaveData(Dictionary<string, Dictionary<string, object>> parData)
    {
      Vector3 position = transform.position;
      Vector3 rotation = transform.rotation.eulerAngles;
      Vector3 scale = transform.localScale;

      if (!parData.ContainsKey("Character"))
        parData.Add("Character", new Dictionary<string, object>());

      parData["Character"][BikeKeySaveLoad.POSITION_KEY] = new float[] { position.x, position.y, position.z };
      parData["Character"][BikeKeySaveLoad.ROTATION_KEY] = new float[] { rotation.x, rotation.y, rotation.z };
      parData["Character"][BikeKeySaveLoad.SCALE_KEY] = new float[] { scale.x, scale.y, scale.z };
      parData["Character"][BikeKeySaveLoad.DIRECTION_KEY] = Direction;
    }

    public void LoadData(Dictionary<string, Dictionary<string, object>> parData)
    {
      if (parData.TryGetValue("Character", out Dictionary<string, object> parObject))
      {
        if (parObject.TryGetValue(BikeKeySaveLoad.POSITION_KEY, out object parPosition))
        {
          Vector3 position = ConvertJson.ConvertFromJsonToVector3(parPosition);

          transform.position = position;
        }

        if (parObject.TryGetValue(BikeKeySaveLoad.ROTATION_KEY, out object parRotation))
        {
          Vector3 rotation = ConvertJson.ConvertFromJsonToVector3(parRotation);

          transform.rotation = Quaternion.Euler(rotation);
        }

        if (parObject.TryGetValue(BikeKeySaveLoad.SCALE_KEY, out object parScale))
        {
          Vector3 scale = ConvertJson.ConvertFromJsonToVector3(parScale);

          transform.localScale = scale;
        }

        if (parObject.TryGetValue(BikeKeySaveLoad.DIRECTION_KEY, out object parDirection))
        {
          string number = $"{parDirection}";

          Direction = int.Parse(number);
        }
      }
    }

    //===================================
  }
}
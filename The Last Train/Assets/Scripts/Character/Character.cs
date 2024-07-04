using System;
using UnityEngine;
using Unity.Cinemachine;
using Zenject;

using TLT.Input;
using TLT.HealthManager;
using TLT.Interfaces;
using TLT.Weapons;
using TLT.CameraManager;

namespace TLT.CharacterManager
{
  public class Character : MonoBehaviour, IDamageable
  {
    [SerializeField] private Health _health;

    [SerializeField] private WeaponController _weaponController;

    [SerializeField] private CharacterDialog _characterDialog;

    //-----------------------------------

    private Interaction interaction;

    private CameraController cameraController;

    private LevelManager levelManager;

    private int direction = 1;

    /*public float transitionSpeed = 2.0f;
    private Vector2 targetScreenPosition;
    private Vector2 currentScreenPosition;*/

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
        //targetScreenPosition = new(-0.25f * value, 0.2f);
        //CinemachinePositionComposer.Composition.ScreenPosition = new(-0.25f * value, 0.2f);
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

      /*currentScreenPosition = Vector2.Lerp(currentScreenPosition, targetScreenPosition, Time.deltaTime * transitionSpeed);
      CinemachinePositionComposer.Composition.ScreenPosition = currentScreenPosition;*/
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
  }
}
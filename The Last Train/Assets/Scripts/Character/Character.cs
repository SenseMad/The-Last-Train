using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Unity.Cinemachine;

using TLT.Input;
using TLT.HealthManager;
using TLT.Interfaces;
using TLT.Weapons;
using System;

namespace TLT.CharacterManager
{
  public class Character : MonoBehaviour, IDamageable
  {
    private static Character instance;

    //===================================

    [SerializeField] private Health _health;

    [SerializeField] private WeaponController _weaponController;

    //-----------------------------------

    private Interaction interaction;

    private int direction = 1;

    //===================================

    public static Character Instance
    {
      get => instance;
    }

    //===================================

    public Camera MainCamera { get; set; }

    public InputHandler InputHandler { get; private set; }

    public CinemachineCamera CinemachineCamera { get; set; }
    public CinemachinePositionComposer CinemachinePositionComposer { get; set; }

    public WeaponController WeaponController { get => _weaponController; private set => _weaponController = value; }

    public Health Health { get => _health; private set => _health = value; }

    public bool IsTakeDamage { get; set; }

    public int Direction
    {
      get => direction;
      set
      {
        direction = value;
        OnChangeDirection?.Invoke(value);
      }
    }

    //===================================

    public event Action<int> OnChangeDirection;

    //===================================

    [Inject]
    private void Construct(InputHandler parInputHandler, CinemachineCamera parCinemachineCamera, CinemachinePositionComposer parCinemachinePositionComposer)
    {
      InputHandler = parInputHandler;
      CinemachineCamera = parCinemachineCamera;
      CinemachinePositionComposer = parCinemachinePositionComposer;
    }

    //===================================

    private void Awake()
    {
      MainCamera = Camera.main;

      interaction = GetComponent<Interaction>();

      instance = this;
    }

    private void Start()
    {
      ChangeDirection(Direction);
    }

    private void OnEnable()
    {
      InputHandler.AI_Player.Player.Select.performed += Select_performed;

      InputHandler.AI_Player.Player.Shooting.performed += Shooting_performed;

      InputHandler.AI_Player.Player.Recharge.performed += Recharge_performed;

      OnChangeDirection += ChangeDirection;
    }

    private void ChangeDirection(int parDirection)
    {
      CinemachinePositionComposer.Composition.ScreenPosition = new(-0.25f * parDirection, 0.2f);
    }

    private void OnDisable()
    {
      InputHandler.AI_Player.Player.Select.performed -= Select_performed;

      InputHandler.AI_Player.Player.Shooting.performed -= Shooting_performed;

      InputHandler.AI_Player.Player.Recharge.performed -= Recharge_performed;

      OnChangeDirection -= ChangeDirection;
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

      WeaponController.CurrentWeapon.Shoot();
    }

    private void Recharge_performed(UnityEngine.InputSystem.InputAction.CallbackContext parContext)
    {
      if (IsTakeDamage)
        return;

      WeaponController.CurrentWeapon.PerformRecharge();
    }

    //===================================
  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Unity.Cinemachine;

using TLT.Input;
using TLT.HealthManager;
using TLT.Interfaces;
using TLT.Weapons;

namespace TLT.CharacterManager
{
  public class Character : MonoBehaviour, IDamageable
  {
    [SerializeField] private Health _health;

    [SerializeField] private WeaponController _weaponController;

    [Space(10)]
    [SerializeField] private CinemachineCamera _cinemachineCamera;

    //-----------------------------------

    private Interaction interaction;

    //===================================

    public Camera MainCamera { get; set; }

    public InputHandler InputHandler { get; private set; }

    public WeaponController WeaponController { get => _weaponController; private set => _weaponController = value; }

    public CinemachineCamera CinemachineCamera { get => _cinemachineCamera; set => _cinemachineCamera = value; }

    public Health Health { get => _health; private set => _health = value; }

    public bool IsTakeDamage { get; set; }

    //===================================

    [Inject]
    private void Construct(InputHandler parInputHandler)
    {
      InputHandler = parInputHandler;
    }

    //===================================

    private void Awake()
    {
      MainCamera = Camera.main;

      interaction = GetComponent<Interaction>();
    }

    private void OnEnable()
    {
      InputHandler.AI_Player.Player.Select.performed += Select_performed;

      InputHandler.AI_Player.Player.Shooting.performed += Shooting_performed;

      InputHandler.AI_Player.Player.Recharge.performed += Recharge_performed;
    }

    private void OnDisable()
    {
      InputHandler.AI_Player.Player.Select.performed -= Select_performed;

      InputHandler.AI_Player.Player.Shooting.performed -= Shooting_performed;

      InputHandler.AI_Player.Player.Recharge.performed -= Recharge_performed;
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

    private void Select_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
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

    private void Shooting_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
      if (IsTakeDamage)
        return;

      WeaponController.CurrentWeapon.Shoot();
    }

    private void Recharge_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
      if (IsTakeDamage)
        return;

      WeaponController.CurrentWeapon.PerformRecharge();
    }

    //===================================
  }
}
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

using TLT.Weapons;
using TLT.Input;
using TLT.CharacterManager;

namespace TLT.Vehicles.Bike
{
  public class BikeCharacter : MonoBehaviour
  {
    [SerializeField] private WeaponController _weaponController;

    [SerializeField] private Animator _animatorShoot;

    [SerializeField] private Animation _animationShoot;

    //-----------------------------------

    private InputHandler inputHandler;
    private Character character;

    //===================================

    [Inject]
    private void Construct(InputHandler parInputHandler, Character parCharacter)
    {
      inputHandler = parInputHandler;
      character = parCharacter;
    }

    //===================================

    private void OnEnable()
    {
      inputHandler.AI_Player.Player.Shooting.performed += Shooting_performed;

      inputHandler.AI_Player.Player.Recharge.performed += Recharge_performed;
    }

    private void OnDisable()
    {
      inputHandler.AI_Player.Player.Shooting.performed -= Shooting_performed;

      inputHandler.AI_Player.Player.Recharge.performed -= Recharge_performed;
    }

    //===================================

    private void Shooting_performed(InputAction.CallbackContext context)
    {
      if (_animatorShoot != null)
        _animatorShoot.SetTrigger("IsShoot");

      //_animationShoot.Play();

      character.WeaponController.CurrentWeapon.Shoot();
      //_weaponController.CurrentWeapon.Shoot();
    }

    private void Recharge_performed(InputAction.CallbackContext context)
    {
      character.WeaponController.CurrentWeapon.PerformRecharge();
      //_weaponController.CurrentWeapon.PerformRecharge();
    }

    //===================================
  }
}
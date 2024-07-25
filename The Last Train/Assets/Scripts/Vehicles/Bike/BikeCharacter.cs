using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

using TLT.Weapons;
using TLT.Input;
using TLT.CharacterManager;

namespace TLT.Bike.Bike
{
  public class BikeCharacter : MonoBehaviour
  {
    [SerializeField] private BikeBody _bikeBody;
    [SerializeField] private BikeController _bikeController;

    [Space]
    [SerializeField] private WeaponController _weaponController;

    [SerializeField] private Animator _animatorShoot;

    [SerializeField] private Animation _animationShoot;

    //-----------------------------------

    private InputHandler inputHandler;

    private LevelManager levelManager;

    //===================================

    public Character Character { get; private set; }

    //===================================

    [Inject]
    private void Construct(InputHandler parInputHandler, Character parCharacter, LevelManager parLevelManager)
    {
      inputHandler = parInputHandler;
      Character = parCharacter;
      levelManager = parLevelManager;
    }

    //===================================

    private void Update()
    {
      if (!_bikeController.IsInCar)
        return;

      //Character.MovingParticles();
    }

    //===================================

    private void OnEnable()
    {
      inputHandler.AI_Player.Player.Shooting.performed += Shooting_performed;

      inputHandler.AI_Player.Player.Recharge.performed += Recharge_performed;

      Character.Health.OnInstantlyKill += Health_OnInstantlyKill;
    }

    private void OnDisable()
    {
      inputHandler.AI_Player.Player.Shooting.performed -= Shooting_performed;

      inputHandler.AI_Player.Player.Recharge.performed -= Recharge_performed;

      Character.Health.OnInstantlyKill -= Health_OnInstantlyKill;
    }

    //===================================

    private void Health_OnInstantlyKill()
    {
      _bikeBody.BikeController.IsInCar = false;
      _bikeBody.ObjectCharacterBody.SetActive(false);
      _bikeController.Character = null;

      _bikeBody.ObjectBody.SetActive(true);

      levelManager.StartRestartScene();
    }

    private void Shooting_performed(InputAction.CallbackContext context)
    {
      if (_animatorShoot != null)
        _animatorShoot.SetTrigger("IsShoot");

      //_animationShoot.Play();

      Character.WeaponController.CurrentWeapon.Shoot();
      //_weaponController.CurrentWeapon.Shoot();
    }

    private void Recharge_performed(InputAction.CallbackContext context)
    {
      Character.WeaponController.CurrentWeapon.PerformRecharge();
      //_weaponController.CurrentWeapon.PerformRecharge();
    }

    //===================================
  }
}
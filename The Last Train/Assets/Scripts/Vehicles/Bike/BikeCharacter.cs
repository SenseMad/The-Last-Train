using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

using TLT.Weapons;
using TLT.Input;
using TLT.CharacterManager;

namespace TLT.Bike.Bike
{
  public class BikeCharacter : MonoBehaviour, IBikeBootstrap
  {
    [Space]
    [SerializeField] private WeaponController _weaponController;

    [SerializeField] private Animator _animatorShoot;

    //-----------------------------------

    private BikeBody bikeBody;
    private BikeController bikeController;

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

    public void CustomAwake()
    {
      bikeBody = GetComponentInParent<BikeBody>();
      bikeController = GetComponentInParent<BikeController>();
    }

    public void CustomStart() { }

    /*private void Update()
    {
      if (!bikeController.IsInCar)
        return;

      //Character.MovingParticles();
    }*/

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
      bikeBody.BikeController.IsInCar = false;
      bikeBody.ObjectCharacterBody.SetActive(false);
      bikeController.Character = null;

      bikeBody.ObjectBody.SetActive(true);

      levelManager.StartRestartScene();
    }

    private void Shooting_performed(InputAction.CallbackContext context)
    {
      if (_animatorShoot != null)
        _animatorShoot.SetTrigger("IsShoot");

      Character.WeaponController.CurrentWeapon.Shoot();
    }

    private void Recharge_performed(InputAction.CallbackContext context)
    {
      Character.WeaponController.CurrentWeapon.PerformRecharge();
    }

    //===================================
  }
}
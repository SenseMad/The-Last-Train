using System;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

using TLT.Interfaces;
using TLT.CharacterManager;
using TLT.Input;
using TLT.Weapons;
using TLT.Vehicles.Bike;

namespace TLT.Vehicles
{
  public abstract class VehicleController : MonoBehaviour, IVehicleable
  {
    [SerializeField] private BikeManager _bikeManager;

    [Space]
    [SerializeField] private Character _currentCharacter;
    [SerializeField] private GameObject _objectBody;

    [SerializeField] private Transform _effectDust;

    [Space(10)]
    [SerializeField] protected BikeData _vehicleData;

    //-----------------------------------

    protected Character oldCharacter;

    private ObjectInteraction objectInteraction;

    private LevelManager levelManager;

    private Animator animator;

    protected bool isCurrentRightFlip = true;

    //===================================

    public InputHandler InputHandler { get; private set; }

    protected Rigidbody2D Rigidbody2D { get; private set; }

    public bool IsInCar { get; private set; }
    public bool IsGrounded { get; set; }

    //-----------------------------------

    public Character CurrentCharacter => _currentCharacter;

    //===================================

    [Inject]
    private void Construct(InputHandler parInputHandler, LevelManager parLevelManager)
    {
      InputHandler = parInputHandler;
      levelManager = parLevelManager;
    }

    //===================================

    public event Action OnGetInCar;
    public event Action OnGetOutCar;

    //===================================

    protected virtual void Awake()
    {
      Rigidbody2D = GetComponent<Rigidbody2D>();

      objectInteraction = GetComponentInChildren<ObjectInteraction>();

      animator = GetComponent<Animator>();

      /*getGrounded = GetComponent<GetGrounded>();
      vehicleGrounded = GetComponent<VehicleGrounded>();*/
    }

    protected virtual void OnEnable()
    {
      objectInteraction.OnInteractCharacter += GetInCar;

      //InputHandler.AI_Player.Player.Move.performed += Move_performed;
      InputHandler.AI_Player.Vehicle.Throttle.performed += Throttle;

      InputHandler.AI_Player.Vehicle.Space.performed += Flip;

      OnGetInCar += VehicleController_OnGetInCar;
      OnGetOutCar += VehicleController_OnGetOutCar;
    }

    protected virtual void OnDisable()
    {
      objectInteraction.OnInteractCharacter -= GetInCar;

      InputHandler.AI_Player.Player.Select.performed -= Select_performed;

      //InputHandler.AI_Player.Player.Move.performed -= Move_performed;
      InputHandler.AI_Player.Vehicle.Throttle.performed -= Throttle;

      InputHandler.AI_Player.Vehicle.Space.performed -= Flip;

      OnGetInCar -= VehicleController_OnGetInCar;
      OnGetOutCar -= VehicleController_OnGetOutCar;
    }

    protected virtual void Update()
    {
      /*if (getGrounded)
      {
        //IsGrounded = getGrounded.GetGround(boxCollider2D);
        //IsGrounded = vehicleGrounded.GetVehicleGround();
      }*/
    }

    protected virtual void FixedUpdate()
    {
      Move();
    }

    //===================================

    public abstract void Move();

    //===================================

    /// <summary>
    /// Сесть в машину
    /// </summary>
    public void GetInCar(Character parOldCharacter)
    {
      if (IsInCar)
        return;

      oldCharacter = parOldCharacter;

      parOldCharacter.gameObject.SetActive(false);
      _currentCharacter.gameObject.SetActive(true);
      _objectBody.SetActive(false);

      #region Weapon

      Weapon oldDataWeapon = oldCharacter.WeaponController.CurrentWeapon;
      Weapon currentDataWeapon = _currentCharacter.WeaponController.CurrentWeapon;

      if (oldDataWeapon != null)
      {
        if (currentDataWeapon != null)
        {
          currentDataWeapon.GetWeaponData(oldDataWeapon.CurrentAmountAmmo, oldDataWeapon.CurrentAmountAmmoInMagazine);
        }
      }

      #endregion

      #region Health

      _currentCharacter.Health.SetHealth(oldCharacter.Health.CurrentHealth);

      #endregion

      #region Camera

      _currentCharacter.CinemachineCamera = oldCharacter.CinemachineCamera;
      _currentCharacter.CinemachineCamera.Target.TrackingTarget = _currentCharacter.transform;

      #endregion

      levelManager.ChangeCharacter(_currentCharacter);

      OnGetInCar?.Invoke();
    }

    /// <summary>
    /// Выйти из машины
    /// </summary>
    public void GetOutCar()
    {
      if (!IsInCar)
        return;

      oldCharacter.transform.position = transform.position;
      oldCharacter.gameObject.SetActive(true);
      _currentCharacter.gameObject.SetActive(false);
      _objectBody.SetActive(true);

      #region Weapon

      Weapon oldDataWeapon = oldCharacter.WeaponController.CurrentWeapon;
      Weapon currentDataWeapon = _currentCharacter.WeaponController.CurrentWeapon;

      if (currentDataWeapon != null)
      {
        if (oldDataWeapon != null)
        {
          oldDataWeapon.GetWeaponData(currentDataWeapon.CurrentAmountAmmo, currentDataWeapon.CurrentAmountAmmoInMagazine);
        }
      }

      #endregion

      #region Health

      oldCharacter.Health.SetHealth(_currentCharacter.Health.CurrentHealth);

      #endregion

      #region Camera

      oldCharacter.CinemachineCamera.Target.TrackingTarget = oldCharacter.transform;

      #endregion

      levelManager.ChangeCharacter(oldCharacter);

      oldCharacter = null;

      OnGetOutCar?.Invoke();
    }

    private void Flip(InputAction.CallbackContext parContext)
    {
      /*if (!IsInCar)
        return;

      if (!IsGrounded)
        return;

      if (isCurrentRightFlip)
      {
        transform.localRotation = Quaternion.Euler(0, 180, 0);
        isCurrentRightFlip = false;
      }
      else
      {
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        isCurrentRightFlip = true;
      }*/
    }

    //===================================

    private void VehicleController_OnGetInCar()
    {
      IsInCar = true;

      InputHandler.AI_Player.Player.Select.performed += Select_performed;
    }

    private void VehicleController_OnGetOutCar()
    {
      IsInCar = false;
    }

    //===================================

    private void Select_performed(InputAction.CallbackContext parContext)
    {
      GetOutCar();

      InputHandler.AI_Player.Player.Select.performed -= Select_performed;
    }

    private void Throttle(InputAction.CallbackContext parContext)
    {
      if (!IsInCar || !_bikeManager.Grounded)
        return;

      animator.SetTrigger("IsMove");
    }

    private void Move_performed(InputAction.CallbackContext parContext)
    {
      if (!IsInCar || !_bikeManager.Grounded)
        return;

      if (Mathf.RoundToInt(parContext.ReadValue<Vector2>().x) > 0 || Mathf.RoundToInt(parContext.ReadValue<Vector2>().x) < 0)
        return;

      if (Mathf.RoundToInt(parContext.ReadValue<Vector2>().y) < 0)
        return;

      animator.SetTrigger("IsMove");
    }

    //===================================
  }
}
using System;
using UnityEngine;
using Zenject;

using TLT.Interfaces;
using TLT.CharacterManager;
using TLT.Input;
using TLT.Weapons;

namespace TLT.Vehicles
{
  public abstract class VehicleController : MonoBehaviour, IVehicleable
  {
    [SerializeField] private Character _currentCharacter;
    [SerializeField] private GameObject _objectBody;

    [SerializeField] private Transform _effectDust;

    [Space(10)]
    [SerializeField] protected VehicleData _vehicleData;

    //-----------------------------------

    protected Character oldCharacter;

    private ObjectInteraction objectInteraction;

    private LevelManager levelManager;

    private Animator animator;

    protected bool isInCar = false;
    protected bool isGrounded = false;

    private bool isCurrentRightFlip = true;

    protected BoxCollider2D boxCollider2D;

    protected GetGrounded getGrounded;

    //===================================

    protected InputHandler InputHandler { get; private set; }

    protected Rigidbody2D Rigidbody2D { get; private set; }

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

      boxCollider2D = GetComponent<BoxCollider2D>();

      getGrounded = GetComponent<GetGrounded>();
    }

    private void OnEnable()
    {
      objectInteraction.OnInteractCharacter += GetInCar;

      InputHandler.AI_Player.Player.Move.performed += Move_performed;

      InputHandler.AI_Player.Vehicle.Space.performed += Space_performed;

      OnGetInCar += VehicleController_OnGetInCar;
      OnGetOutCar += VehicleController_OnGetOutCar;
    }

    private void OnDisable()
    {
      objectInteraction.OnInteractCharacter -= GetInCar;

      InputHandler.AI_Player.Player.Select.performed -= Select_performed;

      InputHandler.AI_Player.Player.Move.performed -= Move_performed;

      InputHandler.AI_Player.Vehicle.Space.performed -= Space_performed;

      OnGetInCar -= VehicleController_OnGetInCar;
      OnGetOutCar -= VehicleController_OnGetOutCar;
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {
      Move();

      if (getGrounded)
      {
        getGrounded.GetGround(boxCollider2D, out isGrounded);
      }
    }

    //===================================

    public abstract void Move();

    //===================================

    /// <summary>
    /// Сесть в машину
    /// </summary>
    public void GetInCar(Character parOldCharacter)
    {
      if (isInCar)
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
      _currentCharacter.CinemachineCamera.Target.TrackingTarget = transform;

      #endregion

      levelManager.ChangeCharacter(_currentCharacter);

      OnGetInCar?.Invoke();
    }

    /// <summary>
    /// Выйти из машины
    /// </summary>
    public void GetOutCar()
    {
      if (!isInCar)
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

    private void Flip()
    {
      if (!isInCar)
        return;

      if (!isGrounded)
        return;

      int input = InputHandler.GetInputVehicleFlip();

      if (input < 0)
        transform.localRotation = Quaternion.Euler(0, 180, 0);
      else if (input > 0)
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void Space_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
      if (!isInCar)
        return;

      if (!isGrounded)
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
      }

      //int input = InputHandler.GetInputVehicleFlip();

      /*if (input < 0)
        transform.localRotation = Quaternion.Euler(0, 180, 0);
      else if (input > 0)
        transform.localRotation = Quaternion.Euler(0, 0, 0);*/
    }

    //===================================

    private void VehicleController_OnGetInCar()
    {
      isInCar = true;

      InputHandler.AI_Player.Player.Select.performed += Select_performed;
    }

    private void VehicleController_OnGetOutCar()
    {
      isInCar = false;
    }

    private void Select_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
      GetOutCar();

      InputHandler.AI_Player.Player.Select.performed -= Select_performed;
    }

    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
      if (!isInCar || !isGrounded)
        return;

      if (Mathf.RoundToInt(obj.ReadValue<Vector2>().x) > 0 || Mathf.RoundToInt(obj.ReadValue<Vector2>().x) < 0)
        return;

      if (Mathf.RoundToInt(obj.ReadValue<Vector2>().y) < 0)
        return;

      animator.SetTrigger("IsMove");
    }

    //===================================
  }
}
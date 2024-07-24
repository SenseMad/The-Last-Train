using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

using TLT.CharacterManager;
using TLT.Weapons;
using UnityEngine.SceneManagement;

namespace TLT.Vehicles.Bike
{
  public class BikeBody : MonoBehaviour
  {
    [SerializeField] private BikeController _bikeController;
    [SerializeField] private BikeManager _bikeManager;
    [SerializeField] private WeaponController _weaponController;

    [Space]
    [SerializeField] private BikeData _bikeData;

    [Space]
    [SerializeField] private ObjectInteraction _objectInteraction;

    [Space]
    [SerializeField] private GameObject _objectBody;
    [SerializeField] private GameObject _objectCharacterBody;

    [Space]
    [SerializeField] private Animator _dustAnimator;

    [Space]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _soundStartingUp;

    //-----------------------------------

    private Rigidbody2D bodyRB;

    private float groundHandicap = 1f;

    private Character character;

    private LevelManager levelManager;

    private Weapon oldCharacterWeapon;

    //===================================

    public float FloorAngle
    {
      get
      {
        return Mathf.Abs(Vector2.Angle(transform.right, Vector2.right));
      }
    }

    public float VelocityMagnitude
    {
      get
      {
        return bodyRB.velocity.magnitude;
      }
    }

    public float WheelLiftAngle { get; private set; }

    public BikeController BikeController => _bikeController;
    public BikeManager BikeManager => _bikeManager;

    public GameObject ObjectBody => _objectBody;
    
    public GameObject ObjectCharacterBody => _objectCharacterBody;

    public Rigidbody2D BodyRB { get => bodyRB; set => bodyRB = value; }

    public WeaponController WeaponController { get => _weaponController; set => _weaponController = value; }

    public BikeData BikeData { get => _bikeData; private set => _bikeData = value; }

    //===================================

    public event Action OnGetInCar;
    public event Action OnGetOutCar;

    //===================================

    [Inject]
    private void Construct(Character parCharacter, LevelManager parLevelManager)
    {
      character = parCharacter;
      levelManager = parLevelManager;
    }

    //===================================

    private void Awake()
    {
      bodyRB = GetComponent<Rigidbody2D>();

      /*if (SceneManager.GetActiveScene().name != $"{NamesScenes.Hub_scene}")
        VehicleController_OnGetInCar();*/
    }

    private void OnEnable()
    {
      _objectInteraction.OnInteract += GetInCar;

      _bikeController.InputHandler.AI_Player.Vehicle.Throttle.performed += Throttle;

      OnGetInCar += VehicleController_OnGetInCar;
      OnGetOutCar += VehicleController_OnGetOutCar;
    }

    private void OnDisable()
    {
      _objectInteraction.OnInteract -= GetInCar;

      _bikeController.InputHandler.AI_Player.Player.Select.performed -= Select_performed;

      _bikeController.InputHandler.AI_Player.Vehicle.Throttle.performed -= Throttle;

      OnGetInCar -= VehicleController_OnGetInCar;
      OnGetOutCar -= VehicleController_OnGetOutCar;
    }

    private void FixedUpdate()
    {
      if (!_bikeController.IsInCar)
        return;

      if (_bikeController.Character != null)
        _bikeController.Character.transform.position = transform.position;

      UpdateBalance();
      UpdateVelocity();

      UpdateAnimation();

      Balance();
    }

    //===================================

    public void GetInCar()
    {
      if (_bikeController.IsInCar)
        return;

      CallEventOnGetInCar();
    }

    public void GetOutCar()
    {
      if (!_bikeController.IsInCar)
        return;

      CallEventOnGetOutCar();
    }

    public void CallEventOnGetInCar()
    {
      OnGetInCar?.Invoke();
    }

    public void CallEventOnGetOutCar()
    {
      OnGetOutCar?.Invoke();
    }

    public void ChangeDirection()
    {
      if (!_bikeController.IsInCar)
        return;

      if (!_bikeManager.Grounded)
        return;

      _bikeController.Animator.SetTrigger("IsFlip");

      _bikeController.IsFlip = true;
    }
    
    private void FlipAnimation()
    {
      _bikeController.IsFlip = false;

      transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);

      _bikeManager.Direction *= -1;

      _bikeController.Character.Direction = _bikeManager.Direction;

      //Debug.Log($"{_bikeController.IsInCar}");
    }

    public int GetDirection()
    {
      return _bikeManager.Direction;
    }

    public void SetFrontWhelieCOM()
    {
      bodyRB.centerOfMass = _bikeManager.FrontWheel.transform.localPosition;
    }

    //===================================

    private void SoundSFX(AudioClip audioClip, bool parLoop)
    {
      _audioSource.clip = audioClip;
      _audioSource.loop = parLoop;
      _audioSource.Play();
    }

    private void StopSoundSFX()
    {
      _audioSource.Stop();
      _audioSource.clip = null;
    }

    private void UpdateVelocity()
    {
      ImpulseBody();
      if (_bikeManager.AnyWheelGrounded)
      {
        if (bodyRB.velocity.magnitude > _bikeData.MaxVelocity)
        {
          bodyRB.velocity = bodyRB.velocity.normalized * _bikeData.MaxVelocity;
        }
      }
      else if (bodyRB.velocity.magnitude > _bikeData.MaxVelocity * 1.5f)
      {
        bodyRB.velocity = bodyRB.velocity.normalized * _bikeData.MaxVelocity * 1.5f;
      }

      if (_bikeController.Throttle == 0 && _bikeManager.Grounded && _bikeController.Brake != 0)
      {
        // ForceBrake();
        bodyRB.velocity = Vector2.Lerp(bodyRB.velocity, new Vector2(0, bodyRB.velocity.y), Time.deltaTime * _bikeData.DefaultForceBrakeSpeed);
      }
    }

    public void ForceBrake()
    {
      _bikeManager.FrontWheel.ForceBrake();
      _bikeManager.BackWheel.ForceBrake();

      if (((_bikeManager.OnlyFrontGrounded || _bikeManager.Grounded) && FloorAngle < 45f) || _bikeManager.OnlyBackGrounded)
      {
        SetFrontWhelieCOM();
        bodyRB.AddTorque(-bodyRB.velocity.magnitude * 250f * (float)_bikeManager.Direction * bodyRB.mass * Time.deltaTime, ForceMode2D.Force);
      }
      else if (!_bikeManager.Grounded && FloorAngle > 45f)
      {
        bodyRB.AddTorque(FloorAngle * 100f * (float)_bikeManager.Direction * bodyRB.mass * Time.deltaTime, ForceMode2D.Force);
      }

      bodyRB.velocity = Vector2.Lerp(bodyRB.velocity, new Vector2(0, bodyRB.velocity.y), Time.deltaTime * _bikeData.DefaultForceBrakeSpeed);
    }

    private void ImpulseBody()
    {
      float d = _bikeController.ForceThrottle ? 1f : _bikeController.Throttle;
      Vector3 v = transform.right * _bikeManager.Direction * d * _bikeData.BodyForce * Time.deltaTime;

      if (_bikeManager.Grounded)
        bodyRB.AddForce(v, ForceMode2D.Force);
    }

    private void Balance()
    {
      if (_bikeController.Balance == 0)
        return;

      if (!_bikeManager.Grounded)
      {
        bodyRB.AddTorque(-_bikeController.Balance * _bikeData.SpinTorque);
        return;
      }

      if (_bikeController.Balance == -1)
        bodyRB.AddTorque(_bikeData.WheelieTorque);
      else if (_bikeController.Balance == 1)
        bodyRB.AddTorque(-_bikeData.WheelieTorque);
    }

    private void UpdateBalance()
    {
      UpdateTorque();
    }

    private void UpdateTorque()
    {
      float num = _bikeController.Balance * _bikeData.BalanceStrength * bodyRB.mass * Time.deltaTime;
      if (_bikeController.Balance == 0)
      {
        if (_bikeManager.Grounded)
          groundHandicap = 0.25f;
        else
          groundHandicap = 0.6f;
      }
      else
      {
        groundHandicap += Time.deltaTime * 2f;
        groundHandicap = Mathf.Clamp01(groundHandicap);
      }

      num *= groundHandicap;
      bodyRB.AddTorque(num, ForceMode2D.Force);

      bodyRB.angularVelocity = Mathf.Clamp(bodyRB.angularVelocity, -_bikeData.MaxAngularVelocity, _bikeData.MaxAngularVelocity);

      WheelLiftAngle = num;
    }

    private void UpdateAnimation()
    {
      float deviation = 0.2f;

      if ((_bikeManager.FrontWheel.WheelRB.velocity.x > deviation || _bikeManager.FrontWheel.WheelRB.velocity.x < -deviation) && 
          (_bikeManager.BackWheel.WheelRB.velocity.x > deviation || _bikeManager.BackWheel.WheelRB.velocity.x < -deviation))
      {
        _bikeController.Animator.SetBool("IsMove", true);
        //_bikeManager.CameraController.Zoom(false);
        return;
      }

      _bikeController.Animator.SetBool("IsMove", false);
      //_bikeManager.CameraController.Zoom(true);
    }

    //===================================

    private void VehicleController_OnGetInCar()
    {
      SoundSFX(_soundStartingUp, true);

      _bikeController.Character = character;
      _bikeController.CinemachineCamera.Target.TrackingTarget = transform;

      _bikeController.Character.transform.position = transform.position;

      oldCharacterWeapon = _bikeController.Character.WeaponController.CurrentWeapon;
      WeaponController.CurrentWeapon.GetWeaponData(oldCharacterWeapon.CurrentAmountAmmo, oldCharacterWeapon.CurrentAmountAmmoInMagazine);
      _bikeController.Character.WeaponController.CurrentWeapon = WeaponController.CurrentWeapon;
      levelManager.ChangeCharacter();

      _bikeController.Character.gameObject.SetActive(false);

      _bikeController.IsInCar = true;

      _objectBody.SetActive(false);
      _objectCharacterBody.SetActive(true);

      _bikeController.Character.Direction = GetDirection();

      _bikeController.InputHandler.AI_Player.Player.Select.performed += Select_performed;
    }

    private void VehicleController_OnGetOutCar()
    {
      StopSoundSFX();

      _bikeController.CinemachineCamera.Target.TrackingTarget = character.transform;

      _bikeController.Character.transform.position = transform.position;
      _bikeController.Character.gameObject.SetActive(true);

      Weapon currentWeapon = WeaponController.CurrentWeapon;
      oldCharacterWeapon.GetWeaponData(currentWeapon.CurrentAmountAmmo, currentWeapon.CurrentAmountAmmoInMagazine);
      _bikeController.Character.WeaponController.CurrentWeapon = oldCharacterWeapon;
      levelManager.ChangeCharacter();

      _bikeController.Character = null;

      _bikeController.IsInCar = false;

      _objectBody.SetActive(true);
      _objectCharacterBody.SetActive(false);
    }

    private void Select_performed(InputAction.CallbackContext parContext)
    {
      if (_bikeController.IsFlip)
        return;

      GetOutCar();

      _bikeController.InputHandler.AI_Player.Player.Select.performed -= Select_performed;
    }

    private void Throttle(InputAction.CallbackContext parContext)
    {
      if (!_bikeController.IsInCar || !_bikeManager.Grounded)
        return;

      if (_dustAnimator != null)
        _dustAnimator.SetTrigger("IsMoveDust");
    }

    //===================================
  }
}
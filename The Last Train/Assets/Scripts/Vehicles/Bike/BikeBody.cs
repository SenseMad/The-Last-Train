using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

using TLT.CharacterManager;
using TLT.Weapons;

namespace TLT.Bike.Bike
{
  public class BikeBody : MonoBehaviour, IBikeBootstrap
  {
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

    private BikeController bikeController;
    private BikeManager bikeManager;

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

    public BikeController BikeController => bikeController;
    public BikeManager BikeManager => bikeManager;

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

    public void CustomAwake()
    {
      bikeController = GetComponent<BikeController>();
      bikeManager = GetComponent<BikeManager>();

      bodyRB = GetComponent<Rigidbody2D>();

      /*if (SceneManager.GetActiveScene().name != $"{NamesScenes.Hub_scene}")
        VehicleController_OnGetInCar();*/
    }

    public void CustomStart() { }

    private void OnEnable()
    {
      _objectInteraction.OnInteract += GetInCar;

      bikeController.InputHandler.AI_Player.Vehicle.Throttle.performed += Throttle;

      OnGetInCar += VehicleController_OnGetInCar;
      OnGetOutCar += VehicleController_OnGetOutCar;

      bikeController.BikeEngine.OnStartEngine += BikeEngine_OnStartEngine;
      bikeController.BikeEngine.OnStopEngine += BikeEngine_OnStopEngine;

      bikeController.BalanceMiniGameManager.OnEndGame += VehicleController_OnGetOutCar;
    }

    private void OnDisable()
    {
      _objectInteraction.OnInteract -= GetInCar;

      bikeController.InputHandler.AI_Player.Player.Select.performed -= Select_performed;

      bikeController.InputHandler.AI_Player.Vehicle.Throttle.performed -= Throttle;

      OnGetInCar -= VehicleController_OnGetInCar;
      OnGetOutCar -= VehicleController_OnGetOutCar;

      bikeController.BikeEngine.OnStartEngine -= BikeEngine_OnStartEngine;
      bikeController.BikeEngine.OnStopEngine -= BikeEngine_OnStopEngine;

      bikeController.BalanceMiniGameManager.OnEndGame -= VehicleController_OnGetOutCar;
    }

    private void FixedUpdate()
    {
      if (!bikeController.IsInCar)
        return;

      if (bikeController.Character != null)
        bikeController.Character.transform.position = transform.position;

      UpdateBalance();
      UpdateVelocity();

      UpdateAnimation();

      Balance();
    }

    //===================================

    public void GetInCar()
    {
      if (bikeController.IsInCar)
        return;

      CallEventOnGetInCar();
    }

    public void GetOutCar()
    {
      if (!bikeController.IsInCar)
        return;

      if (bikeController.BalanceMiniGameManager.IsGameRunning)
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
      if (!bikeController.IsInCar)
        return;

      if (!bikeManager.Grounded)
        return;

      bikeManager.FrontWheel.transform.rotation = Quaternion.Euler(0, 0, 0);
      bikeManager.BackWheel.transform.rotation = Quaternion.Euler(0, 0, 0);

      bikeController.Animator.SetTrigger("IsFlip");

      bikeController.IsFlip = true;
    }
    
    private void FlipAnimation()
    {
      bikeController.IsFlip = false;

      transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);

      bikeManager.Direction *= -1;

      bikeController.Character.Direction = bikeManager.Direction;

      //Debug.Log($"{_bikeController.IsInCar}");
    }

    public int GetDirection()
    {
      return bikeManager.Direction;
    }

    public void SetFrontWhelieCOM()
    {
      bodyRB.centerOfMass = bikeManager.FrontWheel.transform.localPosition;
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
      if (bikeManager.AnyWheelGrounded)
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

      if (bikeController.Throttle == 0 && bikeManager.Grounded && bikeController.Brake != 0)
      {
        // ForceBrake();
        bodyRB.velocity = Vector2.Lerp(bodyRB.velocity, new Vector2(0, bodyRB.velocity.y), Time.deltaTime * _bikeData.DefaultForceBrakeSpeed);
      }
    }

    /*public void ForceBrake()
    {
      bikeManager.FrontWheel.ForceBrake();
      bikeManager.BackWheel.ForceBrake();

      if (((bikeManager.OnlyFrontGrounded || bikeManager.Grounded) && FloorAngle < 45f) || bikeManager.OnlyBackGrounded)
      {
        SetFrontWhelieCOM();
        bodyRB.AddTorque(-bodyRB.velocity.magnitude * 250f * (float)bikeManager.Direction * bodyRB.mass * Time.deltaTime, ForceMode2D.Force);
      }
      else if (!bikeManager.Grounded && FloorAngle > 45f)
      {
        bodyRB.AddTorque(FloorAngle * 100f * (float)bikeManager.Direction * bodyRB.mass * Time.deltaTime, ForceMode2D.Force);
      }

      bodyRB.velocity = Vector2.Lerp(bodyRB.velocity, new Vector2(0, bodyRB.velocity.y), Time.deltaTime * _bikeData.DefaultForceBrakeSpeed);
    }*/

    private void ImpulseBody()
    {
      float d = bikeController.ForceThrottle ? 1f : bikeController.Throttle;
      Vector3 v = transform.right * bikeManager.Direction * d * _bikeData.BodyForce * Time.deltaTime;

      if (bikeManager.Grounded)
        bodyRB.AddForce(v, ForceMode2D.Force);
    }

    private void Balance()
    {
      if (bikeController.Balance == 0)
        return;

      if (!bikeManager.Grounded)
      {
        bodyRB.AddTorque(-bikeController.Balance * _bikeData.SpinTorque);
        return;
      }

      if (bikeController.Balance == -1)
        bodyRB.AddTorque(_bikeData.WheelieTorque);
      else if (bikeController.Balance == 1)
        bodyRB.AddTorque(-_bikeData.WheelieTorque);
    }

    private void UpdateBalance()
    {
      UpdateTorque();
    }

    private void UpdateTorque()
    {
      float num = bikeController.Balance * _bikeData.BalanceStrength * bodyRB.mass * Time.deltaTime;
      if (bikeController.Balance == 0)
      {
        if (bikeManager.Grounded)
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

      if ((bikeManager.FrontWheel.WheelRB.velocity.x > deviation || bikeManager.FrontWheel.WheelRB.velocity.x < -deviation) && 
          (bikeManager.BackWheel.WheelRB.velocity.x > deviation || bikeManager.BackWheel.WheelRB.velocity.x < -deviation))
      {
        bikeController.Animator.SetBool("IsMove", true);
        //_bikeManager.CameraController.Zoom(false);
        return;
      }

      bikeController.Animator.SetBool("IsMove", false);
      //_bikeManager.CameraController.Zoom(true);
    }

    //===================================

    private void VehicleController_OnGetInCar()
    {
      bikeController.Character = character;
      bikeController.CinemachineCamera.Target.TrackingTarget = transform;

      bikeController.Character.transform.position = transform.position;

      oldCharacterWeapon = bikeController.Character.WeaponController.CurrentWeapon;
      WeaponController.CurrentWeapon.GetWeaponData(oldCharacterWeapon.CurrentAmountAmmo, oldCharacterWeapon.CurrentAmountAmmoInMagazine);
      bikeController.Character.WeaponController.CurrentWeapon = WeaponController.CurrentWeapon;
      levelManager.ChangeCharacter();

      bikeController.Character.gameObject.SetActive(false);

      bikeController.IsInCar = true;

      _objectBody.SetActive(false);
      _objectCharacterBody.SetActive(true);

      bikeController.Character.Direction = GetDirection();

      bikeController.InputHandler.AI_Player.Player.Select.performed += Select_performed;
    }

    private void VehicleController_OnGetOutCar()
    {
      bikeController.CinemachineCamera.Target.TrackingTarget = character.transform;

      bikeController.Character.transform.position = transform.position;
      bikeController.Character.gameObject.SetActive(true);

      Weapon currentWeapon = WeaponController.CurrentWeapon;
      oldCharacterWeapon.GetWeaponData(currentWeapon.CurrentAmountAmmo, currentWeapon.CurrentAmountAmmoInMagazine);
      bikeController.Character.WeaponController.CurrentWeapon = oldCharacterWeapon;
      levelManager.ChangeCharacter();

      bikeController.Character = null;

      bikeController.IsInCar = false;

      _objectBody.SetActive(true);
      _objectCharacterBody.SetActive(false);
    }

    private void Select_performed(InputAction.CallbackContext parContext)
    {
      if (bikeController.IsFlip)
        return;

      GetOutCar();

      bikeController.InputHandler.AI_Player.Player.Select.performed -= Select_performed;
    }

    private void Throttle(InputAction.CallbackContext parContext)
    {
      if (!bikeController.IsInCar || !bikeManager.Grounded)
        return;

      if (_dustAnimator != null)
        _dustAnimator.SetTrigger("IsMoveDust");
    }

    private void BikeEngine_OnStartEngine()
    {
      SoundSFX(_soundStartingUp, true);
    }

    private void BikeEngine_OnStopEngine()
    {
      StopSoundSFX();
    }

    //===================================
  }
}
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

using TLT.CharacterManager;
using TLT.Input;

namespace TLT.Vehicles.Bike
{
  public sealed class BikeController : MonoBehaviour
  {
    [SerializeField] private BikeBody _bikeBody;

    //-----------------------------------

    private float balance;
    private float balanceDeadzoneMin = 0.35f;
    private float balanceDeadzoneMax = 0.925f;

    //===================================

    public Character Character { get; set; }

    public InputHandler InputHandler { get; private set; }

    public Animator Animator { get; private set; }

    public CinemachineCamera CinemachineCamera { get; set; }


    public bool IsInCar { get; set; }

    public float Throttle { get; private set; }

    public bool ForceThrottle { get; private set; }

    public float Brake { get; private set; }

    public float Balance => GetBalance();

    //===================================

    [Inject]
    private void Construct(InputHandler parInputHandler, CinemachineCamera parCinemachineCamera)
    {
      InputHandler = parInputHandler;
      CinemachineCamera = parCinemachineCamera;
    }

    //===================================

    private void Awake()
    {
      Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
      InputHandler.AI_Player.Vehicle.Throttle.started += OnThrottle;
      InputHandler.AI_Player.Vehicle.Throttle.performed += OnThrottle;
      InputHandler.AI_Player.Vehicle.Throttle.canceled += OnThrottle;

      InputHandler.AI_Player.Vehicle.Brake.started += OnBrake;
      InputHandler.AI_Player.Vehicle.Brake.performed += OnBrake;
      InputHandler.AI_Player.Vehicle.Brake.canceled += OnBrake;

      InputHandler.AI_Player.Vehicle.Balance.started += OnBalance;
      InputHandler.AI_Player.Vehicle.Balance.performed += OnBalance;
      InputHandler.AI_Player.Vehicle.Balance.canceled += OnBalance;

      InputHandler.AI_Player.Vehicle.Space.performed += OnChangeDirection;
    }

    private void OnDisable()
    {
      InputHandler.AI_Player.Vehicle.Throttle.started -= OnThrottle;
      InputHandler.AI_Player.Vehicle.Throttle.performed -= OnThrottle;
      InputHandler.AI_Player.Vehicle.Throttle.canceled -= OnThrottle;

      InputHandler.AI_Player.Vehicle.Brake.started -= OnBrake;
      InputHandler.AI_Player.Vehicle.Brake.performed -= OnBrake;
      InputHandler.AI_Player.Vehicle.Brake.canceled -= OnBrake;

      InputHandler.AI_Player.Vehicle.Balance.started -= OnBalance;
      InputHandler.AI_Player.Vehicle.Balance.performed -= OnBalance;
      InputHandler.AI_Player.Vehicle.Balance.canceled -= OnBalance;

      InputHandler.AI_Player.Vehicle.Space.performed -= OnChangeDirection;
    }

    //===================================

    public float GetBalance()
    {
      if (Mathf.Abs(balance) < balanceDeadzoneMin)
        return 0;

      if (Mathf.Abs(balance) > balanceDeadzoneMax)
        return Mathf.Sign(balance);

      return Mathf.Sign(balance) * Mathf.Abs(balance).Remap(balanceDeadzoneMin, balanceDeadzoneMax, 0, 1);
    }

    //===================================

    private void OnThrottle(InputAction.CallbackContext parContext)
    {
      if (!IsInCar)
      {
        Throttle = 0;
        return;
      }

      Throttle = parContext.ReadValue<float>();
    }

    private void OnBrake(InputAction.CallbackContext parContext)
    {
      if (!IsInCar)
      {
        Brake = 0;
        return;
      }

      Brake = parContext.ReadValue<float>();
    }

    private void OnBalance(InputAction.CallbackContext parContext)
    {
      if (!IsInCar)
      {
        balance = 0;
        return;
      }

      balance = parContext.ReadValue<float>();
    }

    private void OnChangeDirection(InputAction.CallbackContext context)
    {
      if (!IsInCar)
        return;

      _bikeBody.ChangeDirection();
    }

    //===================================
  }
}
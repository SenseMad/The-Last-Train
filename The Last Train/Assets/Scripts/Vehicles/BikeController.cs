using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

using TLT.CharacterManager;
using TLT.Input;

namespace TLT.Bike.Bike
{
  public sealed class BikeController : MonoBehaviour, IBikeBootstrap
  {
    private float balance;
    private float balanceDeadzoneMin = 0.35f;
    private float balanceDeadzoneMax = 0.925f;

    //===================================

    public Character Character { get; set; }

    public InputHandler InputHandler { get; private set; }

    public Animator Animator { get; private set; }

    public CinemachineCamera CinemachineCamera { get; set; }

    public BikeBody BikeBody { get; private set; }
    public BikeManager BikeManager { get; private set; }
    public BikeEngine BikeEngine { get; private set; }
    public BalanceMiniGameManager BalanceMiniGameManager { get; private set; }


    public bool IsInCar { get; set; }
    public bool IsFlip { get; set; }
    public bool IsMoving { get; private set; }

    public float Throttle { get; private set; }

    public bool ForceThrottle { get; private set; }

    public float Brake { get; private set; }

    public float Space { get; private set; }

    public float Balance => GetBalance();

    //===================================

    [Inject]
    private void Construct(InputHandler parInputHandler, CinemachineCamera parCinemachineCamera)
    {
      InputHandler = parInputHandler;
      CinemachineCamera = parCinemachineCamera;
    }

    //===================================

    public void CustomAwake()
    {
      BikeBody = GetComponent<BikeBody>();
      BikeManager = GetComponent<BikeManager>();
      BikeEngine = GetComponent<BikeEngine>();
      BalanceMiniGameManager = GetComponentInChildren<BalanceMiniGameManager>(true);

      Animator = GetComponent<Animator>();
    }

    public void CustomStart() { }

    private void OnEnable()
    {
      InputHandler.AI_Player.Vehicle.Move.started += Move_started;
      InputHandler.AI_Player.Vehicle.Move.performed += Move_started;
      InputHandler.AI_Player.Vehicle.Move.canceled += Move_started;

      InputHandler.AI_Player.Vehicle.Space.started += Space_performed;
      InputHandler.AI_Player.Vehicle.Space.performed += Space_performed;
      InputHandler.AI_Player.Vehicle.Space.canceled += Space_performed;

      InputHandler.AI_Player.Vehicle.Balance.started += OnBalance;
      InputHandler.AI_Player.Vehicle.Balance.performed += OnBalance;
      InputHandler.AI_Player.Vehicle.Balance.canceled += OnBalance;

      InputHandler.AI_Player.Vehicle.Engine.performed += OnEngineRunning;

      /*InputHandler.AI_Player.Vehicle.Throttle.started += OnThrottle;
      InputHandler.AI_Player.Vehicle.Throttle.performed += OnThrottle;
      InputHandler.AI_Player.Vehicle.Throttle.canceled += OnThrottle;*/

      /*InputHandler.AI_Player.Vehicle.Brake.started += OnBrake;
      InputHandler.AI_Player.Vehicle.Brake.performed += OnBrake;
      InputHandler.AI_Player.Vehicle.Brake.canceled += OnBrake;*/

      //InputHandler.AI_Player.Vehicle.Space.performed += OnChangeDirection;
    }

    private void OnDisable()
    {
      InputHandler.AI_Player.Vehicle.Move.started -= Move_started;
      InputHandler.AI_Player.Vehicle.Move.performed -= Move_started;
      InputHandler.AI_Player.Vehicle.Move.canceled -= Move_started;

      InputHandler.AI_Player.Vehicle.Space.started -= Space_performed;
      InputHandler.AI_Player.Vehicle.Space.performed -= Space_performed;
      InputHandler.AI_Player.Vehicle.Space.canceled -= Space_performed;

      InputHandler.AI_Player.Vehicle.Balance.started -= OnBalance;
      InputHandler.AI_Player.Vehicle.Balance.performed -= OnBalance;
      InputHandler.AI_Player.Vehicle.Balance.canceled -= OnBalance;

      InputHandler.AI_Player.Vehicle.Engine.performed -= OnEngineRunning;

      /*InputHandler.AI_Player.Vehicle.Throttle.started -= OnThrottle;
      InputHandler.AI_Player.Vehicle.Throttle.performed -= OnThrottle;
      InputHandler.AI_Player.Vehicle.Throttle.canceled -= OnThrottle;*/

      /*InputHandler.AI_Player.Vehicle.Brake.started -= OnBrake;
      InputHandler.AI_Player.Vehicle.Brake.performed -= OnBrake;
      InputHandler.AI_Player.Vehicle.Brake.canceled -= OnBrake;*/

      //InputHandler.AI_Player.Vehicle.Space.performed -= OnChangeDirection;
    }

    private void FixedUpdate()
    {
      if (!IsFlip && BikeManager.Grounded)
      {
        if (Throttle > 0 && BikeManager.Direction == 1)
        {
          BikeBody.ChangeDirection();
        }
        else if (Throttle < 0 && BikeManager.Direction == -1)
        {
          BikeBody.ChangeDirection();
        }
      }

      if (BikeBody.BodyRB.velocity.magnitude < 0.01f)
      {
        BikeBody.BodyRB.velocity = Vector2.zero;
      }

      if (BikeBody.BodyRB.velocity.x == 0)
      {
        IsMoving = false;
        return;
      }

      IsMoving = true;
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

    private void Space_performed(InputAction.CallbackContext parContext)
    {
      if (!IsInCar)
      {
        Space = 0;
        return;
      }

      if (BalanceMiniGameManager.IsGameRunning)
      {
        Space = 0;
        return;
      }

      Space = parContext.ReadValue<float>();
      Brake = parContext.ReadValue<float>();
    }

    private void Move_started(InputAction.CallbackContext parContext)
    {
      if (!IsInCar)
      {
        Throttle = 0;
        return;
      }

      if (!BikeEngine.IsEngineRunning)
      {
        Throttle = 0;
        return;
      }

      if (IsFlip)
      {
        Throttle = 0;
        return;
      }

      if (BalanceMiniGameManager.IsGameRunning)
      {
        Throttle = 0;
        return;
      }

      Throttle = -parContext.ReadValue<Vector2>().x;

      if (Space > 0)
      {
        Throttle = 0;
        return;
      }
    }

    private void OnThrottle(InputAction.CallbackContext parContext)
    {
      if (!IsInCar)
      {
        //Throttle = 0;
        return;
      }

      if (!BikeEngine.IsEngineRunning)
      {
        //Throttle = 0;
        return;
      }

      if (IsFlip)
      {
        //Throttle = 0;
        return;
      }

      if (BalanceMiniGameManager.IsGameRunning)
      {
        //Throttle = 0;
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

      if (BalanceMiniGameManager.IsGameRunning)
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

      if (!BikeEngine.IsEngineRunning)
      {
        balance = 0;
        return;
      }

      if (IsFlip)
      {
        balance = 0;
        return;
      }

      if (BalanceMiniGameManager.IsGameRunning)
      {
        balance = 0;
        return;
      }

      if (!BikeManager.Grounded && !BikeManager.OnlyBackGrounded && !BikeManager.OnlyFrontGrounded)
      {
        balance = parContext.ReadValue<Vector2>().y;
        return;
      }

      if (Space <= 0)
      {
        balance = 0;
        return;
      }

      balance = parContext.ReadValue<Vector2>().x;
    }

    private void OnEngineRunning(InputAction.CallbackContext parContext)
    {
      if (!IsInCar)
        return;

      if (IsFlip)
        return;

      if (BalanceMiniGameManager.IsGameRunning)
        return;

      if (!BikeEngine.IsEngineRunning)
        BikeEngine.StartEngine();
    }

    private void OnChangeDirection(InputAction.CallbackContext context)
    {
      if (!IsInCar)
        return;

      if (!BikeEngine.IsEngineRunning)
        return;

      if (IsFlip)
        return;

      if (BalanceMiniGameManager.IsGameRunning)
        return;

      BikeBody.ChangeDirection();
    }

    //===================================
  }
}
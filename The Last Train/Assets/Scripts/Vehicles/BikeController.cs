using UnityEngine;
using UnityEngine.InputSystem;

namespace TLT.Vehicles.Bike
{
  public sealed class BikeController : VehicleController
  {
    [SerializeField] private BikeBody _bikeBody;

    //-----------------------------------

    private float balance;
    private float balanceDeadzoneMin = 0.35f;
    private float balanceDeadzoneMax = 0.925f;

    //===================================

    public float Throttle { get; private set; }

    public bool ForceThrottle { get; private set; }

    public float Brake { get; private set; }

    public float Balance => GetBalance();

    //===================================

    protected override void OnEnable()
    {
      base.OnEnable();

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

    protected override void OnDisable()
    {
      base.OnDisable();

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

    protected override void Update()
    {
      base.Update();

      /*if (_frontWheel != null && _backWheel != null)
        IsGrounded = _frontWheel.IsGrounded() || _backWheel.IsGrounded();*/
    }

    protected override void FixedUpdate()
    {
      //base.FixedUpdate();

      //Move1();
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
        return;

      Throttle = parContext.ReadValue<float>();
    }

    private void OnBrake(InputAction.CallbackContext parContext)
    {
      if (!IsInCar)
        return;

      Brake = parContext.ReadValue<float>();
    }

    private void OnBalance(InputAction.CallbackContext parContext)
    {
      if (!IsInCar)
        return;

      balance = parContext.ReadValue<float>();
    }

    private void OnChangeDirection(InputAction.CallbackContext context)
    {
      if (!IsInCar)
        return;

      _bikeBody.ChangeDirection();
    }

    //===================================

    public override void Move()
    {

    }

    //===================================
  }
}
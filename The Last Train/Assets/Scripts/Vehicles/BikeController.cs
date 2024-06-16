using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Splines.SplineInstantiate;

namespace TLT.Vehicles.Bike
{
  public sealed class BikeController : VehicleController
  {
    [SerializeField] private BikeBody _bikeBody;

    /*[Space]
    [SerializeField] private BikeWheel _frontWheel;
    [SerializeField] private BikeWheel _backWheel;*/

    /*[Space(10)]
    [SerializeField] private Rigidbody2D _frontWheel1;
    [SerializeField] private Rigidbody2D _backWheel1;
    [SerializeField] private Rigidbody2D _bike;*/

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
      /*int input = IsInCar && IsGrounded ? InputHandler.GetInputVertical() : 0;

      // Определяем направление взгляда
      bool facingRight = transform.localRotation.eulerAngles.y == 0;
      if (!facingRight)
        input = -input;

      float targetSpeed = _vehicleData.Speed * input;

      // Определяем, движемся ли задним ходом
      bool isMovingBackward = (facingRight && targetSpeed < 0) || (!facingRight && targetSpeed > 0);
      if (isMovingBackward)
        targetSpeed = _vehicleData.ReverseSpeed * input;

      float acceleration = _vehicleData.Speed / _vehicleData.AccelerationTimeMaxSpeed;
      float deceleration = _vehicleData.Speed / _vehicleData.DeccelerationTime;

      float currentSpeed = Rigidbody2D.velocity.x;
      if ((currentSpeed > 0 && input < 0) || (currentSpeed < 0 && input > 0))
        deceleration *= 2;

      float speedChangeRate = (Mathf.Abs(input) > 0) ? acceleration : deceleration;

      float moveVelocity = Mathf.MoveTowards(currentSpeed, targetSpeed, speedChangeRate * Time.deltaTime);

      Vector2 targetVelocity = new(moveVelocity, Rigidbody2D.velocity.y);
      Rigidbody2D.velocity = targetVelocity;*/
    }

    /*public void Move1()
    {
      int input = IsInCar && IsGrounded ? isCurrentRightFlip ? -InputHandler.GetInputVertical() : InputHandler.GetInputVertical() : 0;

      float adjustedMotorSpeed = input < 0 ? _vehicleData.Speed / 2 : _vehicleData.Speed;

      if (input == 0)
      {
        ApplyBraking(_frontWheel1);
        ApplyBraking(_backWheel1);
      }
      else
      {
        if (input != 0)
        {
          _frontWheel1.AddTorque(input * adjustedMotorSpeed * Time.fixedDeltaTime);
          _backWheel1.AddTorque(input * adjustedMotorSpeed * Time.fixedDeltaTime);
          _bike.AddTorque(input * -50 * Time.fixedDeltaTime);

          if (_bike.velocity.magnitude > _vehicleData.MaxSpeed)
          {
            _bike.velocity = _bike.velocity.normalized * _vehicleData.MaxSpeed;
          }
        }
      }
    }

    private void ApplyBraking(Rigidbody2D wheel)
    {
      // Демпфирование угловой скорости
      wheel.angularVelocity = Mathf.Lerp(wheel.angularVelocity, 0, _vehicleData.DeccelerationTime * Time.fixedDeltaTime);

      // Полная остановка, если угловая скорость достаточно низкая
      if (Mathf.Abs(wheel.angularVelocity) < 0.1f)
      {
        wheel.angularVelocity = 0;
      }
    }*/

    //===================================
  }
}
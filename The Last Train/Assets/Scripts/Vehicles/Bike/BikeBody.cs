using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLT.Vehicles.Bike
{
  public class BikeBody : MonoBehaviour
  {
    [Header("Controllers")]
    [SerializeField] private BikeController _bikeController;
    [SerializeField] private BikeManager _bikeManager;

    [Space]
    [Header("Speed")]
    [SerializeField] private float _bodyForce = 100f;
    [SerializeField] private float _maxVelocity = 14f;
    [SerializeField] private float _defaultForceBrakeSpeed = 50f;

    [Space, Header("Balance")]
    [SerializeField] private float _wheelieTorque = 50f;
    [SerializeField] private float _spinTorque = 5f;

    [SerializeField] private float _balanceStrength = 1f;
    [SerializeField] private float _maxAngularVelocity = 5f;
    [SerializeField] private float _tempBalanceSignTimer;
    [SerializeField] private float _tempBalanceSignTime = 0.3f;
    [SerializeField] private float _balanceLianaStrenght = 1f;

    //-----------------------------------

    private Rigidbody2D bodyRB;

    private float groundHandicap = 1f;

    //===================================

    public Vector2 Velocity => bodyRB.velocity;

    public float MaxVelocity => _maxVelocity;

    public float FloorAngle => Mathf.Abs(Vector2.Angle(transform.right, Vector2.right));

    public Rigidbody2D BodyRB { get => bodyRB; set => bodyRB = value; }

    public float WheelieEasinessMultiplier { get; set; }
    public float tempBalanceSign { get; set; }

    //===================================

    private void Awake()
    {
      bodyRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
      if (!_bikeController.IsInCar)
        return;

      UpdateBalance();
      UpdateVelocity();

      Balance();
    }

    //===================================

    public void ChangeDirection()
    {
      if (!_bikeController.IsInCar)
        return;

      if (!_bikeManager.Grounded)
        return;

      transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);

      _bikeManager.Direction *= -1;

      _bikeController.CurrentCharacter.Direction = _bikeManager.Direction;
    }

    public void SetFrontWhelieCOM()
    {
      bodyRB.centerOfMass = _bikeManager.FrontWheel.transform.localPosition;
    }

    //===================================

    private void UpdateVelocity()
    {
      ImpulseBody();
      if (_bikeManager.AnyWheelGrounded)
      {
        if (bodyRB.velocity.magnitude > _maxVelocity)
        {
          bodyRB.velocity = bodyRB.velocity.normalized * _maxVelocity;
        }
      }
      else if (bodyRB.velocity.magnitude > _maxVelocity * 1.5f)
      {
        bodyRB.velocity = bodyRB.velocity.normalized * _maxVelocity * 1.5f;
      }

      if (_bikeController.Throttle == 0 && _bikeManager.Grounded && _bikeController.Brake != 0)
      {
        //ForceBrake();
      }
    }

    private void ForceBrake()
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

      bodyRB.velocity = Vector2.Lerp(bodyRB.velocity, new Vector2(0, bodyRB.velocity.y), Time.deltaTime * _defaultForceBrakeSpeed);
    }

    private void ImpulseBody()
    {
      float d = _bikeController.ForceThrottle ? 1f : _bikeController.Throttle;
      Vector3 v = transform.right * _bikeManager.Direction * d * _bodyForce * Time.deltaTime;

      if (_bikeManager.Grounded)
        bodyRB.AddForce(v, ForceMode2D.Force);
    }

    private void Balance()
    {
      if (_bikeController.Balance == 0)
        return;

      if (!_bikeManager.Grounded)
      {
        bodyRB.AddTorque(-_bikeController.Balance * _spinTorque);
        return;
      }

      if (_bikeController.Balance == -1)
        bodyRB.AddTorque(_wheelieTorque);
      else if (_bikeController.Balance == 1)
        bodyRB.AddTorque(-_wheelieTorque);
    }

    private void UpdateBalance()
    {
      UpdateTorque();
    }

    private void UpdateTorque()
    {
      float num = _bikeController.Balance * _balanceStrength * bodyRB.mass * Time.deltaTime;
      if (_bikeController.Balance == 0)
      {
        if (_bikeController.IsGrounded)
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

      bodyRB.angularVelocity = Mathf.Clamp(bodyRB.angularVelocity, -_maxAngularVelocity, _maxAngularVelocity);
      if (Mathf.Abs(num) >= 0.2f)
      {
        tempBalanceSign = Mathf.Sign(num);
        _tempBalanceSignTimer = 0;
        return;
      }

      _tempBalanceSignTimer += Time.deltaTime;
      if (_tempBalanceSignTimer >= _tempBalanceSignTime)
      {
        _tempBalanceSignTimer = 0;
        tempBalanceSign = 0;
      }
    }

    //===================================
  }
}
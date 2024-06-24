using UnityEngine;

namespace TLT.Vehicles.Bike
{
  public class BikeBody : MonoBehaviour
  {
    [Header("Controllers")]
    [SerializeField] private BikeController _bikeController;
    [SerializeField] private BikeManager _bikeManager;

    [Space]
    [SerializeField] private BikeData _bikeData;

    //-----------------------------------

    private Rigidbody2D bodyRB;

    private float groundHandicap = 1f;

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

    public BikeController BikeController => _bikeController;
    public BikeManager BikeManager => _bikeManager;

    public Rigidbody2D BodyRB { get => bodyRB; set => bodyRB = value; }

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

      UpdateAnimation();

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
    }

    private void UpdateAnimation()
    {
      float deviation = 0.2f;

      if ((_bikeManager.FrontWheel.WheelRB.velocity.x > deviation || _bikeManager.FrontWheel.WheelRB.velocity.x < -deviation) && 
          (_bikeManager.BackWheel.WheelRB.velocity.x > deviation || _bikeManager.BackWheel.WheelRB.velocity.x < -deviation))
      {
        _bikeController.Animator.SetBool("IsMove", true);
        _bikeManager.CameraController.Zoom(false);
        return;
      }

      _bikeController.Animator.SetBool("IsMove", false);
      _bikeManager.CameraController.Zoom(true);
    }

    //===================================
  }
}
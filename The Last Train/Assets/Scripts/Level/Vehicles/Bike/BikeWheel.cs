using System;
using UnityEngine;

namespace TLT.Bike.Bike
{
  public class BikeWheel : MonoBehaviour, IBikeBootstrap
  {
    [Header("Characteristic")]
    [SerializeField, Min(0)] private float power = 100f;
    [SerializeField, Min(0)] private float _brakePower = 1000f;

    [Header("Floor Detection")]
    [SerializeField, Min(0)] private float _groundDetectionDistance = 1.3f;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Transform _groundDetector;

    //-----------------------------------

    private BikeController bikeController;
    private BikeDash bikeDash;

    private Rigidbody2D wheelRB;

    private RaycastHit2D groundHit;
    private bool grounded;

    private bool wasGrounded;

    //===================================

    public CircleCollider2D Collider2D { get; private set; }

    //-----------------------------------

    public Rigidbody2D WheelRB => wheelRB;

    public bool Grounded => grounded;

    public RaycastHit2D GroundHit => groundHit;

    //===================================

    public event Action OnLanded;

    //===================================

    public void CustomAwake()
    {
      bikeController = GetComponentInParent<BikeController>();
      bikeDash = GetComponentInParent<BikeDash>();

      wheelRB = GetComponent<Rigidbody2D>();

      Collider2D = GetComponent<CircleCollider2D>();
    }

    public void CustomStart()
    {
      _groundDetectionDistance *= Collider2D.radius;
    }

    private void Update()
    {
      if (bikeController.BikeFlip.IsFlip)
        transform.rotation = Quaternion.Euler(0, 0, 0);

      if (bikeDash.IsDashingAnimator)
        transform.rotation = Quaternion.Euler(0, 0, 0);

      UpdateDetectorPosition();
      CheckGround();
    }

    private void FixedUpdate()
    {
      UpdateMotorSpeed();
    }

    //===================================

    public void ForceBrake()
    {
      wheelRB.angularVelocity = 0f;
    }

    //===================================

    private void UpdateMotorSpeed()
    {
      float torque = bikeController.Throttle * power * Time.deltaTime;

      if (!bikeController.IsInCar)
        torque = 0;

      wheelRB.AddTorque(torque);

      if (torque == 0 && bikeController.Brake == 0)
        wheelRB.angularVelocity = Mathf.Lerp(wheelRB.angularVelocity, 0f, Time.deltaTime * _brakePower);

      if (bikeController.Brake != 0)
      {
        wheelRB.angularVelocity = Mathf.Lerp(wheelRB.angularVelocity, 0f, Time.deltaTime * _brakePower);
        wheelRB.freezeRotation = Mathf.Abs(wheelRB.angularVelocity) < 80f;
        return;
      }

      wheelRB.freezeRotation = false;
    }

    private void CheckGround()
    {
      RaycastHit2D raycastHit2D = Physics2D.Raycast(_groundDetector.position, -_groundDetector.up, _groundDetectionDistance, _groundLayerMask);
      Debug.DrawRay(_groundDetector.position, -_groundDetector.up * _groundDetectionDistance, Color.blue);

      RaycastHit2D hit = Physics2D.Raycast(_groundDetector.position, Vector2.down, _groundDetectionDistance, _groundLayerMask);
      Debug.DrawRay(_groundDetector.position, Vector2.down * _groundDetectionDistance, Color.red);

      wasGrounded = grounded;

      grounded = raycastHit2D.collider || hit.collider;

      if (!wasGrounded && grounded)
        OnLanded?.Invoke();

      groundHit = grounded ? (raycastHit2D.collider ? raycastHit2D : hit) : default;
    }

    private void UpdateDetectorPosition()
    {
      _groundDetector.position = transform.position;
    }

    //===================================
  }
}
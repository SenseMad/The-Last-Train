using UnityEngine;

namespace TLT.Bike.Bike
{
  public class BikeFlip : MonoBehaviour, IBikeBootstrap
  {
    [SerializeField, Min(0)] private float _minSpeed = 1.0f;
    [SerializeField, Range(0, 100), Tooltip("Percentage of stalling at minimum speed")] private float _percentStallMinSpeed = 80.0f;
    [SerializeField, Range(0, 100), Tooltip("Percentage of stalling at maximum speed")] private float _percentStallMaxSpeed = 10.0f;

    [Space]
    [SerializeField, Range(1.1f, 1.8f)] private float _brakingCoefficient = 1.1f;

    //-----------------------------------

    private BikeBody bikeBody;
    private BikeManager bikeManager;
    private BikeController bikeController;
    private BikeEngine bikeEngine;

    private float chanceStall;
    private bool isAnimFlip;

    //===================================

    public bool IsFlip { get; private set; }

    public float BrakingCoefficient { get; private set; }

    //===================================

    public void CustomAwake()
    {
      bikeBody = GetComponent<BikeBody>();
      bikeManager = GetComponent<BikeManager>();
      bikeController = GetComponent<BikeController>();
      bikeEngine = GetComponent<BikeEngine>();
    }

    public void CustomStart() { }

    private void FixedUpdate()
    {
      if (bikeBody.VelocityMagnitude <= _minSpeed && IsFlip && !isAnimFlip)
      {
        bikeController.Animator.SetTrigger(BikeAnimations.IS_FLIP_2);
        isAnimFlip = true;
      }

      if (IsFlip)
        return;

      if (bikeController.Throttle > 0 && bikeManager.Direction == 1)
      {
        Flip();
      }
      else if (bikeController.Throttle < 0 && bikeManager.Direction == -1)
      {
        Flip();
      }
    }

    //===================================

    private void Flip()
    {
      if (!bikeController.IsInCar)
        return;

      if (!bikeManager.Grounded)
        return;

      if (bikeBody.VelocityMagnitude >= _minSpeed)
        bikeController.Animator.SetTrigger(BikeAnimations.IS_FLIP_1);
      else
      {
        bikeController.Animator.SetTrigger(BikeAnimations.IS_FLIP);
        isAnimFlip = true;
      }

      IsFlip = true;

      BrakingCoefficient = GetBrakingCoefficient();

      chanceStall = CalculatePercentage();
    }

    private void FlipAnimation()
    {
      IsFlip = false;
      isAnimFlip = false;

      transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);

      bikeManager.Direction *= -1;

      bikeController.Character.Direction = bikeManager.Direction;

      if (CheckStall())
        bikeEngine.StopEngine();
    }

    private bool CheckStall()
    {
      float stallChance = chanceStall;

      float randomValue = Random.Range(0.0f, 100.0f);
      return randomValue < stallChance;
    }

    //===================================

    private float CalculatePercentage()
    {
      float maxSpeed = bikeBody.BikeData.MaxVelocity;
      float currentSpeed = Mathf.Clamp(bikeBody.VelocityMagnitude, _minSpeed, maxSpeed);

      float t = (currentSpeed - _minSpeed) / (maxSpeed - _minSpeed);
      return Mathf.Lerp(_percentStallMinSpeed, _percentStallMaxSpeed, t);
    }

    private float GetBrakingCoefficient()
    {
      return Mathf.Pow(_brakingCoefficient, bikeBody.VelocityMagnitude);
    }

    //===================================
  }
}
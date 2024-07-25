using UnityEngine;

namespace TLT.Bike.Bike
{
  public class BikeEffect : MonoBehaviour, IBikeBootstrap
  {
    [SerializeField] private ParticleSystem _moveParticle;
    [SerializeField] private ParticleSystem _fallParticle;

    [Space]
    [SerializeField, Range(0, 10)] private float _occurAfterVelocity;
    [SerializeField, Range(0, 0.2f)] private float _dustFormationPeriod;

    //-----------------------------------

    private BikeController bikeController;
    private BikeManager bikeManager;
    private BikeBody bikeBody;

    private float counter;

    //===================================

    public void CustomAwake()
    {
      bikeController = GetComponent<BikeController>();
      bikeManager = GetComponent<BikeManager>();
      bikeBody = GetComponent<BikeBody>();
    }

    public void CustomStart() { }

    private void OnEnable()
    {
      bikeManager.BackWheel.OnLanded += BackWheel_OnLanded;
    }

    private void OnDisable()
    {
      bikeManager.BackWheel.OnLanded -= BackWheel_OnLanded;
    }

    private void Update()
    {
      if (!bikeController.IsInCar)
        return;

      counter += Time.deltaTime;

      if ((bikeManager.Grounded || bikeManager.OnlyBackGrounded) && Mathf.Abs(bikeBody.BodyRB.velocity.x) > _occurAfterVelocity)
      {
        if (counter > _dustFormationPeriod)
        {
          _moveParticle.Play();
          counter = 0;
        }
      }
    }

    //===================================

    private void BackWheel_OnLanded()
    {
      _fallParticle.Play();
    }

    //===================================
  }
}
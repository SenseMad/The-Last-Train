using UnityEngine;

namespace TLT.Vehicles.Bike
{
  public class BikeEffect : MonoBehaviour
  {
    [SerializeField] private ParticleSystem _moveParticle;
    [SerializeField] private ParticleSystem _fallParticle;

    [Space]
    [SerializeField] private BikeController _bikeController;
    [SerializeField] private BikeManager _bikeManager;
    [SerializeField] private BikeBody _bikeBody;

    [Space]
    [SerializeField, Range(0, 10)] private float _occurAfterVelocity;
    [SerializeField, Range(0, 0.2f)] private float _dustFormationPeriod;

    //-----------------------------------

    private float counter;

    //===================================

    private void OnEnable()
    {
      _bikeManager.BackWheel.OnLanded += BackWheel_OnLanded;
    }

    private void OnDisable()
    {
      _bikeManager.BackWheel.OnLanded -= BackWheel_OnLanded;
    }

    private void Update()
    {
      if (!_bikeController.IsInCar)
        return;

      counter += Time.deltaTime;

      if ((_bikeManager.Grounded || _bikeManager.OnlyBackGrounded) && Mathf.Abs(_bikeBody.BodyRB.velocity.x) > _occurAfterVelocity)
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
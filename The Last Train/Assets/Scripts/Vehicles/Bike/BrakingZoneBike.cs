using UnityEngine;

namespace TLT.Vehicles.Bike
{
  public class BrakingZoneBike : MonoBehaviour
  {
    private BikeBody bikeBody;

    private bool isBrakingZone = false;

    //===================================

    /*private void Update()
    {
      if (!isBrakingZone)
        return;

      bikeBody.ForceBrake();
    }*/

    //===================================

    private void OnTriggerEnter2D(Collider2D collision)
    {
      if (!collision.TryGetComponent(out BikeBody parBikeBody))
        return;

      bikeBody = parBikeBody;

      isBrakingZone = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
      if (!collision.TryGetComponent(out BikeBody parBikeBody))
        return;

      bikeBody = null;

      isBrakingZone = false;
    }

    //===================================
  }
}
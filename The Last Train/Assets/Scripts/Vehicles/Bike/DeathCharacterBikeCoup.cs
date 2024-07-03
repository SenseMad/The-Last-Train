using UnityEngine;

namespace TLT.Vehicles.Bike
{
  public class DeathCharacterBikeCoup : MonoBehaviour
  {
    [SerializeField] private BikeBody _bikeBody;
    [SerializeField] private BikeCharacter _bikeCharacter;

    [Space]
    [SerializeField] private LayerMask _ignoreLayer;

    [SerializeField] private CapsuleCollider2D _collider2D;

    //===================================

    private void Update()
    {
      if (!_bikeBody.BikeController.IsInCar)
        return;

      if (_collider2D == null)
        return;

      Collider2D[] hits = Physics2D.OverlapCapsuleAll(_collider2D.bounds.center, _collider2D.size, _collider2D.direction, 0f, ~_ignoreLayer);

      foreach (var hit in hits)
      {
        if (hit != _collider2D)
        {
          _bikeCharacter.Character.Health.InstantlyKill();
        }
      }
    }

    //===================================
  }
}
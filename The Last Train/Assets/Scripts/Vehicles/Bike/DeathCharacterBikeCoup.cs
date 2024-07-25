using UnityEngine;

namespace TLT.Bike.Bike
{
  public class DeathCharacterBikeCoup : MonoBehaviour, IBikeBootstrap
  {
    [SerializeField] private BikeCharacter _bikeCharacter;

    [Space]
    [SerializeField] private LayerMask _ignoreLayer;

    [SerializeField] private CapsuleCollider2D _collider2D;

    //-----------------------------------

    private BikeBody bikeBody;

    //===================================

    public void CustomAwake()
    {
      bikeBody = GetComponent<BikeBody>();
    }

    public void CustomStart() { }

    private void Update()
    {
      if (!bikeBody.BikeController.IsInCar)
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
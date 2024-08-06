using UnityEngine;
using Zenject;

using TLT.Input;

namespace TLT.Bike.Bike
{
  public class GetDirectionMovementBike : MonoBehaviour
  {
    [SerializeField] private Collider2D[] _colliders;

    [Space]
    [SerializeField] private Collider2D _frontWheelCollider;

    [Space]
    [SerializeField] private LayerMask _layerMask;

    //-----------------------------------

    private InputHandler inputHandler;

    //===================================

    [Inject]
    private void Construct(InputHandler parInputHandler)
    {
      inputHandler = parInputHandler;
    }

    //===================================

    private void Update()
    {
      foreach (var collider in _colliders)
      {
        Collider2D[] colliders2D = Physics2D.OverlapBoxAll(collider.bounds.center, collider.bounds.size, 0, _layerMask);

        if (colliders2D.Length == 0 || colliders2D == null)
          return;

        foreach (var collider2D in colliders2D)
        {
          if (!collider2D.TryGetComponent(out DirectionMovement parDirectionMovement))
            continue;

          bool isFrontWheelInsideAny = false;
          foreach (var checkCollider in _colliders)
          {
            if (checkCollider != _frontWheelCollider)
              continue;

            Collider2D[] checkColliders = Physics2D.OverlapBoxAll(checkCollider.bounds.center, checkCollider.bounds.size, 0, _layerMask);
            if (checkColliders.Length == 0 || checkColliders == null)
              break;

            foreach (var collider1 in checkColliders)
            {
              isFrontWheelInsideAny = true;
              break;
            }
          }

          bool shouldEnableCollision = (parDirectionMovement.IsUp && inputHandler.GetInputVertical() > 0) || (!parDirectionMovement.IsUp && inputHandler.GetInputVertical() < 0);
          if (shouldEnableCollision)
          {
            Physics2D.IgnoreCollision(collider, parDirectionMovement.ObjectCollider2D, !isFrontWheelInsideAny);
            continue;
          }

          Physics2D.IgnoreCollision(collider, parDirectionMovement.ObjectCollider2D, true);
        }
      }
    }

    //===================================
  }
}
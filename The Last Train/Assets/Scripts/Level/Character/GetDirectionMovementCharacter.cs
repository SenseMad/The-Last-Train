using UnityEngine;
using Zenject;

namespace TLT.CharacterManager
{
  public class GetDirectionMovementCharacter : MonoBehaviour
  {
    [SerializeField] private Collider2D[] _colliders;

    [Space]
    [SerializeField] private LayerMask _layerMask;

    //-----------------------------------

    private Character character;

    //===================================

    [Inject]
    private void Construct(Character parCharacter)
    {
      character = parCharacter;
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

          bool shouldEnableCollision = (parDirectionMovement.IsUp && character.InputHandler.GetInputVertical() > 0) || (!parDirectionMovement.IsUp && character.InputHandler.GetInputVertical() < 0);
          if (shouldEnableCollision)
          {
            Physics2D.IgnoreCollision(collider, parDirectionMovement.ObjectCollider2D, false);
            continue;
          }

          Physics2D.IgnoreCollision(collider, parDirectionMovement.ObjectCollider2D, true);
        }
      }
    }

    //===================================
  }
}
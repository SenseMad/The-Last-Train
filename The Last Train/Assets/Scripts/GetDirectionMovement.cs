using UnityEngine;
using Zenject;

using TLT.CharacterManager;

public class GetDirectionMovement : MonoBehaviour
{
  [Space]
  [SerializeField] private Collider2D[] _colliders;

  [SerializeField] private LayerMask _layerMask;

  //-----------------------------------

  private Character character;

  //===================================

  [Inject]
  private void Construct(Character parCharacter, LevelManager parLevelManager)
  {
    character = parCharacter;
  }

  //===================================

  private void Update()
  {
    foreach (var _collider in _colliders)
    {
      Collider2D[] colliders = Physics2D.OverlapBoxAll(_collider.bounds.center, _collider.bounds.size, 0, _layerMask);

      if (colliders.Length == 0 || colliders == null)
        return;

      foreach (var collider in colliders)
      {
        if (!collider.TryGetComponent(out DirectionMovement parDirectionMovement))
          continue;

        if ((parDirectionMovement.IsUp && character.InputHandler.GetInputVertical() > 0) || (!parDirectionMovement.IsUp && character.InputHandler.GetInputVertical() < 0))
        {
          Physics2D.IgnoreCollision(_collider, parDirectionMovement.ObjectCollider2D, false);
          continue;
        }

        Physics2D.IgnoreCollision(_collider, parDirectionMovement.ObjectCollider2D, true);
      }
    }
  }

  //===================================
}
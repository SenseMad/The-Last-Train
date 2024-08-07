using System.Collections.Generic;
using TLT.Input;
using UnityEngine;
using Zenject;

namespace TLT.CharacterManager
{
  public class GetDirectionMovementCharacter : MonoBehaviour
  {
    [SerializeField] private Collider2D[] _detectionColliders;
    [SerializeField] private Collider2D[] _ignoreColliders;

    [Space]
    [SerializeField] private LayerMask _layerMask;

    //-----------------------------------

    private InputHandler inputHandler;

    private List<Collider2D> ignoreCollidersList = new();

    //===================================

    [Inject]
    private void Construct(InputHandler parInputHandler)
    {
      inputHandler = parInputHandler;
    }

    //===================================

    private void Update()
    {
      foreach (var collider in _detectionColliders)
      {
        Collider2D[] colliders2D = Physics2D.OverlapBoxAll(collider.bounds.center, collider.bounds.size, 0, _layerMask);

        if (colliders2D.Length == 0 || colliders2D == null)
        {
          if (ignoreCollidersList.Count == 0)
            return;

          foreach (var ignoreCollider in _ignoreColliders)
          {
            for (int i = 0; i < ignoreCollidersList.Count; i++)
            {
              if (!Physics2D.GetIgnoreCollision(ignoreCollider, ignoreCollidersList[i]))
                continue;

              Physics2D.IgnoreCollision(ignoreCollider, ignoreCollidersList[i], false);
              //ignoreCollidersList.RemoveAt(i);
            }
          }

          ignoreCollidersList.Clear();

          return;
        }

        foreach (var collider2D in colliders2D)
        {
          if (!collider2D.TryGetComponent(out DirectionMovement parDirectionMovement))
            continue;

          bool shouldEnableCollision = (parDirectionMovement.IsUp && inputHandler.GetInputVertical() > 0) || (!parDirectionMovement.IsUp && inputHandler.GetInputVertical() < 0);

          foreach (var ignoreCollider in _ignoreColliders)
          {
            if (shouldEnableCollision)
            {
              if (!parDirectionMovement.IsUp)
              {
                if (!parDirectionMovement.IsActiveDefault)
                {
                  if (!ignoreCollidersList.Contains(parDirectionMovement.ObjectCollider2D))
                    ignoreCollidersList.Add(parDirectionMovement.ObjectCollider2D);
                }
                else
                {
                  if (ignoreCollidersList.Contains(parDirectionMovement.ObjectCollider2D))
                    ignoreCollidersList.Remove(parDirectionMovement.ObjectCollider2D);
                }
              }

              Physics2D.IgnoreCollision(ignoreCollider, parDirectionMovement.ObjectCollider2D, !parDirectionMovement.IsUp);
              continue;
            }

            Physics2D.IgnoreCollision(ignoreCollider, parDirectionMovement.ObjectCollider2D, parDirectionMovement.IsActiveDefault);
          }
        }
      }
    }

    //===================================
  }
}
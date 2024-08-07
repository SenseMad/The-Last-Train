using System.Collections.Generic;
using UnityEngine;
using Zenject;

using TLT.Input;

namespace TLT.Bike.Bike
{
  public class GetDirectionMovementBike : MonoBehaviour, IBikeBootstrap
  {
    [SerializeField] private Collider2D[] _detectionColliders;
    [SerializeField] private Collider2D[] _ignoreColliders;

    [Space]
    [SerializeField] private Collider2D _frontWheelCollider;

    [Space]
    [SerializeField] private LayerMask _layerMask;

    //-----------------------------------

    private InputHandler inputHandler;

    private BikeController bikeController;

    private bool isAnimationTurnUp;
    private bool isAnimationTurnDown;

    private List<Collider2D> ignoreCollidersList = new();

    //===================================

    [Inject]
    private void Construct(InputHandler parInputHandler)
    {
      inputHandler = parInputHandler;
    }

    //===================================

    public void CustomAwake()
    {
      bikeController = GetComponent<BikeController>();
    }

    public void CustomStart() { }

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
            }
          }

          ignoreCollidersList.Clear();

          return;
        }

        foreach (var collider2D in colliders2D)
        {
          if (!collider2D.TryGetComponent(out DirectionMovement parDirectionMovement))
            continue;

          bool isFrontWheelInsideAny = false;
          foreach (var ignoreCollider in _ignoreColliders)
          {
            if (ignoreCollider != _frontWheelCollider)
              continue;

            Collider2D[] checkColliders = Physics2D.OverlapBoxAll(ignoreCollider.bounds.center, ignoreCollider.bounds.size, 0, _layerMask);
            if (checkColliders.Length == 0 || checkColliders == null)
              break;

            isFrontWheelInsideAny = true;
          }

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

                Physics2D.IgnoreCollision(ignoreCollider, parDirectionMovement.ObjectCollider2D, !parDirectionMovement.IsUp);

                if (isFrontWheelInsideAny && !isAnimationTurnDown)
                {
                  bikeController.Animator.SetTrigger(BikeAnimations.IS_TURN_DOWN);
                  isAnimationTurnDown = true;
                }
              }
              else
              {
                Physics2D.IgnoreCollision(ignoreCollider, parDirectionMovement.ObjectCollider2D, isFrontWheelInsideAny && !parDirectionMovement.IsUp);

                if (isFrontWheelInsideAny && !isAnimationTurnUp)
                {
                  bikeController.Animator.SetTrigger(BikeAnimations.IS_TURN_UP);
                  isAnimationTurnUp = true;
                }
              }

              continue;
            }

            Physics2D.IgnoreCollision(ignoreCollider, parDirectionMovement.ObjectCollider2D, parDirectionMovement.IsActiveDefault);

            isAnimationTurnUp = false;
            isAnimationTurnDown = false;
          }
        }
      }
    }

    //===================================
  }
}
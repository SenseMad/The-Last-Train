using UnityEngine;
using Zenject;

using TLT.CharacterManager;
using TLT.Enemy;

namespace TLT.Vehicles.Bike
{
  public class BikeCollisionHandler : MonoBehaviour
  {
    [SerializeField] private BikeController _bikeController;
    [SerializeField] private BikeManager _bikeManager;
    [SerializeField] private BikeBody _bikeBody;

    [Space]
    [SerializeField] private float _slowDownFactorSpeed = 0.5f;
    [SerializeField] private float _slowDownLanding = 0.5f;
    [SerializeField] private LayerMask _enemyLayer;

    [Space]
    [Header("RAY")]
    [SerializeField] private BoxCollider2D _boxCollider2D;
    [SerializeField] private BoxCollider2D _lowerBoxCollider;
    [SerializeField, Min(0)] private float _rayDistanceForward = 1.3f;

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
      RayForward();

      LowerCollider();
    }

    //===================================

    private void RayForward()
    {
      if (!_bikeController.IsInCar)
        return;

      if (_bikeBody.BodyRB.velocity.x * _bikeManager.Direction < _bikeBody.BikeData.MaxVelocity / 3)
        return;

      Bounds bounds = _boxCollider2D.bounds;

      Vector2 direction;
      Vector2[] rayOrigins;

      if (_bikeManager.Direction == 1)
      {
        direction = Vector2.right;
        rayOrigins = new Vector2[]
        {
          // Верх правого края
          //new Vector2(bounds.max.x, bounds.max.y),
          // Центр правого края
          new Vector2(bounds.max.x, bounds.center.y),
          // Низ правого края
          //new Vector2(bounds.max.x, bounds.min.y),
        };
      }
      else
      {
        direction = Vector2.left;
        rayOrigins = new Vector2[]
        {
          // Верх левого края
          //new Vector2(bounds.min.x, bounds.max.y),
          // Центр
          new Vector2(bounds.min.x, bounds.center.y),
          // Низ левого края
          //new Vector2(bounds.min.x, bounds.min.y),
        };
      }

      for (int i = 0; i < rayOrigins.Length; i++)
      {
        RaycastHit2D hit = Physics2D.Raycast(rayOrigins[i], direction, _rayDistanceForward, _enemyLayer);

        Debug.DrawRay(rayOrigins[i], direction * _rayDistanceForward, Color.red);

        if (hit.collider == null)
          continue;

        if (!hit.collider.TryGetComponent(out EnemyAgent parEnemyAgent))
          continue;

        if (_bikeManager.Grounded || _bikeManager.OnlyFrontGrounded)
        {
          _bikeBody.BodyRB.velocity = new Vector2(_bikeBody.BodyRB.velocity.x * _slowDownFactorSpeed, _bikeBody.BodyRB.velocity.y);
          parEnemyAgent.TypeDeath("IsDeathSpeed");
          parEnemyAgent.ApplyDamage(1);
          character.ApplyDamage(1);
          _bikeController.Animator.SetTrigger("IsHurt");
          return;
        }
      }
    }

    private void LowerCollider()
    {
      if (!_bikeController.IsInCar)
        return;

      if (_bikeBody.BodyRB.velocity.x * _bikeManager.Direction < _bikeBody.BikeData.MaxVelocity / _bikeBody.BikeData.MaxVelocity)
        return;

      Collider2D[] colliders = Physics2D.OverlapBoxAll(_lowerBoxCollider.bounds.center, _lowerBoxCollider.bounds.size, 0, _enemyLayer);

      if (colliders.Length == 0 || colliders == null)
        return;

      foreach (var collider in colliders)
      {
        if (collider.TryGetComponent(out EnemyAgent parEnemyAgent))
        {
          if (_bikeManager.OnlyBackGrounded)
          {
            /*if (_bikeBody.WheelLiftAngle < -0.011f)
            {
              _bikeBody.BodyRB.velocity = new Vector2(_bikeBody.BodyRB.velocity.x * _slowDownFactorSpeed, _bikeBody.BodyRB.velocity.y);
              parEnemyAgent.TypeDeath("IsDeathSpeed");
              parEnemyAgent.ApplyDamage(1);
              character.ApplyDamage(1);
              _bikeController.Animator.SetTrigger("IsHurt");
              return;
            }*/

            _bikeBody.BodyRB.velocity = new Vector2(_bikeBody.BodyRB.velocity.x * _slowDownLanding, _bikeBody.BodyRB.velocity.y);
            parEnemyAgent.TypeDeath("IsDeathSpeed");
            parEnemyAgent.ApplyDamage(1);
            return;
          }

          if (!_bikeManager.Grounded)
          {
            _bikeBody.BodyRB.velocity = new Vector2(_bikeBody.BodyRB.velocity.x * _slowDownLanding, _bikeBody.BodyRB.velocity.y);
            parEnemyAgent.TypeDeath("IsDeathLanding");
            parEnemyAgent.ApplyDamage(1);
            return;
          }
        }
      }
    }

    //===================================
  }
}
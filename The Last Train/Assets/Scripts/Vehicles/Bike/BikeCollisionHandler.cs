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

    private void Start()
    {
      //Vector2 boxCenter = _boxCollider2D.bounds.center;
      //Vector2 boxExtents = _boxCollider2D.bounds.extents;
    }

    private void Update()
    {
      RayForward();

      LowerCollider();
      /*Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1.2f, _enemyLayer);

      foreach (var hitCollider in hitColliders)
      {
        if (hitCollider.TryGetComponent(out EnemyAgent parEnemyAgent))
        {
          if (_bikeManager.Grounded || _bikeManager.OnlyFrontGrounded)
          {
            parEnemyAgent.Health.TakeHealth(1);
            _character.Health.TakeHealth(1);
            _bikeController.Animator.SetTrigger("IsHurt");
            return;
          }

          if (_bikeManager.OnlyBackGrounded)
          {
            parEnemyAgent.Health.TakeHealth(1);
            return;
          }

          if (!_bikeManager.OnlyBackGrounded && !_bikeManager.OnlyFrontGrounded && !_bikeManager.Grounded)
          {
            parEnemyAgent.Health.InstantlyKill();
            return;
          }
        }
      }*/
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
      if (!collision.TryGetComponent(out EnemyAgent parEnemyAgent))
        return;

      if (_bikeManager.Grounded || _bikeManager.OnlyFrontGrounded)
      {
        parEnemyAgent.Health.TakeHealth(1);
        _character.Health.TakeHealth(1);
        _bikeController.Animator.SetTrigger("IsHurt");
        return;
      }

      if (_bikeManager.OnlyBackGrounded)
      {
        parEnemyAgent.Health.TakeHealth(1);
        return;
      }

      if (!_bikeManager.OnlyBackGrounded && !_bikeManager.OnlyFrontGrounded && !_bikeManager.Grounded)
      {
        parEnemyAgent.Health.InstantlyKill();
        return;
      }
    }*/

    //===================================

    private void OnCollisionEnter2D(Collision2D collision)
    {
      /*if (!collision.collider.TryGetComponent(out EnemyAgent parEnemyAgent))
        return;

      if (!_bikeController.IsInCar)
        return;

      if (_bikeBody.BodyRB.velocity.x > 0)
      {
        if (_bikeManager.Grounded || _bikeManager.OnlyFrontGrounded)
        {
          parEnemyAgent.TypeDeath("IsDeathSpeed");
          parEnemyAgent.ApplyDamage(1);
          character.ApplyDamage(1);
          _bikeController.Animator.SetTrigger("IsHurt");
          return;
        }

        if (_bikeManager.OnlyBackGrounded)
        {
          parEnemyAgent.TypeDeath("IsDeath");
          parEnemyAgent.ApplyDamage(1);
          return;
        }
      }

      if (!_bikeManager.Grounded)
      {
        parEnemyAgent.TypeDeath("IsDeathLanding");
        parEnemyAgent.Health.InstantlyKill();
        return;
      }*/
    }

    //===================================

    private void RayForward()
    {
      if (!_bikeController.IsInCar)
        return;

      if (_bikeBody.BodyRB.velocity.x < _bikeBody.BikeData.MaxVelocity / 3)
        return;

      Bounds bounds = _boxCollider2D.bounds;

      Vector2[] rayOrigins = new Vector2[]
      {
        // Верхний центр
        //new Vector2(bounds.max.x, bounds.max.y),
        // Центр
        new Vector2(bounds.max.x, bounds.center.y),
        // Правый низ
        //new Vector2(bounds.max.x, bounds.min.y),
      };

      Vector2 direction = new(transform.right.x * _bikeManager.Direction, transform.right.y);

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

      if (_bikeBody.BodyRB.velocity.x < _bikeBody.BikeData.MaxVelocity / _bikeBody.BikeData.MaxVelocity)
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
using UnityEngine;
using Zenject;

using TLT.CharacterManager;
using TLT.Enemy;

namespace TLT.Bike.Bike
{
  public class BikeCollisionHandler : MonoBehaviour, IBikeBootstrap
  {
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

    private BikeController bikeController;
    private BikeManager bikeManager;
    private BikeBody bikeBody;

    private BikeDash bikeDash;

    private Character character;

    //===================================

    [Inject]
    private void Construct(Character parCharacter)
    {
      character = parCharacter;
    }

    //===================================

    public void CustomAwake()
    {
      bikeController = GetComponent<BikeController>();
      bikeManager = GetComponent<BikeManager>();
      bikeBody = GetComponent<BikeBody>();

      bikeDash = GetComponent<BikeDash>();
    }

    public void CustomStart() { }

    private void Update()
    {
      if (bikeDash.IsDashing)
        return;

      RayForward();

      LowerCollider();
    }

    //===================================

    private void RayForward()
    {
      if (!bikeController.IsInCar)
        return;

      if (bikeBody.BodyRB.velocity.x * bikeManager.Direction < bikeBody.BikeData.MaxVelocity / 3)
        return;

      Bounds bounds = _boxCollider2D.bounds;

      Vector2 direction;
      Vector2[] rayOrigins;

      if (bikeManager.Direction == 1)
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

        if (bikeManager.Grounded || bikeManager.OnlyFrontGrounded)
        {
          bikeBody.BodyRB.velocity = new Vector2(bikeBody.BodyRB.velocity.x * _slowDownFactorSpeed, bikeBody.BodyRB.velocity.y);
          parEnemyAgent.TypeDeath(EnemyAnimations.IS_DEATH_SPEED);
          parEnemyAgent.ApplyDamage(1);
          character.ApplyDamage(1);
          bikeController.Animator.SetTrigger(BikeAnimations.IS_HURT);

          BalanceMiniGame balanceMiniGame = parEnemyAgent.GetComponent<BalanceMiniGame>();
          bikeController.BalanceMiniGameManager.Initialize(balanceMiniGame.PowerSkidding);

          return;
        }
      }
    }

    private void LowerCollider()
    {
      if (!bikeController.IsInCar)
        return;

      if (bikeBody.BodyRB.velocity.x * bikeManager.Direction < bikeBody.BikeData.MaxVelocity / bikeBody.BikeData.MaxVelocity)
        return;

      Collider2D[] colliders = Physics2D.OverlapBoxAll(_lowerBoxCollider.bounds.center, _lowerBoxCollider.bounds.size, 0, _enemyLayer);

      if (colliders.Length == 0 || colliders == null)
        return;

      foreach (var collider in colliders)
      {
        if (collider.TryGetComponent(out EnemyAgent parEnemyAgent))
        {
          if (bikeManager.OnlyBackGrounded)
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

            bikeBody.BodyRB.velocity = new Vector2(bikeBody.BodyRB.velocity.x * _slowDownLanding, bikeBody.BodyRB.velocity.y);
            parEnemyAgent.TypeDeath(EnemyAnimations.IS_DEATH_SPEED);
            parEnemyAgent.ApplyDamage(1);
            return;
          }

          if (!bikeManager.Grounded)
          {
            bikeBody.BodyRB.velocity = new Vector2(bikeBody.BodyRB.velocity.x * _slowDownLanding, bikeBody.BodyRB.velocity.y);
            parEnemyAgent.TypeDeath(EnemyAnimations.IS_DEATH_LANDING);
            parEnemyAgent.ApplyDamage(1);
            return;
          }
        }
      }
    }

    //===================================
  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TLT.CharacterManager;
using TLT.HealthManager;
using TLT.Interfaces;
using TLT.Vehicles;
using TLT.Vehicles.Bike;

namespace TLT.Enemy
{
  public abstract class EnemyAgent : MonoBehaviour, IDamageable
  {
    [SerializeField] private float _speed = 5.0f;

    [SerializeField] private float _minFlipDistance = 0.2f;

    [SerializeField] private LayerMask _targetLayerMask;

    [SerializeField] private BoxCollider2D bodyBoxCollider2D;

    [Space(10)]
    [SerializeField] protected EnemyAttackData _enemyAttackData;
    
    //-----------------------------------

    private new Rigidbody2D rigidbody2D;

    private Animator animator;

    private EnemyAIState enemyAIState;

    private float lastAttackTime = 0f;

    private bool targetAttactRadius = false;

    private bool takeDamage = false;
    private float timeAfterTakingDamage = 0;

    private int direction = 1;

    private string typeDeath = "";

    //===================================

    //public Character Target { get; private set; }
    public BoxCollider2D BodyBoxCollider2D { get => bodyBoxCollider2D; set => bodyBoxCollider2D = value; }

    public GameObject Targetable { get; private set; }

    public Health Health { get; private set; }

    //===================================

    private void Awake()
    {
      rigidbody2D = GetComponent<Rigidbody2D>();

      animator = GetComponent<Animator>();

      Health = GetComponent<Health>();
    }

    private void OnEnable()
    {
      Health.OnTakeHealth += Health_OnTakeHealth;

      Health.OnInstantlyKill += Health_OnInstantlyKill;
    }

    private void OnDisable()
    {
      Health.OnTakeHealth -= Health_OnTakeHealth;

      Health.OnInstantlyKill -= Health_OnInstantlyKill;
    }

    protected virtual void Update()
    {
      if (enemyAIState == EnemyAIState.Death)
        return;

      targetAttactRadius = false;

      IgnoreCollision();

      TargetSearch();

      DelayBeforeAttack();

      ChangeAnimationState();
    }

    protected virtual void FixedUpdate()
    {
      if (enemyAIState == EnemyAIState.Death)
        return;

      FollowTarget();
    }

    //===================================

    private void Health_OnTakeHealth(int obj)
    {
      /*takeDamage = true;

      animator.SetBool("IsTakeDamage", true);*/
    }

    private void Health_OnInstantlyKill()
    {
      animator.SetBool(typeDeath, true);

      enemyAIState = EnemyAIState.Death;
      rigidbody2D.velocity = Vector2.zero;

      rigidbody2D.gravityScale = 0;
      rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

      bodyBoxCollider2D.enabled = false;

      Destroy(gameObject, 1.25f);
    }

    public void OnDeath()
    {
      Destroy(gameObject);
    }

    public void TypeDeath(string parValue)
    {
      typeDeath = parValue;
    }

    //===================================

    protected virtual void FollowTarget()
    {
      if (Targetable == null)
      {
        rigidbody2D.velocity = Vector2.zero;
        animator.SetBool("IsWalk", false);
        return;
      }

      Vector2 direction = (Targetable.transform.position - transform.position).normalized;

      Vector2 targetVelocity = new(direction.x * _speed, rigidbody2D.velocity.y);

      rigidbody2D.velocity = targetVelocity;

      if (!targetAttactRadius)
        animator.SetBool("IsWalk", true);
    }

    protected virtual void Attack()
    {
      if (enemyAIState == EnemyAIState.Death)
        return;

      if (!targetAttactRadius)
        return;

      if (Targetable == null)
        return;

      if (Targetable.TryGetComponent(out Character parCharacter))
      {
        if (Targetable.TryGetComponent(out BikeController parBikeController))
        {
          parBikeController.Character.ApplyDamage(_enemyAttackData.Damage);
          return;
        }

        parCharacter.ApplyDamage(_enemyAttackData.Damage);
      }
    }

    //===================================

    public void ApplyDamage(int parDamage)
    {
      Health.TakeHealth(parDamage);

      if (typeDeath == "IsDeathLanding" || typeDeath == "IsDeathSpeed")
        return;

      takeDamage = true;

      if (Health.CurrentHealth <= 0)
        animator.SetBool("IsDeath", true);
      else
        animator.SetBool("IsTakeDamage", true);
    }

    //===================================

    private void ChangeAnimationState()
    {
      if (!targetAttactRadius || Targetable == null)
      {
        animator.SetBool("IsAttack", false);
      }

      if (takeDamage)
      {
        timeAfterTakingDamage += Time.deltaTime;
        if (timeAfterTakingDamage >= 0.2f)
        {
          takeDamage = false;
          animator.SetBool("IsTakeDamage", false);
          timeAfterTakingDamage = 0;
        }

        return;
      }
    }

    private bool DelayBeforeAttack()
    {
      if (!targetAttactRadius)
      {
        lastAttackTime = 0;
        return false;
      }

      if (Targetable == null)
      {
        lastAttackTime = 0;
        return false;
      }

      animator.SetBool("IsAttack", true);

      lastAttackTime += Time.deltaTime;
      if (lastAttackTime >= _enemyAttackData.AttackDelay)
      {
        lastAttackTime = 0;
        animator.SetBool("IsAttack", false);

        return true;
      }

      return false;
    }

    //===================================

    private void IgnoreCollision()
    {
      /*if (bodyBoxCollider2D == null)
        return;

      Collider2D[] colliders = Physics2D.OverlapBoxAll(bodyBoxCollider2D.bounds.center, new Vector2(_enemyAttackData.RangeVisibility.x, _enemyAttackData.RangeVisibility.y), 0, _targetLayerMask);

      if (colliders.Length == 0 || colliders == null)
        return;

      foreach (var collider in colliders)
      {
        if (collider.TryGetComponent(out BikeBody parBikeBody))
        {
          if (parBikeBody.BodyRB.velocity.x > 0)
            Physics2D.IgnoreLayerCollision(9, 10, false);
          else
            Physics2D.IgnoreLayerCollision(9, 10, true);
        }
      }*/
    }

    private bool TargetSearch()
    {
      if (bodyBoxCollider2D == null)
        return false;

      Collider2D[] colliders = Physics2D.OverlapBoxAll(bodyBoxCollider2D.bounds.center, new Vector2(_enemyAttackData.RangeVisibility.x, _enemyAttackData.RangeVisibility.y), 0, _targetLayerMask);

      if (colliders.Length == 0 || colliders == null)
      {
        Targetable = null;
        return false;
      }

      foreach (var collider in colliders)
      {
        if (collider.TryGetComponent(out Character parCharacter))
        {
          Targetable = parCharacter.gameObject;

          Flip();

          TargetAttackRadius();

          return true;
        }
      }

      foreach (var collider in colliders)
      {
        if (collider.TryGetComponent(out BikeController parBikeController))
        {
          if (parBikeController.Character != null)
          {
            Targetable = parBikeController.gameObject;

            Flip();

            TargetAttackRadius();

            return true;
          }
        }
      }

      Targetable = null;
      return false;
    }

    private bool TargetAttackRadius()
    {
      if (Targetable == null)
        return false;

      Collider2D[] colliders = Physics2D.OverlapCircleAll(bodyBoxCollider2D.bounds.center, _enemyAttackData.AttackRadius, _targetLayerMask);

      if (colliders.Length == 0 || colliders == null)
        return false;

      targetAttactRadius = false;

      foreach (var collider in colliders)
      {
        if (Targetable.TryGetComponent(out Character parCharacter))
        {
          if (parCharacter == collider.GetComponent<Character>())
          {
            targetAttactRadius = true;
            return true;
          }
        }

        if (Targetable.TryGetComponent(out BikeController parBikeController))
        {
          if (parBikeController.Character == null)
            return false;

          if (parBikeController == collider.GetComponent<BikeController>())
          {
            targetAttactRadius = true;
            return true;
          }
        }
      }

      return false;
    }

    protected virtual void Flip()
    {
      if (Targetable == null)
        return;

      Vector3 directionToTarget = Targetable.transform.position - transform.position;

      if (directionToTarget.magnitude <= _minFlipDistance)
        return;

      if (directionToTarget.x > 0)
        direction = -1;
      else
        direction = 1;

      transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
    }

    //===================================

    private void OnDrawGizmos()
    {
      if (bodyBoxCollider2D == null)
        return;

      Gizmos.color = Color.yellow;
      Gizmos.DrawWireCube(bodyBoxCollider2D.bounds.center, new Vector2(_enemyAttackData.RangeVisibility.x, _enemyAttackData.RangeVisibility.y));

      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(bodyBoxCollider2D.bounds.center, _enemyAttackData.AttackRadius);
    }

    //===================================
  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TLT.CharacterManager;
using TLT.HealthManager;
using TLT.Interfaces;
using TLT.Vehicles;

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

    //===================================

    public Character Target { get; private set; }

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
      takeDamage = true;

      animator.SetBool("IsTakeDamage", true);
    }

    private void Health_OnInstantlyKill()
    {
      animator.SetBool("IsDeath", true);

      enemyAIState = EnemyAIState.Death;
      rigidbody2D.velocity = Vector2.zero;
    }

    public void OnDeath()
    {
      Destroy(gameObject);
    }

    //===================================

    protected virtual void FollowTarget()
    {
      if (Target == null)
      {
        rigidbody2D.velocity = Vector2.zero;
        animator.SetBool("IsWalk", false);
        return;
      }

      Vector2 direction = (Target.transform.position - transform.position).normalized;

      Vector2 targetVelocity = new(direction.x * _speed, rigidbody2D.velocity.y);

      rigidbody2D.velocity = targetVelocity;

      if (!targetAttactRadius)
        animator.SetBool("IsWalk", true);
    }

    protected virtual void Attack()
    {
      if (!targetAttactRadius)
        return;

      if (Target == null)
        return;

      Target.ApplyDamage(_enemyAttackData.Damage);

      Debug.Log($"Урон нанесен");
    }

    //===================================

    public void ApplyDamage(int parDamage)
    {
      Health.TakeHealth(parDamage);
    }

    //===================================

    private void ChangeAnimationState()
    {
      if (!targetAttactRadius || Target == null)
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

      if (Target == null)
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

    private bool TargetSearch()
    {
      if (bodyBoxCollider2D == null)
        return false;

      Collider2D collider2D = Physics2D.OverlapBox(bodyBoxCollider2D.bounds.center, new Vector2(_enemyAttackData.RangeVisibility.x, _enemyAttackData.RangeVisibility.y), 0, _targetLayerMask);

      if (collider2D == null)
      {
        Target = null;
        return false;
      }

      if (!collider2D.TryGetComponent(out Character parCharacter))
      {
        if (!collider2D.TryGetComponent(out VehicleController parVehicleController))
          return false;

        if (parVehicleController.CurrentCharacter == null)
          return false;

        Target = parVehicleController.CurrentCharacter;

        Flip();

        TargetAttackRadius();

        return true;
      }

      Target = parCharacter;

      Flip();

      TargetAttackRadius();

      return true;
    }

    private bool TargetAttackRadius()
    {
      if (Target == null)
        return false;

      Collider2D targetAttackRadius = Physics2D.OverlapCircle(bodyBoxCollider2D.bounds.center, _enemyAttackData.AttackRadius, _targetLayerMask);

      if (targetAttackRadius == null)
        return false;

      if (!targetAttackRadius.TryGetComponent(out Character parCharacter))
      {
        if (!targetAttackRadius.TryGetComponent(out VehicleController parVehicleController))
          return false;

        if (parVehicleController.CurrentCharacter == null)
          return false;

        targetAttactRadius = true;

        return true;
      }

      targetAttactRadius = true;

      return true;
    }

    protected virtual void Flip()
    {
      if (Target == null)
        return;

      Vector3 directionToTarget = Target.transform.position - transform.position;

      if (directionToTarget.magnitude <= _minFlipDistance)
        return;

      if (directionToTarget.x > 0)
        transform.rotation = Quaternion.Euler(0, 180, 0);
      else
        transform.rotation = Quaternion.Euler(0, 0, 0);
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
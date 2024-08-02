using UnityEngine;
using Newtonsoft.Json.Linq;
using System;

using TLT.CharacterManager;
using TLT.HealthManager;
using TLT.Interfaces;
using TLT.Bike.Bike;
using TLT.Save;
using TLT.Data;

namespace TLT.Enemy
{
  public abstract class EnemyAgent : MonoBehaviour, IDamageable, ISaveLoad
  {
    [SerializeField] private float _speed = 5.0f;

    [SerializeField] private float _minFlipDistance = 0.2f;

    [SerializeField] private LayerMask _targetLayerMask;

    [SerializeField] private Collider2D _collider2D;

    [Space(10)]
    [SerializeField] protected EnemyAttackData _enemyAttackData;

    [Space]
    [SerializeField] private Collider2D _deathCollider;
    [SerializeField] private Collider2D _deathBikeSpeedCollider;
    [SerializeField] private Collider2D _deathBikeLandingCollider;

    //-----------------------------------

    private Animator animator;

    private EnemyPatrol enemyPatrol;

    private EnemyAIState enemyAIState;

    private float lastAttackTime = 0f;

    private bool targetAttactRadius = false;

    private bool takeDamage = false;
    private float timeAfterTakingDamage = 0;

    private bool canAttack;

    private string typeDeath = "";

    private EnemyParticleDeath enemyParticleDeath;

    //===================================

    public Rigidbody2D Rigidbody2D { get; set; }

    public GameObject Targetable { get; private set; }

    public Health Health { get; set; }

    public int Direction { get; set; } = 1;

    public float Speed => _speed;

    //===================================

    private void Awake()
    {
      Rigidbody2D = GetComponent<Rigidbody2D>();

      enemyPatrol = GetComponent<EnemyPatrol>();

      animator = GetComponent<Animator>();

      Health = GetComponent<Health>();

      enemyParticleDeath = GetComponent<EnemyParticleDeath>();
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
      Rigidbody2D.velocity = Vector2.zero;

      Rigidbody2D.gravityScale = 0;
      Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

      _collider2D.enabled = false;

      //Destroy(gameObject, 30f);
    }

    public void OnDeath()
    {
      //Destroy(gameObject, 30f);
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
        Rigidbody2D.velocity = Vector2.zero;

        if (enemyPatrol != null && enemyPatrol.Patrol())
        {
          animator.SetBool("IsWalk", true);
        }
        else
        {
          animator.SetBool("IsWalk", false);
        }

        return;
      }

      Vector2 direction = (Targetable.transform.position - transform.position).normalized;

      Vector2 targetVelocity = new(direction.x * _speed, Rigidbody2D.velocity.y);

      Rigidbody2D.velocity = targetVelocity;

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

      canAttack = true;

      if (Targetable.TryGetComponent(out Character parCharacter))
      {
        parCharacter.ApplyDamage(_enemyAttackData.Damage);
        return;
      }

      if (Targetable.TryGetComponent(out BikeController parBikeController))
      {
        parBikeController.Character.ApplyDamage(_enemyAttackData.Damage);
        return;
      }
    }

    //===================================

    public void ApplyDamage(int parDamage)
    {
      Health.TakeHealth(parDamage);

      if (typeDeath == "IsDeathLanding")
      {
        _deathBikeLandingCollider.gameObject.SetActive(true);
        return;
      }

      if (typeDeath == "IsDeathSpeed")
      {
        _deathBikeSpeedCollider.gameObject.SetActive(true);
        return;
      }

      takeDamage = true;

      if (Health.CurrentHealth <= 0)
      {
        animator.SetBool("IsDeath", true);
        _deathCollider.gameObject.SetActive(true);
      }
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
      if (lastAttackTime >= _enemyAttackData.AttackDelay && canAttack)
      {
        lastAttackTime = 0;
        animator.SetBool("IsAttack", false);
        canAttack = false;

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
      if (_collider2D == null)
        return false;

      Collider2D[] colliders = Physics2D.OverlapBoxAll(_collider2D.bounds.center, new Vector2(_enemyAttackData.RangeVisibility.x, _enemyAttackData.RangeVisibility.y), 0, _targetLayerMask);

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

      Collider2D[] colliders = Physics2D.OverlapCircleAll(_collider2D.bounds.center, _enemyAttackData.AttackRadius, _targetLayerMask);

      if (colliders.Length == 0 || colliders == null)
        return false;

      targetAttactRadius = false;

      foreach (var collider in colliders)
      {
        if (collider.GetComponent<Character>())
        {
          if (Targetable.TryGetComponent(out Character parCharacter))
          {
            targetAttactRadius = true;
            return true;
          }
        }

        if (collider.GetComponent<BikeController>())
        {
          if (Targetable.TryGetComponent(out BikeController parBikeController))
          {
            if (parBikeController.Character != null)
            {
              targetAttactRadius = true;
              return true;
            }
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
        Direction = -1;
      else
        Direction = 1;

      transform.localScale = new Vector3(Direction, transform.localScale.y, transform.localScale.z);
    }

    //===================================

    private void OnDrawGizmos()
    {
      if (_collider2D == null)
        return;

      Gizmos.color = Color.yellow;
      Gizmos.DrawWireCube(_collider2D.bounds.center, new Vector2(_enemyAttackData.RangeVisibility.x, _enemyAttackData.RangeVisibility.y));

      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(_collider2D.bounds.center, _enemyAttackData.AttackRadius);
    }

    //===================================

    public ObjectData SaveData()
    {
      string objectName = transform.name;
      Vector3 rotation = transform.rotation.eulerAngles;
      Vector3 position = transform.position;
      Vector3 scale = transform.localScale;

      ObjectData data = new(objectName);
      data.Parameters["Position"] = new float[] { position.x, position.y, position.z };
      data.Parameters["Rotation"] = new float[] { rotation.x, rotation.y, rotation.z };
      data.Parameters["Scale"] = new float[] { scale.x, scale.y, scale.z };

      data.Parameters["EnemyAIState"] = enemyAIState;

      return data;
    }

    public void LoadData(ObjectData parObjectData)
    {
      if (parObjectData.ObjectName == transform.name)
      {
        LoadTransform(parObjectData);

        if (parObjectData.Parameters.TryGetValue("EnemyAIState", out var parEnemyAIState) && parEnemyAIState is long || parEnemyAIState is int)
        {
          enemyAIState = (EnemyAIState)Convert.ToInt32(parEnemyAIState);

          if (enemyAIState == EnemyAIState.Death)
            gameObject.SetActive(false);
        }
      }
    }

    //===================================

    private void LoadTransform(ObjectData parObjectData)
    {
      if (parObjectData.Parameters.TryGetValue("Position", out var position) && position is JArray positionArray)
      {
        transform.position = new Vector3(positionArray[0].ToObject<float>(), positionArray[1].ToObject<float>(), positionArray[2].ToObject<float>());
      }

      if (parObjectData.Parameters.TryGetValue("Rotation", out var rotation) && rotation is JArray rotationArray)
      {
        transform.rotation = Quaternion.Euler(rotationArray[0].ToObject<float>(), rotationArray[1].ToObject<float>(), rotationArray[2].ToObject<float>());
      }

      if (parObjectData.Parameters.TryGetValue("Scale", out var scale) && scale is JArray scaleArray)
      {
        transform.localScale = new Vector3(scaleArray[0].ToObject<float>(), scaleArray[1].ToObject<float>(), scaleArray[2].ToObject<float>());
      }
    }

    //===================================
  }
}
using UnityEngine;

namespace TLT.Enemy
{
  [System.Serializable]
  public sealed class EnemyAttackData
  {
    [SerializeField, Min(0)] private int _damage = 0;

    [Space(10)]
    [SerializeField, Min(0)] private Vector2 _rangeVisibility = new(5.0f, 0.5f);

    [SerializeField, Min(0)] private float _attackRadius = 0.35f;
    [SerializeField, Min(0)] private float _attackDelay = 1.0f;

    //===================================

    public int Damage { get => _damage; private set => _damage = value; }

    public Vector2 RangeVisibility { get => _rangeVisibility; private set => _rangeVisibility = value; }

    public float AttackRadius { get => _attackRadius; private set => _attackRadius = value; }

    public float AttackDelay { get => _attackDelay; private set => _attackDelay = value; }

    //===================================
  }
}
using UnityEngine;

namespace TLT.Enemy
{
  public class EnemyPatrol : MonoBehaviour
  {
    [SerializeField] private Transform[] _patrolPoints;

    //-----------------------------------

    private int currentPointIndex = 0;

    private EnemyAgent enemyAgent;

    //===================================

    private void Awake()
    {
      enemyAgent = GetComponent<EnemyAgent>();
    }

    //===================================

    public bool Patrol()
    {
      if (_patrolPoints == null || _patrolPoints.Length == 0)
        return false;

      Vector2 target = _patrolPoints[currentPointIndex].position;
      Vector2 moveDirection = (target - (Vector2)transform.position).normalized;

      enemyAgent.Rigidbody2D.velocity = moveDirection * enemyAgent.Speed;

      if (Vector2.Distance(transform.position, target) < 0.3f)
      {
        currentPointIndex = (currentPointIndex + 1) % _patrolPoints.Length;

        Flip();
      }

      return true;
    }

    private void Flip()
    {
      if (_patrolPoints == null || _patrolPoints.Length == 0)
        return;

      Vector3 directionToTarget = _patrolPoints[currentPointIndex].position - transform.position;

      if (directionToTarget.magnitude <= 0.3f)
        return;

      if (directionToTarget.x > 0)
        enemyAgent.Direction = -1;
      else
        enemyAgent.Direction = 1;

      transform.localScale = new Vector3(enemyAgent.Direction, transform.localScale.y, transform.localScale.z);
    }

    //===================================
  }
}
using UnityEngine;

namespace TLT.Enemy.StateMachine
{
  public class EnemyStateMachine : MonoBehaviour
  {
    private EnemyBaseState currentState;

    //===================================
    
    public EnemyIdleState IdleState { get; private set; } = new();
    public EnemyFollowState FollowState { get; private set; } = new();

    public EnemyAgent Agent { get; private set; }

    //===================================

    private void Awake()
    {
      Agent = GetComponent<EnemyAgent>();
    }

    private void Start()
    {
      currentState.EnterState(this);
    }

    private void Update()
    {
      currentState.UpdateState(this);
    }

    //===================================

    public void SwithState(EnemyBaseState parState)
    {
      currentState = parState;
      parState.EnterState(this);
    }

    //===================================
  }
}
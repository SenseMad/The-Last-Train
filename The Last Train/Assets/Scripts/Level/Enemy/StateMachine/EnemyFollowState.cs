using UnityEngine;

using TLT.CharacterManager;

namespace TLT.Enemy.StateMachine
{
  public class EnemyFollowState : EnemyBaseState
  {
    private Character target;

    //===================================

    public override void EnterState(EnemyStateMachine parState)
    {
      //target = parState.Agent.Target;
    }

    public override void UpdateState(EnemyStateMachine parState)
    {

    }

    //===================================
  }
}
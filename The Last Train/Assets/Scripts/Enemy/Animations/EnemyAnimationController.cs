using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLT.Enemy.Animation
{
  public class EnemyAnimationController : MonoBehaviour
  {
    private Animator animator;

    private string currentState;

    //===================================

    private void Awake()
    {
      animator = GetComponent<Animator>();
    }

    //===================================

    /*public void ChangeAnimationState(string parNewState)
    {
      if (currentState == parNewState)
        return;

      animator.Play(parNewState);

      currentState = parNewState;
    }*/

    //===================================
  }
}
using UnityEngine;

public class BalanceMiniGame : MonoBehaviour
{
  [SerializeField, Range(0, 1)] private float _powerSkidding;

  //===================================

  public float PowerSkidding { get => _powerSkidding; private set => _powerSkidding = value; }

  //===================================
}
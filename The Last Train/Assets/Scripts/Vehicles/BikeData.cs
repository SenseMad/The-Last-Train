using UnityEngine;

namespace TLT.Vehicles
{
  [System.Serializable]
  public sealed class BikeData
  {
    [Header("Speed")]
    [SerializeField] private float _bodyForce = 100f;
    [SerializeField, Min(0)] private float _maxVelocity = 14f;
    [SerializeField] private float _defaultForceBrakeSpeed = 50f;

    [Space, Header("Balance")]
    [SerializeField] private float _wheelieTorque = 50f;
    [SerializeField] private float _spinTorque = 30f;
    [SerializeField] private float _balanceStrength = 1f;
    [SerializeField] private float _maxAngularVelocity = 5f;
    [SerializeField] private float _tempBalanceSignTime = 0.3f;

    //===================================

    public float BodyForce => _bodyForce;
    public float MaxVelocity => _maxVelocity;
    public float DefaultForceBrakeSpeed => _defaultForceBrakeSpeed;

    public float WheelieTorque => _wheelieTorque;
    public float SpinTorque => _spinTorque;
    public float BalanceStrength => _balanceStrength;
    public float MaxAngularVelocity => _maxAngularVelocity;
    public float TempBalanceSignTime => _tempBalanceSignTime;

    //===================================
  }
}
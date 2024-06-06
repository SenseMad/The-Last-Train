using UnityEngine;

namespace TLT.Vehicles
{
  [System.Serializable]
  public sealed class VehicleData
  {
    [SerializeField, Min(0)] private float _speed = 0;
    [SerializeField, Min(0)] private float _reverseSpeed = 0;

    [Space(10)]
    [SerializeField, Min(0)] private float _accelerationTimeMaxSpeed;
    [SerializeField, Min(0)] private float _deccelerationTime;

    /*[Space(10)]
    [SerializeField, Min(0)] private float _maxAngle = 30f;*/

    //===================================

    public float Speed => _speed;
    public float ReverseSpeed => _reverseSpeed;

    public float AccelerationTimeMaxSpeed => _accelerationTimeMaxSpeed;
    public float DeccelerationTime => _deccelerationTime;

    //public float MaxAngle => _maxAngle;

    //===================================
  }
}
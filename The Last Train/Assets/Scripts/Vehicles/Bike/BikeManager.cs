using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLT.Vehicles.Bike
{
  public class BikeManager : MonoBehaviour
  {
    [SerializeField] private BikeWheel _frontWheel;

    [SerializeField] private BikeWheel _backWheel;

    [SerializeField, Range(-1, 1)] private int _direction = 1;

    //===================================

    public BikeWheel FrontWheel => _frontWheel;
    public BikeWheel BackWheel => _backWheel;

    public int Direction { get => _direction; set => _direction = value; }

    public bool Grounded => _backWheel.Grounded && _frontWheel.Grounded;
    public bool OnlyFrontGrounded => !_backWheel.Grounded && _frontWheel.Grounded;
    public bool OnlyBackGrounded => _backWheel.Grounded && !_frontWheel.Grounded;
    public bool AnyWheelGrounded => _backWheel.Grounded || _frontWheel.Grounded;

    //===================================



    //===================================



    //===================================
  }
}
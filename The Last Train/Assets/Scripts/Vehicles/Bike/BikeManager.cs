using UnityEngine;

using TLT.CameraManager;

namespace TLT.Vehicles.Bike
{
  public class BikeManager : MonoBehaviour
  {
    [SerializeField] private BikeWheel _frontWheel;

    [SerializeField] private BikeWheel _backWheel;

    [Space]
    [SerializeField] private CameraController _cameraController;

    [SerializeField, Range(-1, 1)] private int _direction = 1;

    //===================================

    public BikeWheel FrontWheel => _frontWheel;

    public BikeWheel BackWheel => _backWheel;

    public CameraController CameraController => _cameraController;

    //-----------------------------------

    public int Direction { get => _direction; set => _direction = value; }

    /// <summary>
    /// На земле двумя колесами
    /// </summary>
    public bool Grounded => _backWheel.Grounded && _frontWheel.Grounded;

    /// <summary>
    /// На земле только передним колесом
    /// </summary>
    public bool OnlyFrontGrounded => !_backWheel.Grounded && _frontWheel.Grounded;

    /// <summary>
    /// На земле только задним колесом
    /// </summary>
    public bool OnlyBackGrounded => _backWheel.Grounded && !_frontWheel.Grounded;

    /// <summary>
    /// На земле либо переднее либо заднее колесо
    /// </summary>
    public bool AnyWheelGrounded => _backWheel.Grounded || _frontWheel.Grounded;

    //===================================
  }
}
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
    /// �� ����� ����� ��������
    /// </summary>
    public bool Grounded => _backWheel.Grounded && _frontWheel.Grounded;

    /// <summary>
    /// �� ����� ������ �������� �������
    /// </summary>
    public bool OnlyFrontGrounded => !_backWheel.Grounded && _frontWheel.Grounded;

    /// <summary>
    /// �� ����� ������ ������ �������
    /// </summary>
    public bool OnlyBackGrounded => _backWheel.Grounded && !_frontWheel.Grounded;

    /// <summary>
    /// �� ����� ���� �������� ���� ������ ������
    /// </summary>
    public bool AnyWheelGrounded => _backWheel.Grounded || _frontWheel.Grounded;

    //===================================
  }
}
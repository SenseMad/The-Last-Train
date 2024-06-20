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
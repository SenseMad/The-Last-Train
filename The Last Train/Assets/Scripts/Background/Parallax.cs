using UnityEngine;

namespace TLT.Background
{
  public class Parallax : MonoBehaviour
  {
    [SerializeField] private Transform _target;

    [SerializeField, Range(0, 1)] private float strength = 0.5f;

    [SerializeField] private bool _verticalParallax;

    //-----------------------------------

    private Vector3 _targetPreviousPosition;

    //===================================

    private void Start()
    {
      if (_target == null)
        _target = Camera.main.transform;

      _targetPreviousPosition = _target.position;
    }

    private void Update()
    {
      Vector3 delta = _target.position - _targetPreviousPosition;

      if (_verticalParallax)
        delta.y = 0;

      _targetPreviousPosition = _target.position;

      transform.position += delta * strength;
    }

    //===================================
  }
}
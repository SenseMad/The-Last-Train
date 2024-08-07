using UnityEngine;

public class DirectionMovement : MonoBehaviour
{
  [SerializeField] private Collider2D _objectCollider2D;

  [SerializeField, Tooltip("True if you need to go up when the button is pressed")] private bool _isUp = false;

  [Space]
  [SerializeField] private bool _isActiveDefault = false;

  //===================================

  public Collider2D ObjectCollider2D => _objectCollider2D;

  public bool IsUp => _isUp;

  public bool IsActiveDefault => _isActiveDefault;

  //===================================
}
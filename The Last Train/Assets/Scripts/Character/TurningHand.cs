using UnityEngine;

namespace TLT.CharacterManager
{
  public class TurningHand : MonoBehaviour
  {
    [SerializeField] private Transform _handPivot;

    [SerializeField] private float _minAngle = -45;
    [SerializeField] private float _maxAngle = 45;

    [Space(10)]
    [SerializeField] private Vector2 _offset = new(0, -30.0f);

    //-----------------------------------

    private Character character;

    //===================================

    private void Awake()
    {
      character = GetComponent<Character>();
    }

    private void Update()
    {
      Vector3 mousePosition = character.InputHandler.GetMousePosition();
      Vector2 worldPoint = character.MainCamera.ScreenToWorldPoint(mousePosition);

      Vector2 vector = -_handPivot.up * 0.228f;

      if (Vector2.Distance(_handPivot.position, worldPoint) > 0.5f)
        worldPoint += vector;

      Vector2 vector2 = worldPoint - (Vector2)_handPivot.position;
      vector2 *= character.Direction;
      vector2.Normalize();

      bool isMouseOnRightSide = mousePosition.x >= character.MainCamera.WorldToScreenPoint(_handPivot.position).x;
      if (character.Direction < 0)
        isMouseOnRightSide = !isMouseOnRightSide;

      float angle = Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg;

      if (isMouseOnRightSide)
        angle = Mathf.Clamp(angle, _minAngle, _maxAngle);
      else
      {
        if (angle > 90)
          angle = Mathf.Clamp(angle - 180, _minAngle, _maxAngle) + 180;
        else if (angle < -90)
          angle = Mathf.Clamp(angle + 180, _minAngle, _maxAngle) - 180;
        else
          angle = Mathf.Clamp(angle + 180, _minAngle, _maxAngle) - 180;
      }

      Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
      _handPivot.rotation = Quaternion.Slerp(_handPivot.rotation, targetRotation, Time.deltaTime * 20f);

      Vector3 localScale = _handPivot.localScale;
      localScale.y = isMouseOnRightSide ? 1 : -1;
      _handPivot.localScale = localScale;

      /*Vector3 mousePosition = character.InputHandler.GetMousePosition() + _offset;
      Vector3 worldPoint = character.MainCamera.ScreenToWorldPoint(mousePosition);
      Vector3 difference = worldPoint - _handPivot.position;

      difference.Normalize();

      float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
      bool isMouseOnRightSide = mousePosition.x >= character.MainCamera.WorldToScreenPoint(_handPivot.position).x;

      if (character.Direction < 0)
      {
        //angle = 180 - angle;
        isMouseOnRightSide = !isMouseOnRightSide;
      }

      if (isMouseOnRightSide)
        angle = Mathf.Clamp(angle, _minAngle, _maxAngle);
      else
      {
        if (angle > 90)
          angle = Mathf.Clamp(angle - 180, _minAngle, _maxAngle) + 180;
        else if (angle < -90)
          angle = Mathf.Clamp(angle + 180, _minAngle, _maxAngle) - 180;
        else
          angle = Mathf.Clamp(angle + 180, _minAngle, _maxAngle) - 180;
      }

      _handPivot.rotation = Quaternion.Euler(0, 0, angle);

      Vector3 localScale = _handPivot.localScale;
      localScale.y = isMouseOnRightSide ? 1 : -1;
      _handPivot.localScale = localScale;*/
    }

    //===================================
  }
}
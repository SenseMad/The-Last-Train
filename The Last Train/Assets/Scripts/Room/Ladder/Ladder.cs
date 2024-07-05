using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TLT.CharacterManager;

namespace TLT.Building
{
  public class Ladder : MonoBehaviour
  {
    [SerializeField] private RoomManager _roomNeedOpened;

    [SerializeField] private BoxCollider2D _boxCollider;

    [SerializeField] private BoxCollider2D _colliderRoomOpening;

    [SerializeField] private LayerMask _characterMask;

    //===================================

    public BoxCollider2D BoxCollider => _boxCollider;

    public RoomManager RoomNeedOpened => _roomNeedOpened;

    //===================================

    /*private void Update()
    {
      if (_roomNeedOpened.IsRoomOpen)
        return;

      Collider2D[] colliders = Physics2D.OverlapBoxAll(_colliderRoomOpening.bounds.center, _colliderRoomOpening.bounds.size, 0, _characterMask);

      if (colliders.Length == 0 || colliders == null)
        return;

      foreach (var collider in colliders)
      {
        if (collider.TryGetComponent(out Character parCharacter))
        {
          _roomNeedOpened.OpenRoom();
          return;
        }
      }
    }*/

    //===================================

    private void OnTriggerEnter2D(Collider2D collider)
    {
      if (collider.TryGetComponent(out CharacterLadder parCharacterLadder))
      {
        parCharacterLadder.Ladder = this;
      }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
      if (collider.TryGetComponent(out CharacterLadder parCharacterLadder))
      {
        parCharacterLadder.Rigidbody2D.gravityScale = parCharacterLadder.Gravity;
        parCharacterLadder.BoxCollider2D.isTrigger = false;
        parCharacterLadder.IsLadder = false;
        parCharacterLadder.Character.InputHandler.IsInputHorizontal = true;
        parCharacterLadder.Ladder = null;
      }
    }

    //===================================
  }
}
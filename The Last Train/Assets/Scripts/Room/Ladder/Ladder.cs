using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TLT.CharacterManager;

namespace TLT.Building
{
  public class Ladder : MonoBehaviour
  {
    [SerializeField] private RoomManager _roomNeedOpened;

    [Space]
    [SerializeField] private Collider2D _colliderLadder;
    [SerializeField] private Collider2D _colliderRoomOpening;
    [SerializeField] private Collider2D _colliderUpperLimit;
    [SerializeField] private Collider2D _colliderLowerLimit;

    //-----------------------------------

    /*private CharacterLadder characterLadder;

    private bool isCharacterLadder = false;*/

    //===================================

    public Collider2D ColliderLadder => _colliderLadder;

    public Collider2D ColliderUpperLimit => _colliderUpperLimit;

    public Collider2D ColliderLowerLimit => _colliderLowerLimit;

    public RoomManager RoomNeedOpened => _roomNeedOpened;
    
    /*public bool IsTopLadder { get; set; }
    public bool IsBottomLadder { get; set; }*/

    //===================================

    /*private void TriggerEnter2D()
    {
      Collider2D collider = Physics2D.OverlapBox(_colliderLadder.bounds.center, _colliderLadder.bounds.size, 0, _characterMask);

      if (collider == null)
      {
        isCharacterLadder = false;
        return;
      }

      if (collider.TryGetComponent(out CharacterLadder parCharacterLadder))
      {
        characterLadder = parCharacterLadder;
        parCharacterLadder.Ladder = this;

        isCharacterLadder = true;
      }
    }

    private void TriggerExit2D()
    {
      if (characterLadder == null)
        return;

      if (isCharacterLadder)
        return;

      characterLadder.Rigidbody2D.gravityScale = characterLadder.Gravity;
      characterLadder.Collider2D.isTrigger = false;
      //characterLadder.IsLadder = false;
      characterLadder.Character.InputHandler.IsInputHorizontal = true;
      characterLadder.Ladder = null;

      characterLadder = null;
    }*/

    //===================================
  }
}
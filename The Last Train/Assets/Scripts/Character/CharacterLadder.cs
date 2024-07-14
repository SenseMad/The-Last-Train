using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TLT.Building;

namespace TLT.CharacterManager
{
  public class CharacterLadder : MonoBehaviour
  {
    [SerializeField, Min(0)] private float _climbSpeed;

    //===================================

    public Collider2D Collider2D { get; set; }

    //-----------------------------------

    public Character Character { get; set; }

    public Rigidbody2D Rigidbody2D { get; private set; }

    public bool IsLadder { get; set; }

    public float Gravity { get; private set; }

    public Ladder Ladder { get; set; }

    //===================================

    private void Awake()
    {
      Character = GetComponent<Character>();

      Collider2D = GetComponent<Collider2D>();

      Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
      Gravity = Rigidbody2D.gravityScale;
    }

    private void FixedUpdate()
    {
      Move();
    }

    //===================================

    private void Move()
    {
      if (Ladder == null)
        return;

      if ((Collider2D.bounds.min.y - 0.1f <= Ladder.BoxCollider.bounds.min.y || Collider2D.bounds.min.y - 0.1f >= Ladder.BoxCollider.bounds.max.y) && IsLadder)
      {
        Rigidbody2D.gravityScale = Gravity;
        Collider2D.isTrigger = false;
        IsLadder = false;
        Character.InputHandler.IsInputHorizontal = true;
        return;
      }

      float moveVelocity = _climbSpeed * Character.InputHandler.GetInputVertical();

      if (Ladder != null && moveVelocity != 0)
      {
        IsLadder = true;

        if (!Ladder.RoomNeedOpened.IsRoomOpen)
          Ladder.RoomNeedOpened.OpenRoom();
      }

      if (moveVelocity != 0)
        transform.position = new(Ladder.BoxCollider.bounds.center.x, transform.position.y);

      Vector2 targetVelocity = new(Rigidbody2D.velocity.x, moveVelocity);

      Rigidbody2D.velocity = targetVelocity;

      if (IsLadder)
      {
        Character.InputHandler.IsInputHorizontal = false;

        Rigidbody2D.gravityScale = 0;

        Collider2D.isTrigger = true;
      }
    }

    //===================================
  }
}
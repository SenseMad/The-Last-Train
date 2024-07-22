using UnityEngine;

using TLT.Building;

namespace TLT.CharacterManager
{
  public class CharacterLadder : MonoBehaviour
  {
    [SerializeField, Min(0)] private float _climbSpeed;

    [SerializeField, Min(0)] private float _rayDistance = 0.2f;

    [SerializeField] private LayerMask _ladderMask;

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
      //Move1();
    }

    //===================================

    private void Move()
    {
      Vector2 bottomRay = new(Collider2D.bounds.center.x, Collider2D.bounds.min.y);

      Vector2 rayDirection = Vector2.down;

      RaycastHit2D hit = Physics2D.Raycast(bottomRay, rayDirection, _rayDistance, _ladderMask);

      if (hit.collider == null)
      {
        Ladder = null;
        Character.InputHandler.IsInputHorizontal = true;
        Rigidbody2D.gravityScale = Gravity;
        Collider2D.isTrigger = false;
        return;
      }

      Ladder = hit.collider.GetComponentInParent<Ladder>();

      if (Ladder == null)
        return;

      Collider2D upperLimit = Ladder.ColliderUpperLimit;
      Collider2D lowerLimit = Ladder.ColliderLowerLimit;

      if (hit.collider == upperLimit || lowerLimit.bounds.Contains(hit.point))
      {
        Character.InputHandler.IsInputHorizontal = true;
        Rigidbody2D.gravityScale = Gravity;
        Collider2D.isTrigger = false;
      }

      float moveVelocity = _climbSpeed * Character.InputHandler.GetInputVertical();

      if (hit.collider == upperLimit && moveVelocity > 0)
        return;

      if (lowerLimit.bounds.Contains(hit.point) && moveVelocity < 0)
        return;

      if (moveVelocity != 0)
      {
        if (!Ladder.RoomNeedOpened.IsRoomOpen)
          Ladder.RoomNeedOpened.OpenRoom();

        transform.position = new(Ladder.ColliderLadder.bounds.center.x, transform.position.y);

        Character.InputHandler.IsInputHorizontal = false;

        Rigidbody2D.gravityScale = 0;

        Collider2D.isTrigger = true;
      }

      Vector2 targetVelocity = new(0, moveVelocity);

      Rigidbody2D.velocity = targetVelocity;
    }

    //===================================

    private void OnDrawGizmosSelected()
    {
      if (Collider2D != null)
      {
        Vector2 playerBottom = new(Collider2D.bounds.center.x, Collider2D.bounds.min.y);
        Vector2 rayDirection = Vector2.down;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerBottom, playerBottom + rayDirection * _rayDistance);
      }
    }

    //===================================
  }
}
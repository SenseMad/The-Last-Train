using UnityEngine;

namespace TLT.CharacterManager
{
  public sealed class Movement : MonoBehaviour
  {
    [SerializeField] private float _speedWalking = 7.0f;
    [SerializeField, Range(0, 1)] private float _walkingMultiplierBackwards = 0.7f;

    [SerializeField, Range(0.0f, -50.0f)] private float _rateOfFall = -14.0f;

    [SerializeField] private LayerMask _groundLayer;

    //-----------------------------------

    private Character character;

    private BoxCollider2D boxCollider2D;

    private CharacterState characterState;

    private bool isGrounded;

    //===================================

    public Rigidbody2D Rigidbody2D { get; private set; }

    //===================================

    private void Awake()
    {
      character = GetComponent<Character>();

      boxCollider2D = GetComponent<BoxCollider2D>();

      Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
      Move();

      Flip();

      LimitRateFall();

      //IsGrounded();
    }

    //===================================

    private void Move()
    {
      float moveVelocity = _speedWalking * character.InputHandler.GetInputHorizontal();

      // Проверяем направление взгляда персонажа
      bool facingRight = transform.localRotation.eulerAngles.y == 0;
      // Определяем, движется ли персонаж задом
      bool isMovingBackward = (facingRight && moveVelocity < 0) || (!facingRight && moveVelocity > 0);
      if (isMovingBackward)
        moveVelocity *= _walkingMultiplierBackwards;

      Vector2 targetVelocity = new(moveVelocity, Rigidbody2D.velocity.y);

      Rigidbody2D.velocity = targetVelocity;

      characterState = Rigidbody2D.velocity.x == 0 ? CharacterState.Idle : CharacterState.Walking;
    }

    private void Flip()
    {
      Vector3 cameraPosition = character.MainCamera.WorldToScreenPoint(transform.position);
      Vector2 mousePosition = character.InputHandler.GetMousePosition();

      if (mousePosition.x < cameraPosition.x)
        transform.localRotation = Quaternion.Euler(0, 180, 0);
      else if (mousePosition.x > cameraPosition.x)
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void LimitRateFall()
    {
      if (Rigidbody2D.velocity.y < _rateOfFall)
      {
        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, _rateOfFall);
      }
    }

    private bool IsGrounded()
    {
      Vector2 position = (Vector2)transform.position + boxCollider2D.offset;

      Vector2 bottomCenter = new(position.x, position.y - boxCollider2D.bounds.extents.y / 1.7f);

      return Physics2D.OverlapCircle(bottomCenter, boxCollider2D.size.x / 2f, _groundLayer);
    }

    //===================================

    private void OnDrawGizmos()
    {
      if (boxCollider2D != null)
      {
        Vector2 position = (Vector2)transform.position + boxCollider2D.offset;
        Vector2 bottomCenter = new(position.x, position.y - boxCollider2D.bounds.extents.y / 1.7f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(bottomCenter, boxCollider2D.size.x / 2f);
      }
    }

    //===================================
  }
}
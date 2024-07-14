using TLT.Vehicles.Bike;
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

    private Collider2D collider2D;

    //===================================

    public Rigidbody2D Rigidbody2D { get; private set; }

    //===================================

    private void Awake()
    {
      character = GetComponent<Character>();

      collider2D = GetComponent<Collider2D>();

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
      if (!character.InputHandler.IsInputHorizontal)
        return;

      float moveVelocity = _speedWalking * character.InputHandler.GetInputHorizontal();

      // Проверяем направление взгляда персонажа
      /*bool facingRight = transform.localRotation.eulerAngles.y == 0;
      //bool facingRight = ;
      // Определяем, движется ли персонаж задом
      bool isMovingBackward = (facingRight && moveVelocity < 0) || (!facingRight && moveVelocity > 0);
      if (isMovingBackward)
        moveVelocity *= _walkingMultiplierBackwards;*/

      Vector2 targetVelocity = new(moveVelocity, Rigidbody2D.velocity.y);

      Rigidbody2D.velocity = targetVelocity;

      //character.CharacterState = Rigidbody2D.velocity.x == 0 ? CharacterState.Idle : CharacterState.Walking;
      if (Rigidbody2D.velocity.x == 0)
      {
        character.Animator.SetBool("IsWalk", false);
      }
      else
      {
        character.Animator.SetBool("IsWalk", true);
      }
    }

    private void Flip()
    {
      Vector3 cameraPosition = character.MainCamera.WorldToScreenPoint(transform.position);
      Vector2 mousePosition = character.InputHandler.GetMousePosition();

      /*transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);

      _bikeManager.Direction *= -1;*/

      if (mousePosition.x < cameraPosition.x)
        character.Direction = -1;
      //transform.localRotation = Quaternion.Euler(0, 180, 0);
      else if (mousePosition.x > cameraPosition.x)
        character.Direction = 1;

      transform.localScale = new Vector3(character.Direction, transform.localScale.y, transform.localScale.z);
      //transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void LimitRateFall()
    {
      if (Rigidbody2D.velocity.y < _rateOfFall)
      {
        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, _rateOfFall);
      }
    }

    /*private bool IsGrounded()
    {
      Vector2 position = (Vector2)transform.position + boxCollider2D.offset;

      Vector2 bottomCenter = new(position.x, position.y - boxCollider2D.bounds.extents.y / 1.7f);

      return Physics2D.OverlapCircle(bottomCenter, boxCollider2D.size.x / 2f, _groundLayer);
    }*/

    //===================================

    private void OnDrawGizmos()
    {
      if (collider2D != null)
      {
        Vector2 position = (Vector2)transform.position + collider2D.offset;
        Vector2 bottomCenter = new(position.x, position.y - collider2D.bounds.extents.y / 1.7f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(bottomCenter, collider2D.bounds.size.x / 2f);
      }
    }

    //===================================
  }
}
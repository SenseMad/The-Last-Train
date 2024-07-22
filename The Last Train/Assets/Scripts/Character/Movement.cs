using TLT.Vehicles.Bike;
using UnityEngine;
using UnityEngine.Audio;

namespace TLT.CharacterManager
{
  public sealed class Movement : MonoBehaviour
  {
    [SerializeField] private float _speedWalking = 7.0f;
    [SerializeField, Range(0, 1)] private float _walkingMultiplierBackwards = 0.7f;

    [SerializeField, Range(0.0f, -50.0f)] private float _rateOfFall = -14.0f;

    [SerializeField] private LayerMask _groundLayer;

    [Space]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _soundMove;

    //-----------------------------------

    private Character character;

    private Collider2D collider2D;

    private float stepInterval = 0.5f;
    private float stepTimer;

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

    private void SoundSFX(AudioClip audioClip, bool parLoop)
    {
      if (_audioSource.isPlaying)
        return;

      _audioSource.clip = audioClip;
      _audioSource.loop = parLoop;
      _audioSource.Play();
    }

    private void Move()
    {
      /*if (!character.InputHandler.IsInputHorizontal)
      {
        character.Animator.SetBool("IsWalk", false);
        return;
      }*/

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

        if (Mathf.Abs(character.InputHandler.GetInputHorizontal()) > 0.1f)
        {
          stepTimer -= Time.deltaTime;
          if (stepTimer <= 0)
          {
            SoundSFX(_soundMove, false);
            stepTimer = stepInterval;
          }
        }
        else
        {
          stepTimer = 0;
        }
      }
    }

    private void Flip()
    {
      Vector3 cameraPosition = character.MainCamera.WorldToScreenPoint(transform.position);
      Vector2 mousePosition = character.InputHandler.GetMousePosition();

      if (mousePosition.x < cameraPosition.x)
        character.Direction = -1;
      else if (mousePosition.x > cameraPosition.x)
        character.Direction = 1;

      transform.localScale = new Vector3(character.Direction, transform.localScale.y, transform.localScale.z);
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
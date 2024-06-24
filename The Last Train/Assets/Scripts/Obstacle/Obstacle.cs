using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
  [SerializeField] private float _pushForce = 5.0f;

  [SerializeField] private LayerMask _playerLayer;

  //===================================

  private new Rigidbody2D rigidbody2D;

  private new Collider2D collider2D;

  private bool hasCollided;

  private float pushTimer = 0.0f;

  //===================================

  private void Awake()
  {
    rigidbody2D = GetComponent<Rigidbody2D>();

    collider2D = GetComponent<Collider2D>();
  }

  //===================================

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (hasCollided)
      return;

    if (((1 << collision.gameObject.layer) & _playerLayer.value) != 0)
    {
      hasCollided = true;

      //gameObject.layer = "PushObstacle";

      //collider2D.enabled = false;

      //Vector2 pushDirection = (transform.position - collision.transform.position).normalized;
      //rigidbody2D.AddForce(pushDirection * _pushForce, ForceMode2D.Impulse);

      //hasCollided = true;

      //rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }
  }

  //===================================
}
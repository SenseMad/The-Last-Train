using UnityEngine;

using TLT.Bike.Bike;

public class Obstacle : MonoBehaviour
{
  private new Collider2D collider2D;

  private bool hasCollided;

  //===================================

  private void Awake()
  {
    collider2D = GetComponent<BoxCollider2D>();
  }

  //===================================

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (hasCollided)
      return;

    if (collision.collider.TryGetComponent(out BikeManager parBikeManager))
    {
      Physics2D.IgnoreCollision(collision.collider, collider2D, true);
      Physics2D.IgnoreCollision(parBikeManager.FrontWheel.Collider2D, collider2D, true);
      Physics2D.IgnoreCollision(parBikeManager.BackWheel.Collider2D, collider2D, true);
      hasCollided = true;
    }
  }

  //===================================
}
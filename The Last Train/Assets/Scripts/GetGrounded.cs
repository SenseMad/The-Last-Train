using UnityEngine;

public class GetGrounded : MonoBehaviour
{
  [SerializeField, Min(0)] private float _rayDistance = 0.05f;

  //===================================

  private BoxCollider2D[] ignoreColliders;

  //===================================

  private void Awake()
  {
    ignoreColliders = GetComponentsInChildren<BoxCollider2D>();
  }

  //===================================

  public void GetGround(BoxCollider2D parBoxCollider2D, out bool isGrounded)
  {
    Vector3 bottomLeft = new(parBoxCollider2D.bounds.min.x, parBoxCollider2D.bounds.min.y);
    Vector3 bottomCenter = new(parBoxCollider2D.bounds.center.x, parBoxCollider2D.bounds.min.y);
    Vector3 bottomRight = new(parBoxCollider2D.bounds.max.x, parBoxCollider2D.bounds.min.y);

    Debug.DrawLine(bottomLeft, bottomLeft + Vector3.down * _rayDistance, Color.green);
    Debug.DrawLine(bottomCenter, bottomCenter + Vector3.down * _rayDistance, Color.green);
    Debug.DrawLine(bottomRight, bottomRight + Vector3.down * _rayDistance, Color.green);

    Ray[] rays = new Ray[3];
    rays[0] = new Ray(bottomLeft, -transform.up);
    rays[1] = new Ray(bottomCenter, -transform.up);
    rays[2] = new Ray(bottomRight, -transform.up);

    RaycastHit2D[] hits = new RaycastHit2D[rays.Length];
    for (int i = 0; i < rays.Length; i++)
      hits[i] = Physics2D.Raycast(rays[i].origin, rays[i].direction, _rayDistance);

    isGrounded = false;

    foreach (var hit in hits)
    {
      if (hit.collider == null)
        continue;

      foreach (var ignoreCollider in ignoreColliders)
      {
        if (hit.collider == ignoreCollider)
          return;
      }

      isGrounded = true;
      break;
    }

    //===================================



    //===================================
  }
}
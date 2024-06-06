using UnityEngine;

public class GetGrounded : MonoBehaviour
{
  [SerializeField, Min(0)] private float _rayDistance = 0.05f;

  //===================================

  public BoxCollider2D[] IgnoreColliders { get; private set; }

  //===================================



  //===================================

  private void Awake()
  {
    IgnoreColliders = GetComponentsInChildren<BoxCollider2D>();
  }

  private void FixedUpdate()
  {
    
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

      foreach (var ignoreCollider in IgnoreColliders)
      {
        if (hit.collider == ignoreCollider)
          return;
      }

      //GetSurfaceAngle(hit);

      isGrounded = true;
      break;
    }
  }

  //===================================

  public void GetSurfaceAngle(RaycastHit2D parHit)
  {
    float groundAngle = Vector2.Angle(parHit.normal, Vector2.up);

    Quaternion targetRotation = Quaternion.Euler(0, 0, Mathf.Clamp(-groundAngle, -45, 45));
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 200 * Time.deltaTime);
  }

  //===================================
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLT.Vehicles
{
  public class VehicleGrounded : MonoBehaviour
  {
    [SerializeField] private Collider2D _frontWheel;
    [SerializeField] private Collider2D _backWheel;

    [SerializeField] private LayerMask ignoreLayer;

    [Space(10)]
    [SerializeField, Min(0)] private float _rayDistance = 0.05f;

    //===================================

    public BoxCollider2D[] ignoreColliders;

    //===================================

    private void Awake()
    {
      ignoreColliders = GetComponentsInChildren<BoxCollider2D>();
    }

    private void Update()
    {
      GetVehicleGround();
    }

    //===================================

    public bool GetVehicleGround()
    {
      return false;

      /*if (Physics2D.OverlapCircle(_frontWheel.transform.position, _rayDistance) || Physics2D.OverlapCircle(_backWheel.transform.position, _rayDistance))
      {
        foreach (var ignoreCollider in ignoreColliders)
        {
          if (ignoreCollider == _frontWheel || ignoreCollider == _backWheel)
            continue;

          return false;
        }

        return true;
      }

      return false;*/
    }

    //===================================

    private void OnDrawGizmosSelected()
    {
      /*Gizmos.color = Color.green;
      Gizmos.DrawWireSphere(_frontWheel.transform.position, _rayDistance);
      Gizmos.DrawWireSphere(_backWheel.transform.position, _rayDistance);*/
    }

    /*public bool GetGround(BoxCollider2D parBoxCollider2D)
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

      bool isGrounded = false;

      foreach (var hit in hits)
      {
        if (hit.collider == null)
          continue;

        foreach (var ignoreCollider in IgnoreColliders)
        {
          if (hit.collider == ignoreCollider)
            return false;
        }

        //GetSurfaceAngle(hit);

        isGrounded = true;
        break;
      }

      return isGrounded;
    }*/
  }
}
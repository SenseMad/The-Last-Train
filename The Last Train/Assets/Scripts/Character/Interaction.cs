using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TLT.CharacterManager
{
  public class Interaction : MonoBehaviour
  {
    //[SerializeField] private Transform _point;

    //[SerializeField] private float _radius = 0.5f;

    [SerializeField] private float _maxDistance = 0.5f;

    [SerializeField] private LayerMask _layerMask;

    //-----------------------------------

    /*private List<ObjectInteraction> objectInteractions = new();

    private ObjectInteraction _currentObjectInteraction;*/

    private BoxCollider2D boxCollider2D;

    //===================================

    public ObjectInteraction ObjectInteraction { get; private set; }

    //===================================

    private void Awake()
    {
      boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
      if (ObjectInteraction != null)
        ObjectInteraction.Select();
    }

    private void OnDisable()
    {
      if (ObjectInteraction != null)
        ObjectInteraction.DeSelect();
    }

    private void Update()
    {
      CheckForObjectsInSight();
    }

    //===================================

    private void CheckForObjectsInSight()
    {
      Vector3 center = boxCollider2D.bounds.center;
      Vector3 topCenter = new(center.x, boxCollider2D.bounds.max.y);
      Vector3 bottomCenter = new(center.x, boxCollider2D.bounds.min.y);

      /*Debug.DrawLine(center, center + transform.right * _maxDistance, Color.green);
      Debug.DrawLine(topCenter, topCenter + transform.right * _maxDistance, Color.green);
      Debug.DrawLine(bottomCenter, bottomCenter + transform.right * _maxDistance, Color.green);*/

      Ray[] rays = new Ray[3];
      rays[0] = new Ray(center, transform.right);
      rays[1] = new Ray(topCenter, transform.right);
      rays[2] = new Ray(bottomCenter, transform.right);

      RaycastHit2D[] hits = new RaycastHit2D[rays.Length];
      for (int i = 0; i < rays.Length; i++)
      {
        hits[i] = Physics2D.Raycast(rays[i].origin, rays[i].direction, _maxDistance, _layerMask);
      }

      ObjectInteraction newObjectInteraction = null;

      foreach (var hit in hits)
      {
        if (hit.collider == null)
          continue;

        if (hit.collider.TryGetComponent(out ObjectInteraction objectInteraction))
        {
          newObjectInteraction = objectInteraction;
          break;
        }
      }

      if (newObjectInteraction != null)
      {
        if (ObjectInteraction != newObjectInteraction)
        {
          if (ObjectInteraction != null)
            ObjectInteraction.DeSelect();

          newObjectInteraction.Select();
          ObjectInteraction = newObjectInteraction;
        }
      }
      else
      {
        if (ObjectInteraction != null)
        {
          ObjectInteraction.DeSelect();
          ObjectInteraction = null;
        }
      }      
    }    

    //===================================
  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLT.Enemy
{
  public class EnemyParticleDeath : MonoBehaviour
  {
    [SerializeField] private GameObject _objectPrefab;

    [SerializeField, Min(0)] private int _particleAmount;

    [SerializeField, Min(0)] private Vector2 _minXY;
    
    [SerializeField, Min(0)] private Vector2 _maxXY;

    //===================================



    //===================================



    //===================================

    private void CreateObject()
    {
      for (int i = 0; i < _particleAmount; i++)
      {
        GameObject objectInstance = Instantiate(_objectPrefab, transform);

        float xPosition = transform.position.x + Random.Range(_minXY.x, _maxXY.x);
        float yPosition = transform.position.y + Random.Range(_minXY.y, _maxXY.y);

        objectInstance.transform.position = new Vector3(xPosition, yPosition);
      }
    }

    //===================================
  }
}
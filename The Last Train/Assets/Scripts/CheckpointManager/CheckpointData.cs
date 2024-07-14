using System;
using UnityEngine;

namespace TLT.Checkpoints
{
  [Serializable]
  public class CheckpointData
  {
    [SerializeField] private string _levelName;
    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;

    //===================================

    public CheckpointData(string parLevelName, Vector3 parPosition, Quaternion parRotation)
    {
      _levelName = parLevelName;
      _position = parPosition;
      _rotation = parRotation;
    }

    //===================================

    public string LevelName => _levelName;
    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;

    //===================================
  }
}
using UnityEngine;
using System;
using Newtonsoft.Json.Linq;

using TLT.Save;
using TLT.Data;

namespace TLT.Bike.Bike
{
  public class BikeManager : MonoBehaviour, ISaveLoad, IBikeBootstrap
  {
    [SerializeField] private BikeWheel _frontWheel;
    [SerializeField] private BikeWheel _backWheel;

    [Space]
    [SerializeField, Range(-1, 1)] private int _direction = 1;

    //-----------------------------------

    private BikeController bikeController;
    private BikeBody bikeBody;

    //===================================

    public BikeWheel FrontWheel => _frontWheel;

    public BikeWheel BackWheel => _backWheel;

    //-----------------------------------

    public int Direction { get => _direction; set => _direction = value; }

    /// <summary>
    /// На земле двумя колесами
    /// </summary>
    public bool Grounded => _backWheel.Grounded && _frontWheel.Grounded;

    /// <summary>
    /// На земле только передним колесом
    /// </summary>
    public bool OnlyFrontGrounded => !_backWheel.Grounded && _frontWheel.Grounded;

    /// <summary>
    /// На земле только задним колесом
    /// </summary>
    public bool OnlyBackGrounded => _backWheel.Grounded && !_frontWheel.Grounded;

    /// <summary>
    /// На земле либо переднее либо заднее колесо
    /// </summary>
    public bool AnyWheelGrounded => _backWheel.Grounded || _frontWheel.Grounded;

    //===================================

    public void CustomAwake()
    {
      bikeController = GetComponent<BikeController>();
      bikeBody = GetComponent<BikeBody>();
    }

    public void CustomStart() { }

    //===================================

    public ObjectData SaveData()
    {
      string objectName = transform.name;
      Vector3 rotation = transform.rotation.eulerAngles;
      Vector3 position = transform.position;
      Vector3 scale = transform.localScale;

      ObjectData data = new(objectName);
      data.Parameters["Position"] = new float[] { position.x, position.y, position.z };
      data.Parameters["Rotation"] = new float[] { rotation.x, rotation.y, rotation.z };
      data.Parameters["Scale"] = new float[] { scale.x, scale.y, scale.z };
      data.Parameters["IsInCar"] = bikeController.IsInCar;

      return data;
    }

    public void LoadData(ObjectData parObjectData)
    {
      if (parObjectData.ObjectName == gameObject.name)
      {
        LoadTransform(parObjectData);
      }
    }

    //===================================

    private void LoadTransform(ObjectData parObjectData)
    {
      if (parObjectData.Parameters.TryGetValue("Position", out var position) && position is JArray positionArray)
      {
        transform.position = new Vector3(positionArray[0].ToObject<float>(), positionArray[1].ToObject<float>(), positionArray[2].ToObject<float>());
      }

      if (parObjectData.Parameters.TryGetValue("Rotation", out var rotation) && rotation is JArray rotationArray)
      {
        transform.rotation = Quaternion.Euler(rotationArray[0].ToObject<float>(), rotationArray[1].ToObject<float>(), rotationArray[2].ToObject<float>());
      }

      if (parObjectData.Parameters.TryGetValue("Scale", out var scale) && scale is JArray scaleArray)
      {
        transform.localScale = new Vector3(scaleArray[0].ToObject<float>(), scaleArray[1].ToObject<float>(), scaleArray[2].ToObject<float>());

        Direction = scaleArray[0].ToObject<int>();
      }

      if (parObjectData.Parameters.TryGetValue("IsInCar", out var isInCar) && isInCar is bool)
      {
        bikeController.IsInCar = (bool)isInCar;

        if (bikeController.IsInCar)
          bikeBody.CallEventOnGetInCar();
        else
          bikeBody.CallEventOnGetOutCar();
      }
    }

    //===================================
  }
}
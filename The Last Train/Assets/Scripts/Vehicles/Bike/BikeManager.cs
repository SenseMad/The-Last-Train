using UnityEngine;
using System.Collections.Generic;
using System;
using TLT.Json;

using TLT.Save;

namespace TLT.Vehicles.Bike
{
  public class BikeManager : MonoBehaviour, ISaveLoad
  {
    [SerializeField] private BikeWheel _frontWheel;

    [SerializeField] private BikeWheel _backWheel;

    [SerializeField, Range(-1, 1)] private int _direction = 1;

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

    public void SaveData(Dictionary<string, Dictionary<string, object>> parData)
    {
      Vector3 position = transform.position;
      Vector3 rotation = transform.rotation.eulerAngles;
      Vector3 scale = transform.localScale;

      if (!parData.ContainsKey(BikeKeySaveLoad.BIKE_KEY))
        parData.Add(BikeKeySaveLoad.BIKE_KEY, new Dictionary<string, object>());

      parData[BikeKeySaveLoad.BIKE_KEY][BikeKeySaveLoad.POSITION_KEY] = new float[] { position.x, position.y, position.z };
      parData[BikeKeySaveLoad.BIKE_KEY][BikeKeySaveLoad.ROTATION_KEY] = new float[] { rotation.x, rotation.y, rotation.z };
      parData[BikeKeySaveLoad.BIKE_KEY][BikeKeySaveLoad.SCALE_KEY] = new float[] { scale.x, scale.y, scale.z };
      parData[BikeKeySaveLoad.BIKE_KEY][BikeKeySaveLoad.DIRECTION_KEY] = Direction;
    }

    public void LoadData(Dictionary<string, Dictionary<string, object>> parData)
    {
      if (parData.TryGetValue(BikeKeySaveLoad.BIKE_KEY, out Dictionary<string, object> parObject))
      {
        if (parObject.TryGetValue(BikeKeySaveLoad.POSITION_KEY, out object parPosition))
        {
          Vector3 position = ConvertJson.ConvertFromJsonToVector3(parPosition);

          transform.position = position;
        }

        if (parObject.TryGetValue(BikeKeySaveLoad.ROTATION_KEY, out object parRotation))
        {
          Vector3 rotation = ConvertJson.ConvertFromJsonToVector3(parRotation);

          transform.rotation = Quaternion.Euler(rotation);
        }

        if (parObject.TryGetValue(BikeKeySaveLoad.SCALE_KEY, out object parScale))
        {
          Vector3 scale = ConvertJson.ConvertFromJsonToVector3(parScale);

          transform.localScale = scale;
        }

        if (parObject.TryGetValue(BikeKeySaveLoad.DIRECTION_KEY, out object parDirection))
        {
          string number = $"{parDirection}";

          Direction = int.Parse(number);
        }
      }

      /*if (parData.TryGetValue(PositionKey, out object parObject))
      {
        Vector3 position = ConvertFromJsonToVector3(parObject);

        transform.position = position;

        float[] positionArray = parObject as float[];
        if (parObject != null && positionArray.Length == 3)
        {
          Vector3 position = new(positionArray[0], positionArray[1], positionArray[2]);
          transform.position = position;
        }
      }*/
    }

    //===================================
  }
}
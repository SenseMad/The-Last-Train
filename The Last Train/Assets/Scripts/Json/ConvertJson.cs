using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace TLT.Json
{
  public static class ConvertJson
  {
    public static Vector3 ConvertFromJsonToVector3(object parObject)
    {
      string input = $"{parObject}";
      input = input.Replace("[", "").Replace("]", "").Replace(" ", "");

      string[] parts = input.Split(',');
      List<float> numbers = new();

      CultureInfo culture = CultureInfo.InvariantCulture;

      foreach (var part in parts)
      {
        if (float.TryParse(part, NumberStyles.Float, culture, out float number))
          numbers.Add(number);
        else
          Debug.Log($"Number parsing error: {part}");
      }

      return new Vector3(numbers[0], numbers[1], numbers[2]);
    }
  }
}
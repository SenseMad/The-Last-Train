using UnityEngine;

public static class Utilities
{
  public static float Remap(this float value, float from1, float to1, float from2, float to2)
  {
    return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
  }

  public static int Remap(this int value, int from1, int to1, int from2, int to2)
  {
    return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
  }
}
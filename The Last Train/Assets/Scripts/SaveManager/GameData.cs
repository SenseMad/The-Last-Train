using System;
using System.Collections.Generic;

namespace TLT.Data
{
  [Serializable]
  public class GameData
  {
    public Dictionary<string, LevelData> Levels = new();

    public ObjectData PlayerData;
  }
}
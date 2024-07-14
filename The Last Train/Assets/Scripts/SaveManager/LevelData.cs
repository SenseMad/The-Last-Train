using System;
using System.Collections.Generic;

namespace TLT.Data
{
  [Serializable]
  public class LevelData
  {
    public string LevelName;
    public List<ObjectData> Objects = new();

    //===================================

    public LevelData(string parLevelName)
    {
      LevelName = parLevelName;
    }

    //===================================
  }
}
using System;
using System.Collections.Generic;

namespace TLT.Data
{
  [Serializable]
  public class ObjectData
  {
    public string ObjectName;

    public Dictionary<string, object> Parameters = new();

    //===================================

    public ObjectData(string parObjectName)
    {
      ObjectName = parObjectName;
    }

    //===================================
  }
}
using System.Collections.Generic;

namespace TLT.Save
{
  interface ISaveLoad
  {
    void SaveData(Dictionary<string, Dictionary<string, object>> parData);
    void LoadData(Dictionary<string, Dictionary<string, object>> parData);
  }
}
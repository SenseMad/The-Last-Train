using TLT.Data;

namespace TLT.Save
{
  interface ISaveLoad
  {
    ObjectData SaveData();
    void LoadData(ObjectData parObjectData);
  }
}
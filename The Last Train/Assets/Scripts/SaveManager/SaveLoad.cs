using TLT.Data;
using UnityEngine;

namespace TLT.Save
{
  public class SaveLoad : MonoBehaviour
  {
    private ISaveLoad[] saveLoadComponents;

    //===================================

    private void Awake()
    {
      saveLoadComponents = GetComponentsInChildren<ISaveLoad>(true);
    }

    //===================================

    public ObjectData Save()
    {
      if (saveLoadComponents == null)
      {
        Debug.Log($"{gameObject.name}");
        return null;
      }

      foreach (var component in saveLoadComponents)
      {
        return component.SaveData();
      }

      return null;
    }

    public void Load(ObjectData parObjectData)
    {
      foreach (var component in saveLoadComponents)
      {
        component.LoadData(parObjectData);
      }
    }

    //===================================
  }
}
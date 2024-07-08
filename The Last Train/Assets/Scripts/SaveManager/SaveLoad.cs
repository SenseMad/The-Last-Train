using System.Collections.Generic;
using UnityEngine;

namespace TLT.Save
{
  public class SaveLoad : MonoBehaviour
  {
    private ISaveLoad[] saveLoadComponents;

    //===================================

    private void Awake()
    {
      saveLoadComponents = GetComponentsInChildren<ISaveLoad>();
    }

    //===================================

    public void Save(Dictionary<string, Dictionary<string, object>> parData)
    {
      foreach (var component in saveLoadComponents)
      {
        component.SaveData(parData);
      }
    }

    public void Load(Dictionary<string, Dictionary<string, object>> parData)
    {
      foreach (var component in saveLoadComponents)
      {
        component.LoadData(parData);
      }
    }

    //===================================
  }
}
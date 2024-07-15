using System.Linq;
using UnityEngine;

namespace TLT.Save
{
  public class SaveLoads : MonoBehaviour
  {
    private static SaveLoads instance;

    //===================================

    public static SaveLoads Instance
    {
      get
      {
        return instance != null ? instance : FindAnyObjectByType<SaveLoads>();
      }
    }

    internal ISaveLoad[] SaveLoadObjects { get; private set; }

    //===================================

    private void Awake()
    {
      if (instance != null && instance != this)
      {
        Destroy(this);
        return;
      }

      instance = GetComponent<SaveLoads>();

      SaveLoadObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ISaveLoad>().ToArray();
    }

    //===================================
  }
}
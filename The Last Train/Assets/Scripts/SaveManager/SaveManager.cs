using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace TLT.Save
{
  public class SaveManager : MonoBehaviour
  {
    private static SaveManager instance;

    private Dictionary<string, Dictionary<string, object>> saveData = new();

    //===================================

    public static SaveManager Instance
    {
      get
      {
        if (instance == null)
        {
          instance = FindAnyObjectByType<SaveManager>();
          if (instance == null)
          {
            GameObject obj = new("SaveManager");
            instance = obj.AddComponent<SaveManager>();
          }
        }
        return instance;
      }
    }

    //===================================

    private void Start()
    {
      //LoadGame();
    }

    /*private void OnDisable()
    {
      SaveGame();
    }*/

    private void OnApplicationQuit()
    {
      //SaveGame();
    }

    //===================================

    public void SaveGame()
    {
      saveData.Clear();

      SaveLoad[] saveLoadObjects = FindObjectsByType<SaveLoad>(FindObjectsSortMode.None);

      foreach (SaveLoad saveLoad in saveLoadObjects)
      {
        saveLoad.Save(saveData);
      }

      string json = JsonConvert.SerializeObject(saveData);
      File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

      Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
      if (File.Exists(Application.persistentDataPath + "/savefile.json"))
      {
        string json = File.ReadAllText(Application.persistentDataPath + "/savefile.json");
        saveData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(json);

        SaveLoad[] saveLoadObjects = FindObjectsByType<SaveLoad>(FindObjectsSortMode.None);
        foreach (SaveLoad saveLoad in saveLoadObjects)
        {
          saveLoad.Load(saveData);
        }

        Debug.Log("Game Loaded");
      }
    }

    //===================================
  }
}
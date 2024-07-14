using System;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Linq;

using TLT.Data;

namespace TLT.Save
{
  public sealed class SaveManager
  {
    private GameData gameData;

    private SaveLoads saveLoads;

    private PlayerSaveLoad playerSaveLoad;

    //===================================

    public event Action OnSaveGame;

    //===================================

    public void Init()
    {
      gameData = new();

      saveLoads = SaveLoads.Instance;

      playerSaveLoad = PlayerSaveLoad.Instance;
    }

    public void SaveGame()
    {
      string currentLevelName = SceneManager.GetActiveScene().name;

      if (!gameData.Levels.ContainsKey(currentLevelName))
      {
        gameData.Levels[currentLevelName] = new LevelData(currentLevelName);
      }

      LevelData levelData = gameData.Levels[currentLevelName];
      levelData.Objects.Clear();

      foreach (var saveLoad in saveLoads.SaveLoadObjects)
      {
        ObjectData objectData = saveLoad.SaveData();
        if (objectData == null)
          continue;

        if (objectData.ObjectName == "PlayerData")
        {
          gameData.PlayerData = objectData;
        }
        else
        {
          levelData.Objects.Add(objectData);
        }
      }

      string jsonData = JsonConvert.SerializeObject(gameData, Formatting.Indented);
      File.WriteAllText(Application.persistentDataPath + "/gameData.json", jsonData);
    }

    public void LoadGame()
    {
      if (File.Exists(Application.persistentDataPath + "/gameData.json"))
      {
        string jsonData = File.ReadAllText(Application.persistentDataPath + "/gameData.json");
        gameData = JsonConvert.DeserializeObject<GameData>(jsonData);

        LoadCurrentLevelData();
        LoadPlayerData();
      }
      else
      {
        gameData = new();
      }
    }

    public void DeleteSaveGame()
    {
      if (File.Exists(Application.persistentDataPath + "/gameData.json"))
      {
        gameData = new();

        string jsonData = JsonConvert.SerializeObject(gameData, Formatting.Indented);
        File.WriteAllText(Application.persistentDataPath + "/gameData.json", jsonData);
      }
    }

    private void LoadCurrentLevelData()
    {
      string currentLevelName = SceneManager.GetActiveScene().name;

      if (!gameData.Levels.ContainsKey(currentLevelName))
        return;

      LevelData levelData = gameData.Levels[currentLevelName];

      foreach (var saveLoad in saveLoads.SaveLoadObjects)
      {
        ObjectData data = levelData.Objects.Find(obj => obj.ObjectName == saveLoad.SaveData().ObjectName);
        if (data == null)
          continue;

        saveLoad.LoadData(data);
      }
    }

    #region PlayerData

    public void SavePlayerData()
    {
      string currentLevelName = SceneManager.GetActiveScene().name;

      if (!gameData.Levels.ContainsKey(currentLevelName))
      {
        gameData.Levels[currentLevelName] = new LevelData(currentLevelName);
      }

      LevelData levelData = gameData.Levels[currentLevelName];
      levelData.Objects.Clear();

      foreach (var saveLoad in saveLoads.SaveLoadObjects)
      {
        ObjectData objectData = saveLoad.SaveData();
        if (objectData == null)
          continue;

        if (objectData.ObjectName != "PlayerData")
          continue;

        gameData.PlayerData = objectData;
      }

      string jsonData = JsonConvert.SerializeObject(gameData, Formatting.Indented);
      File.WriteAllText(Application.persistentDataPath + "/gameData.json", jsonData);
    }

    private void LoadPlayerData()
    {
      if (playerSaveLoad != null)
      {
        playerSaveLoad.LoadData(gameData.PlayerData);
      }
    }

    #endregion

    //===================================
  }
}
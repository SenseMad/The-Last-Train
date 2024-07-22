using System;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

using TLT.Data;

namespace TLT.Save
{
  public sealed class SaveManager
  {
    private GameData gameData;

    private SaveLoads saveLoads;

    private PlayerSaveLoad playerSaveLoad;

    //-----------------------------------

    private bool isSavesEnabled = false;

    //===================================

    public event Action OnSaveGame;

    //===================================

    public void Init()
    {
      if (!isSavesEnabled)
        return;

      gameData = new();

      saveLoads = SaveLoads.Instance;

      playerSaveLoad = PlayerSaveLoad.Instance;
    }

    #region SaveGame

    public void SaveGame()
    {
      if (!isSavesEnabled)
        return;

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

    #endregion

    #region LoadGame

    public void LoadGame()
    {
      if (!isSavesEnabled)
        return;

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

    private void LoadCurrentLevelData()
    {
      if (!isSavesEnabled)
        return;

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

    #endregion

    #region DeleteSaveGame

    public void DeleteSaveGameLevels()
    {
      if (!isSavesEnabled)
        return;

      if (File.Exists(Application.persistentDataPath + "/gameData.json"))
      {
        string jsonData = File.ReadAllText(Application.persistentDataPath + "/gameData.json");
        gameData = JsonConvert.DeserializeObject<GameData>(jsonData);

        gameData.Levels.Clear();

        jsonData = JsonConvert.SerializeObject(gameData, Formatting.Indented);
        File.WriteAllText(Application.persistentDataPath + "/gameData.json", jsonData);
      }
    }

    public void DeleteSaveGame(string parLevelName)
    {
      if (!isSavesEnabled)
        return;

      if (File.Exists(Application.persistentDataPath + "/gameData.json"))
      {
        string jsonData = File.ReadAllText(Application.persistentDataPath + "/gameData.json");
        gameData = JsonConvert.DeserializeObject<GameData>(jsonData);

        if (!gameData.Levels.ContainsKey(parLevelName))
          return;

        gameData.Levels.Remove(parLevelName);

        jsonData = JsonConvert.SerializeObject(gameData, Formatting.Indented);
        File.WriteAllText(Application.persistentDataPath + "/gameData.json", jsonData);
      }
    }

    public void DeleteAllSaveGame()
    {
      if (!isSavesEnabled)
        return;

      if (File.Exists(Application.persistentDataPath + "/gameData.json"))
      {
        gameData = new();

        string jsonData = JsonConvert.SerializeObject(gameData, Formatting.Indented);
        File.WriteAllText(Application.persistentDataPath + "/gameData.json", jsonData);
      }
    }

    #endregion

    #region PlayerData

    public void SavePlayerData()
    {
      if (!isSavesEnabled)
        return;

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
      if (!isSavesEnabled)
        return;

      if (playerSaveLoad != null)
      {
        playerSaveLoad.LoadData(gameData.PlayerData);
      }
    }

    #endregion

    //===================================
  }
}
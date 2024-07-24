using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Zenject;

using TLT.Data;
using TLT.CharacterManager;

namespace TLT.Save
{
  public sealed class PlayerSaveLoad: MonoBehaviour, ISaveLoad
  {
    private static PlayerSaveLoad instance;

    private Character character;

    //===================================

    [Inject]
    private void Construct(Character parCharacter)
    {
      character = parCharacter;
    }

    //===================================

    public static PlayerSaveLoad Instance
    {
      get
      {
        return instance != null ? instance : FindAnyObjectByType<PlayerSaveLoad>();
      }
    }

    //===================================

    private void Awake()
    {
      if (instance != null && instance != this)
      {
        Destroy(this);
        return;
      }

      instance = GetComponent<PlayerSaveLoad>();
    }

    //===================================

    public ObjectData SaveData()
    {
      string parObjectName = "PlayerData";

      ObjectData data = new(parObjectName);
      data.Parameters["Health"] = character.Health.CurrentHealth;

      #region Weapon

      List<WeaponSaveData> weaponSaveDatas = new();
      foreach (var weapon in character.WeaponController.ListWeapons)
      {
        WeaponSaveData weaponSaveData = new(weapon.Index, weapon.Name, weapon.CurrentAmountAmmo, weapon.CurrentAmountAmmoInMagazine);
        weaponSaveDatas.Add(weaponSaveData);
      }

      data.Parameters["CurrentWeaponIndex"] = character.WeaponController.CurrentWeapon.Index;
      data.Parameters["Weapons"] = weaponSaveDatas;

      #endregion

      return data;
    }

    public void LoadData(ObjectData parObjectData)
    {
      if (parObjectData == null)
        return;

      if (parObjectData.Parameters.TryGetValue("Health", out var health) && health is long || health is int)
      {
        character.Health.SetHealth(Convert.ToInt32(health));
      }

      #region Weapon

      if (parObjectData.Parameters.TryGetValue("CurrentWeaponIndex", out var currentWeaponIndex) && currentWeaponIndex is long || currentWeaponIndex is int)
      {
        if (parObjectData.Parameters.TryGetValue("Weapons", out var weapons) && weapons is JArray parArray)
        {
          foreach (var weapon in character.WeaponController.ListWeapons)
          {
            foreach (var weaponSaveData in parArray.ToObject<List<WeaponSaveData>>())
            {
              int weaponIndex = Convert.ToInt32(currentWeaponIndex);
              weapon.GetWeaponData(weaponSaveData.CurrentAmountAmmo, weaponSaveData.CurrentAmountAmmoInMagazine);

              if (weaponSaveData.Index == weaponIndex)
              {
                character.WeaponController.CurrentWeapon.GetWeaponData(weaponSaveData.CurrentAmountAmmo, weaponSaveData.CurrentAmountAmmoInMagazine);
                break;
              }
            }
          }
        }
      }

      #endregion
    }

    //===================================
  }
}
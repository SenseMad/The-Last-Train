using System;
using System.Collections.Generic;

namespace TLT.Data
{
  [Serializable]
  public class PlayerData
  {
    public int Health { get; set; }

    public List<WeaponSaveData> WeaponSaveDatas { get; set; }

    //===================================

    public PlayerData()
    {
      Health = 0;

      WeaponSaveDatas = new();
    }

    //===================================
  }
}
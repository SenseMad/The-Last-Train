using System;

namespace TLT.Data
{
  [Serializable]
  public class WeaponSaveData
  {
    public int Index;
    public string Name;

    public int CurrentAmountAmmo;
    public int CurrentAmountAmmoInMagazine;

    //===================================

    public WeaponSaveData(int parIndex, string parName, int parCurrentAmountAmmo, int parCurrentAmountAmmoInMagazine)
    {
      Index = parIndex;
      Name = parName;
      CurrentAmountAmmo = parCurrentAmountAmmo;
      CurrentAmountAmmoInMagazine = parCurrentAmountAmmoInMagazine;
    }

    //===================================
  }
}
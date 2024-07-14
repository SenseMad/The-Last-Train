using System.Collections.Generic;
using UnityEngine;

namespace TLT.Weapons
{
  public class WeaponController : MonoBehaviour
  {
    [SerializeField] private Weapon _currentWeapon;
    [SerializeField] private List<Weapon> _listWeapons = new();

    //===================================

    public Weapon CurrentWeapon { get => _currentWeapon; set => _currentWeapon = value; }

    public List<Weapon> ListWeapons { get => _listWeapons; set => _listWeapons = value; }

    //===================================

    public void ChangeWeapon(Weapon parWeapon)
    {
      _currentWeapon = parWeapon;

      /*_currentWeapon.CallEventOnChangeAmmo();
      _currentWeapon.CallEventOnShoot();
      _currentWeapon.CallEventOnAddAmmo();
      _currentWeapon.CallEventOnRecharge();*/
    }

    //===================================
  }
}
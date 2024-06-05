using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLT.Weapons
{
  public class WeaponController : MonoBehaviour
  {
    [SerializeField] private Weapon _currentWeapon;

    //===================================

    public Weapon CurrentWeapon => _currentWeapon;

    //===================================
  }
}
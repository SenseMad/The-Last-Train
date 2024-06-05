using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLT.Weapons
{
  public class WeaponPistol : Weapon
  {
    [Space(10)]
    [SerializeField] private float _delayShoot = 1f;

    //===================================

    protected override void Start()
    {
      lastShotTime = Time.time - _delayShoot;

      base.Start();
    }

    //===================================

    protected override bool DelayShoot()
    {
      if (Time.time - lastShotTime < _delayShoot)
        return false;

      lastShotTime = Time.time;
      return true;
    }

    //===================================
  }
}
using UnityEngine;

namespace TLT.Weapons
{
  public class WeaponController : MonoBehaviour
  {
    [SerializeField] private Weapon _currentWeapon;

    //===================================

    public Weapon CurrentWeapon { get => _currentWeapon; set => _currentWeapon = value; }

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
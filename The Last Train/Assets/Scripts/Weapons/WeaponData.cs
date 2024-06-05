using UnityEngine;

namespace TLT.Weapons
{
  [System.Serializable]
  public sealed class WeaponData
  {
    [SerializeField, Min(0)] private int _damage = 0;

    [Space(10)]
    [SerializeField, Min(0)] private float _distance = 0;

    [Space(10)]
    [SerializeField, Min(0)] private int _maxAmountAmmo = 0;
    [SerializeField, Min(0)] private int _maxAmountAmmoInMagazine = 0;

    [Space(10)]
    [SerializeField] private bool _autoRecharge = false;
    [SerializeField, Min(0)] private float _rechargeTime = 1.0f;

    [Space(10)]
    [SerializeField] private AudioClip _soundGunshot;
    [SerializeField] private AudioClip _soundEmptyAmmo;
    [SerializeField] private AudioClip _soundRecharge;

    //===================================

    public int Damage => _damage;

    public float Distance => _distance;

    public int MaxAmountAmmo => _maxAmountAmmo;
    public int MaxAmountAmmoInMagazine => _maxAmountAmmoInMagazine;

    public bool AutoRecharge => _autoRecharge;
    public float RechargeTime => _rechargeTime;

    public AudioClip SoundGunshot => _soundGunshot;
    public AudioClip SoundEmptyAmmo => _soundEmptyAmmo;
    public AudioClip SoundRecharge => _soundRecharge;

    //===================================
  }
}
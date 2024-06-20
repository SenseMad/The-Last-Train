using System;
using UnityEngine;

using TLT.Interfaces;
using TLT.CharacterManager;
using TLT.Enemy;

namespace TLT.Weapons
{
  public abstract class Weapon : MonoBehaviour
  {
    [Space]
    [SerializeField] private WeaponData _weaponData;

    [Space(10)]
    [SerializeField] private Transform _pointShot;
    [SerializeField] private Transform _hitEffectPrefab;

    [Space(10)]
    [SerializeField] private LineRenderer _lineRenderer;

    [Space(10)]
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private Character _character;

    //-----------------------------------

    private int currentAmountAmmo;
    private int currentAmountAmmoInMagazine;

    protected float lastShotTime;

    private bool isRecharge = false;
    private float currentRechargeTime;

    //===================================

    public WeaponData WeaponData => _weaponData;

    public Transform PointShot => _pointShot;

    //===================================

    public int CurrentAmountAmmo
    {
      get => currentAmountAmmo;
      private set
      {
        currentAmountAmmo = value;
        OnChangeAmmo?.Invoke();
      }
    }

    public int CurrentAmountAmmoInMagazine
    {
      get => currentAmountAmmoInMagazine;
      private set
      {
        currentAmountAmmoInMagazine = value;
        OnChangeAmmo?.Invoke();
      }
    }

    //===================================

    public event Action OnChangeAmmo;

    public event Action OnShoot;

    public event Action OnAddAmmo;

    public event Action OnRecharge;

    //===================================

    private void Awake()
    {
      CurrentAmountAmmo = _weaponData.MaxAmountAmmo;
      CurrentAmountAmmoInMagazine = _weaponData.MaxAmountAmmoInMagazine;
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
      Recharge();
    }

    //===================================

    protected abstract bool DelayShoot();

    protected virtual void Recharge()
    {
      if (!isRecharge)
        return;

      currentRechargeTime += Time.deltaTime;
      if (currentRechargeTime >= _weaponData.RechargeTime)
      {
        RechargeAmmo();

        currentRechargeTime = 0;
        isRecharge = false;
      }
    }

    //===================================

    public void GetWeaponData(int parCurrentAmountAmmo, int parCurrentAmountAmmoInMagazine)
    {
      CurrentAmountAmmo = parCurrentAmountAmmo;
      CurrentAmountAmmoInMagazine = parCurrentAmountAmmoInMagazine;
    }

    public virtual void Shoot()
    {
      if (!CanShoot())
        return;

      CurrentAmountAmmoInMagazine--;
      PlaySound(_weaponData.SoundGunshot);
      OnShoot?.Invoke();

      Ray ray = new(_pointShot.position, _pointShot.right * _character.Direction);

      RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, _weaponData.Distance, ~_layerMask);

      RaycastHit2D validHit = new();
      bool foundValidHit = false;

      for (int i = 0; i < hits.Length; i++)
      {
        if (hits[i].collider.GetComponent<Character>())
          continue;

        if (!hits[i].collider.isTrigger)
        {
          validHit = hits[i];
          foundValidHit = true;
          break;
        }

        if (hits[i].collider.isTrigger && hits[i].collider.GetComponent<EnemyAgent>())
        {
          validHit = hits[i];
          foundValidHit = true;
          break;
        }
      }

      if (!foundValidHit)
      {
        //Debug.DrawLine(ray.origin, ray.origin + ray.direction * _weaponData.Distance, Color.green);
        CreateLineRenderer(ray.origin + ray.direction * _weaponData.Distance);
        return;
      }

      Debug.DrawLine(ray.origin, validHit.point, Color.green);

      if (validHit.collider.TryGetComponent(out IDamageable parIDamageable))
      {
        int damage = GetDamageFromDistance(validHit.distance, _weaponData.Distance);

        parIDamageable.ApplyDamage(damage);

        Debug.Log($"{validHit.collider.name}: {damage}");
      }

      CreateLineRenderer(validHit.point);
      OnHitEffect(validHit.point);
    }

    //===================================

    public void PerformRecharge()
    {
      if (isRecharge)
        return;

      if (currentAmountAmmo == 0)
        return;

      if (currentAmountAmmoInMagazine < _weaponData.MaxAmountAmmoInMagazine)
      {
        isRecharge = true;
        PlaySound(_weaponData.SoundRecharge);
        return;
      }

      if (currentAmountAmmoInMagazine >= _weaponData.MaxAmountAmmoInMagazine)
        return;
    }

    public void AddAmmo(int parValue)
    {
      if (parValue < 0)
        return;

      if (currentAmountAmmo + parValue > _weaponData.MaxAmountAmmo)
      {
        CurrentAmountAmmo = _weaponData.MaxAmountAmmo;
        OnAddAmmo?.Invoke();

        return;
      }

      CurrentAmountAmmo += parValue;
      OnAddAmmo?.Invoke();
    }

    //===================================

    private bool CanShoot()
    {
      if (isRecharge)
        return false;

      if (currentAmountAmmoInMagazine == 0)
      {
        if (currentAmountAmmo == 0)
        {
          PlaySound(_weaponData.SoundEmptyAmmo);
          return false;
        }

        if (_weaponData.AutoRecharge)
        {
          PerformRecharge();
          return false;
        }

        PlaySound(_weaponData.SoundEmptyAmmo);
        return false;
      }

      if (!DelayShoot())
        return false;

      lastShotTime = Time.time;
      return true;
    }

    private void PlaySound(AudioClip parAudioClip)
    {
      if (_audioSource == null && parAudioClip == null)
        return;

      var sound = Instantiate(_audioSource, transform.position, Quaternion.identity);

      sound.Stop();
      sound.clip = parAudioClip;
      sound.Play();

      Destroy(sound.gameObject, parAudioClip.length);
    }

    private int GetDamageFromDistance(float parCurrentDistance, float parMaxShootDistance)
    {
      float percent = parCurrentDistance / parMaxShootDistance * 100;

      if (percent <= 25)
        return _weaponData.Damage;
      else if (percent <= 50)
        return (int)Math.Ceiling(_weaponData.Damage / 1.5f);
      else if (percent <= 75)
        return (int)Math.Ceiling(_weaponData.Damage / 2f);
      else
        return (int)Math.Ceiling(_weaponData.Damage / 3f);
    }

    private void OnHitEffect(Vector2 parPointHit)
    {
      if (_hitEffectPrefab == null)
        return;

      Transform hitEffect = Instantiate(_hitEffectPrefab, parPointHit, Quaternion.identity);

      Destroy(hitEffect.gameObject, 0.5f);
    }

    private void CreateLineRenderer(Vector2 parEndPosition)
    {
      var positionPointShot = _pointShot.position;
      positionPointShot.z = 0;

      _lineRenderer.SetPosition(0, positionPointShot);
      _lineRenderer.SetPosition(1, parEndPosition);

      LineRenderer lineRenderer = Instantiate(_lineRenderer, _pointShot.position, Quaternion.identity);

      Destroy(lineRenderer.gameObject, 0.5f);
    }

    private void RechargeAmmo()
    {
      int amountAmmoBefore = currentAmountAmmo;
      int amountAmmoInMagazineBefore = currentAmountAmmoInMagazine;

      int needAmmo = _weaponData.MaxAmountAmmoInMagazine - amountAmmoInMagazineBefore;

      if (amountAmmoBefore - needAmmo < 0)
        needAmmo = amountAmmoBefore;

      CurrentAmountAmmo -= needAmmo;
      CurrentAmountAmmoInMagazine += needAmmo;

      OnRecharge?.Invoke();
    }

    //===================================
  }
}
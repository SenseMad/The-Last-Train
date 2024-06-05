using UnityEngine;
using TMPro;
using Zenject;

namespace TLT.UI
{
  public class UIGame : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _textHealth;

    [SerializeField] private TextMeshProUGUI _textAmmo;

    //-----------------------------------

    private LevelManager levelManager;

    //===================================

    [Inject]
    private void Construct(LevelManager parLevelManager)
    {
      levelManager = parLevelManager;
    }

    //===================================

    private void Start()
    {
      UpdateTextHealth();

      UpdateTextAmmo();
    }

    private void OnEnable()
    {
      levelManager.Character.Health.OnChangeHealth += UpdateTextHealth;

      levelManager.Character.WeaponController.CurrentWeapon.OnChangeAmmo += UpdateTextAmmo;

      levelManager.OnChangeCharacter += LevelManager_OnChangeCharacter;
    }

    private void OnDisable()
    {
      levelManager.Character.Health.OnChangeHealth -= UpdateTextHealth;

      levelManager.Character.WeaponController.CurrentWeapon.OnChangeAmmo -= UpdateTextAmmo;

      levelManager.OnChangeCharacter -= LevelManager_OnChangeCharacter;
    }

    //===================================

    private void LevelManager_OnChangeCharacter()
    {
      UpdateTextHealth();

      UpdateTextAmmo();

      levelManager.Character.Health.OnChangeHealth += UpdateTextHealth;

      levelManager.Character.WeaponController.CurrentWeapon.OnChangeAmmo += UpdateTextAmmo;
    }

    //===================================

    private void UpdateTextHealth()
    {
      var health = levelManager.Character.Health;

      _textHealth.text = $"{health.CurrentHealth}/{health.MaxHealth}";
    }

    private void UpdateTextAmmo()
    {
      var weapon = levelManager.Character.WeaponController.CurrentWeapon;

      _textAmmo.text = $"{weapon.CurrentAmountAmmoInMagazine}/{weapon.CurrentAmountAmmo}";
    }

    //===================================
  }
}
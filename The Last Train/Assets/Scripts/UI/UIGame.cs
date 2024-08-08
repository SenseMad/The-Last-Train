using UnityEngine;
using TMPro;
using Zenject;
using UnityEngine.UI;

namespace TLT.UI
{
  public class UIGame : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _textHealth;

    [SerializeField] private TextMeshProUGUI _textAmmo;

    [Space]
    [SerializeField] private Image _dashImage;

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

    public void UpdateDashImage(bool parValue)
    {
      _dashImage.gameObject.SetActive(parValue);
    }

    public void UpdateDashImage(Sprite parSpriteDash)
    {
      if (_dashImage.gameObject.activeSelf)
        _dashImage.sprite = parSpriteDash;
    }

    public void UpdateDashImage(bool parValue, Sprite parSpriteDash)
    {
      _dashImage.gameObject.SetActive(parValue);

      if (_dashImage.gameObject.activeSelf)
        _dashImage.sprite = parSpriteDash;
    }

    //===================================

    private void LevelManager_OnChangeCharacter()
    {
      UpdateTextHealth();

      UpdateTextAmmo();

      levelManager.Character.Health.OnChangeHealth += UpdateTextHealth;

      levelManager.Character.WeaponController.CurrentWeapon.OnChangeAmmo += UpdateTextAmmo;

      //levelManager.Character.WeaponController.CurrentWeapon.OnChangeAmmo -= UpdateTextAmmo;
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
using UnityEngine;

using TLT.CharacterManager;
using TLT.Interfaces;

namespace TLT.Items
{
  public class AmmoPickup : ObjectInteraction, IPickupable
  {
    [SerializeField] private int _amountAmmo;

    //===================================

    private void OnEnable()
    {
      OnInteract += AmmoPickup_OnInteract;
    }

    private void OnDisable()
    {
      OnInteract -= AmmoPickup_OnInteract;
    }

    //===================================

    public void Pickup(Character parCharacter)
    {
      parCharacter.WeaponController.CurrentWeapon.AddAmmo(_amountAmmo);
    }

    //===================================

    private void AmmoPickup_OnInteract()
    {
      Debug.Log($"Подобрали {_amountAmmo} патронов!");

      Destroy(gameObject);
    }

    //===================================
  }
}
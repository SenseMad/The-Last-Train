using UnityEngine;

using TLT.CharacterManager;
using TLT.Interfaces;

namespace TLT.Items
{
  public class HealthPickup : ObjectInteraction, IPickupable
  {
    [SerializeField] private int _amountHealth;

    //===================================

    private void OnEnable()
    {
      OnInteract += HealthPickup_OnInteract;
    }

    private void OnDisable()
    {
      OnInteract -= HealthPickup_OnInteract;
    }

    //===================================

    public void Pickup(Character parCharacter)
    {
      parCharacter.Health.AddHealth(_amountHealth);
    }

    //===================================

    private void HealthPickup_OnInteract()
    {
      Debug.Log($"Подобрали {_amountHealth} здоровья!");

      Destroy(gameObject);
    }

    //===================================
  }
}
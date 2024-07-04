using System;
using UnityEngine;

namespace TLT.InteractionObjects
{
  public class DoorManager : ObjectInteraction
  {
    [SerializeField] private GameObject _openDoor;

    //===================================

    public event Action OnDoorOpen;

    //===================================

    private void OnEnable()
    {
      OnInteract += OpenDoor;
    }

    private void OnDisable()
    {
      OnInteract -= OpenDoor;
    }

    //===================================

    public void OpenDoor()
    {
      OnDoorOpen?.Invoke();

      if (_openDoor != null)
        _openDoor.SetActive(true);

      gameObject.SetActive(false);
    }

    //===================================
  }
}
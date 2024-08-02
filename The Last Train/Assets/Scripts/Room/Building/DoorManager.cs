using System;
using UnityEngine;
using Zenject;

using TLT.Sound;

namespace TLT.InteractionObjects
{
  public class DoorManager : ObjectInteraction
  {
    [SerializeField] private GameObject _openDoor;

    [Space]
    [SerializeField] private AudioClip _soundOpenDoor;
    [SerializeField, Range(0, 1)] private float _soundVolume = 0.5f;

    //-----------------------------------

    private SoundManager soundManager;

    //===================================

    public event Action OnDoorOpen;

    //===================================

    [Inject]
    private void Construct(SoundManager parSoundManager)
    {
      soundManager = parSoundManager;
    }

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

      soundManager.PlaySound(_soundOpenDoor, transform.position, _soundVolume);

      gameObject.SetActive(false);
    }

    //===================================
  }
}
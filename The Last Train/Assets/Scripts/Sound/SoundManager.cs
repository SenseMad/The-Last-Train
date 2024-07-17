using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLT.Sound
{
  public class SoundManager : MonoBehaviour
  {
    public AudioSource PlaySound(AudioClip parClip, Vector3 parLocation, float parVolume)
    {
      GameObject tempAudio = new("SoundEffects");
      tempAudio.transform.position = parLocation;

      AudioSource audioSource = tempAudio.AddComponent<AudioSource>();

      audioSource.clip = parClip;
      audioSource.volume = parVolume;
      audioSource.Play();

      Destroy(tempAudio, parClip.length);

      return audioSource;
    }

    //===================================
  }
}
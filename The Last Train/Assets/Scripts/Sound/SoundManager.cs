using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{


  //===================================



  //===================================



  //===================================

  public AudioSource PlaySound(AudioClip parSFX, Vector3 parLocation, float parVolume)
  {
    GameObject tempAudio = new("SoundEffects");
    tempAudio.transform.position = parLocation;

    AudioSource audioSource = tempAudio.AddComponent<AudioSource>();

    audioSource.clip = parSFX;
    audioSource.volume = parVolume;
    audioSource.Play();

    Destroy(tempAudio, parSFX.length);

    return audioSource;
  }

  public void CreateSound()
  {
    
  }

  //===================================
}
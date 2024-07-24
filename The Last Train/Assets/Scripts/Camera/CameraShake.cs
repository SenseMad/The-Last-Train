using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
  private CinemachineImpulseSource cinemachineImpulseSource;

  private CinemachineImpulseDefinition cinemachineImpulseDefinition;

  //===================================

  private void Awake()
  {
    cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

    cinemachineImpulseDefinition = cinemachineImpulseSource.ImpulseDefinition;
  }

  //===================================

  public void Shake(float duration, float power)
  {
    cinemachineImpulseDefinition.ImpulseDuration = duration;
    cinemachineImpulseSource.GenerateImpulseWithForce(power);
  }

  //===================================
}
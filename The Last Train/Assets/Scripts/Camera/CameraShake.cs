using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
  private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

  private Coroutine coroutine;

  //===================================

  private void Awake()
  {
    cinemachineBasicMultiChannelPerlin = GetComponent<CinemachineBasicMultiChannelPerlin>();
  }

  private void Start()
  {
    cinemachineBasicMultiChannelPerlin.AmplitudeGain = 0;
    cinemachineBasicMultiChannelPerlin.FrequencyGain = 0;
  }

  //===================================

  public void Shake(float duration, float power)
  {
    if (coroutine != null)
      StopCoroutine(coroutine);

    coroutine = StartCoroutine(StartShake(duration, power));
  }

  private IEnumerator StartShake(float duration, float power)
  {
    cinemachineBasicMultiChannelPerlin.AmplitudeGain = power;
    cinemachineBasicMultiChannelPerlin.FrequencyGain = power;

    yield return new WaitForSeconds(duration);

    cinemachineBasicMultiChannelPerlin.AmplitudeGain = 0;
    cinemachineBasicMultiChannelPerlin.FrequencyGain = 0;
    coroutine = null;
  }

  //===================================
}
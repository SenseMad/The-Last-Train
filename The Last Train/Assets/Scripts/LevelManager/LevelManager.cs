using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using System;
using Zenject;

using TLT.CharacterManager;

public class LevelManager : MonoBehaviour
{
  [SerializeField, Min(0)] private float _timeRestart = 1.0f;

  //===================================

  public Character Character { get; set; }

  //===================================

  public event Action OnChangeCharacter;

  //===================================

  [Inject]
  private void Construct(Character parCharacter)
  {
    Character = parCharacter;
  }

  //===================================

  public void ChangeCharacter()
  {
    //_character = parNewCharacter;

    OnChangeCharacter?.Invoke();
  }

  //===================================

  public void StartRestartScene()
  {
    StartCoroutine(RestartScene());
  }

  private IEnumerator RestartScene()
  {
    Scene currentScene = SceneManager.GetActiveScene();

    yield return new WaitForSeconds(_timeRestart);

    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentScene.name);

    while (!asyncLoad.isDone)
      yield return null;
  }

  //===================================
}
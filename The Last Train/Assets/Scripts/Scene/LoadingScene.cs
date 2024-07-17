using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
  public bool IsSceneTransition { get; private set; }

  //===================================

  public void LoadScene(string parNameScene)
  {
    StartCoroutine(LoadSceneAsync(parNameScene));
  }

  //===================================

  private IEnumerator LoadSceneAsync(string parNameScene)
  {
    IsSceneTransition = true;

    AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync("LoadingScene");

    while (!loadSceneAsync.isDone)
      yield return null;

    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(parNameScene);
    asyncOperation.allowSceneActivation = false;

    while(asyncOperation.progress < 0.9f)
      yield return null;

    asyncOperation.allowSceneActivation = true;
  }

  //===================================
}
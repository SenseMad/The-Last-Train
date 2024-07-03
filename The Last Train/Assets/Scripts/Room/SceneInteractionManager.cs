using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TLT.InteractionObjects
{
  public class SceneInteractionManager : ObjectInteraction
  {
    [SerializeField] private string _nameScene;

    //===================================

    private void OnEnable()
    {
      OnInteract += SceneInteractionManager_OnInteract;
    }

    private void OnDisable()
    {
      OnInteract -= SceneInteractionManager_OnInteract;
    }

    //===================================

    private void SceneInteractionManager_OnInteract()
    {
      StartCoroutine(StartScene());
    }

    private IEnumerator StartScene()
    {
      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_nameScene);

      while (!asyncLoad.isDone)
        yield return null;
    }

    //===================================
  }
}
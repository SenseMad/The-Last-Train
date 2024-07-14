using System.Collections;
using TLT.Save;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace TLT.InteractionObjects
{
  public class SceneInteractionManager : ObjectInteraction
  {
    [SerializeField] private string _nameScene;

    //-----------------------------------

    private GameManager gameManager;

    //===================================

    [Inject]
    private void Construct(GameManager parGameManager)
    {
      gameManager = parGameManager;
    }

    //===================================

    protected override void Awake()
    {
      base.Awake();
    }

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
      gameManager.SaveManager.SaveGame();

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
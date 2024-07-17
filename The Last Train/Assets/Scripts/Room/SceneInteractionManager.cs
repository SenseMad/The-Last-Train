using TLT.Save;
using UnityEngine;
using Zenject;

namespace TLT.InteractionObjects
{
  public class SceneInteractionManager : ObjectInteraction
  {
    [SerializeField] private NamesScenes _namesScenes;

    //-----------------------------------

    private GameManager gameManager;
    private LoadingScene loadingScene;

    //===================================

    [Inject]
    private void Construct(GameManager parGameManager, LoadingScene parLoadingScene)
    {
      gameManager = parGameManager;
      loadingScene = parLoadingScene;
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

      loadingScene.LoadScene($"{_namesScenes}");
    }

    //===================================
  }
}
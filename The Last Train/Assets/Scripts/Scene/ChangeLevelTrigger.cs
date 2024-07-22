using UnityEngine;
using Zenject;

using TLT.Save;
using TLT.Vehicles.Bike;

public class ChangeLevelTrigger : MonoBehaviour
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

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (!collision.TryGetComponent(out BikeController parBikeController))
      return;

    if (!parBikeController.IsInCar)
      return;

    gameManager.SaveManager.DeleteSaveGameLevels();

    gameManager.SaveManager.SavePlayerData();

    loadingScene.LoadScene($"{_namesScenes}");
  }

  //===================================
}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

using TLT.Save;
using TLT.Vehicles.Bike;

public class ChangeLevelTrigger : MonoBehaviour
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

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (!collision.TryGetComponent(out BikeController parBikeController))
      return;

    if (!parBikeController.IsInCar)
      return;

    gameManager.SaveManager.SavePlayerData();
    StartCoroutine(StartScene());
  }

  //===================================

  private IEnumerator StartScene()
  {
    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_nameScene);

    while (!asyncLoad.isDone)
      yield return null;
  }

  //===================================
}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

using TLT.CharacterManager;

namespace TLT.Vehicles.Bike
{
  public class DeathCharacterBikeCoup : MonoBehaviour
  {
    [SerializeField] private BikeBody _bikeBody;

    [SerializeField] private LayerMask _ignoreLayer;

    [SerializeField] private CapsuleCollider2D _collider2D;

    [Space] 
    [SerializeField, Min(0)] private float _timeRestart = 1.0f;

    //-----------------------------------

    private Character character;

    //===================================

    [Inject]
    private void Construct(Character parCharacter)
    {
      character = parCharacter;
    }

    //===================================

    private void Update()
    {
      if (!_bikeBody.BikeController.IsInCar)
        return;

      if (_collider2D == null)
        return;

      Collider2D[] hits = Physics2D.OverlapCapsuleAll(_collider2D.bounds.center, _collider2D.size, _collider2D.direction, 0f, ~_ignoreLayer);

      foreach (var hit in hits)
      {
        if (hit != _collider2D)
          StartCoroutine(RestartScene());
      }
    }

    //===================================

    private IEnumerator RestartScene()
    {
      Scene currentScene = SceneManager.GetActiveScene();

      character.Health.InstantlyKill();

      _bikeBody.BikeController.IsInCar = false;
      _bikeBody.ObjectBody.SetActive(true);

      yield return new WaitForSeconds(_timeRestart);

      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentScene.name);

      while (!asyncLoad.isDone)
        yield return null;
    }

    //===================================
  }
}
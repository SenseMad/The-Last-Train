using System.Collections;
using UnityEngine;

namespace TLT.Bike
{
  public class BikeBootstrap : MonoBehaviour
  {
    [SerializeField] private MonoBehaviour[] _scriptsToExecute;

    //-----------------------------------

    private IBikeBootstrap[] bikeBootstraps;

    //===================================

    private void Awake()
    {
      bikeBootstraps = new IBikeBootstrap[_scriptsToExecute.Length];

      for (int i = 0; i < _scriptsToExecute.Length; i++)
      {
        _scriptsToExecute[i].enabled = false;
        bikeBootstraps[i] = (IBikeBootstrap)_scriptsToExecute[i];
      }

      foreach (var bikeBootstrap in bikeBootstraps)
      {
        bikeBootstrap.CustomAwake();
      }

      for (int i = 0; i < _scriptsToExecute.Length; i++)
      {
        _scriptsToExecute[i].enabled = true;
      }
    }

    private IEnumerator Start()
    {
      foreach (var bikeBootstrap in bikeBootstraps)
      {
        bikeBootstrap.CustomStart();
        yield return null;
      }
    }

    //===================================
  }
}
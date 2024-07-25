using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

using TLT.Input;
using TLT.Bike.Bike;
using Unity.VisualScripting;

namespace TLT.Bike
{
  public class BikeEngine : MonoBehaviour, IBikeBootstrap
  {
    [SerializeField, Min(0)] private int _numberClicksLaunch;
    [SerializeField, Min(0)] private float _engineStartTime;

    [Space]
    [SerializeField, Min(0)] private float _engineShutdownTime;
    //[SerializeField, Min(1)] private float _engineRunningTimeWithoutMovement;

    //-----------------------------------

    private InputHandler inputHandler;

    private BikeManager bikeManager;
    private BikeController bikeController;

    private int tempNumberClicksLaunch = 0;
    private float tempEngineStartTime = 0;

    //===================================

    public bool IsEngineRunning {  get; private set; }

    //===================================

    public event Action OnStartEngine;

    public event Action OnStopEngine;

    //===================================

    [Inject]
    private void Construct(InputHandler parInputHandler)
    {
      inputHandler = parInputHandler;
    }

    //===================================

    public void CustomAwake()
    {
      bikeManager = GetComponent<BikeManager>();
      bikeController = GetComponent<BikeController>();
    }

    public void CustomStart() { }

    /*private void Awake()
    {
      bikeManager = GetComponent<BikeManager>();
      bikeController = GetComponent<BikeController>();
    }*/

    private void Update()
    {
      if (!IsEngineRunning && tempNumberClicksLaunch > 0)
      {
        tempEngineStartTime += Time.deltaTime;

        if (tempEngineStartTime >= _engineStartTime)
        {
          tempNumberClicksLaunch = 0;
          tempEngineStartTime = 0;
        }
      }
    }

    private void OnEnable()
    {
      OnStartEngine += BikeEngine_OnStartEngine;
      OnStopEngine += BikeEngine_OnStopEngine;
    }

    private void OnDisable()
    {
      OnStartEngine -= BikeEngine_OnStartEngine;
      OnStopEngine -= BikeEngine_OnStopEngine;
    }

    //===================================

    public void StartEngine()
    {
      tempNumberClicksLaunch++;

      if (tempNumberClicksLaunch < _numberClicksLaunch)
        return;

      IsEngineRunning = true;

      OnStartEngine?.Invoke();
    }

    public void StopEngine()
    {
      IsEngineRunning = false;

      OnStopEngine?.Invoke();
    }

    //===================================

    private void BikeEngine_OnStartEngine()
    {
      tempNumberClicksLaunch = 0;
      tempEngineStartTime = 0;
    }

    private void BikeEngine_OnStopEngine()
    {
      tempNumberClicksLaunch = 0;
      tempEngineStartTime = 0;
    }

    //===================================
  }
}
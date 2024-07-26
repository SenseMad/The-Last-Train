using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TLT.Bike.Bike;

namespace TLT.Bike
{
  public class BikeEngine : MonoBehaviour, IBikeBootstrap
  {
    [SerializeField, Min(0)] private int _numberClicksLaunch;
    [SerializeField, Min(0)] private float _engineStartTime;

    [Space]
    [SerializeField, Min(0)] private float _engineShutdownTime;

    [Space]
    [SerializeField] private Animator _animatorQButton;
    //[SerializeField, Min(1)] private float _engineRunningTimeWithoutMovement;

    //-----------------------------------

    private BikeBody bikeBody;
    private BikeController bikeController;

    private int tempNumberClicksLaunch = 0;
    private float tempEngineStartTime = 0;

    private float tempEngineShutdownTime = 0;

    //===================================

    public bool IsEngineRunning {  get; private set; }

    //===================================

    public event Action OnStartEngine;

    public event Action OnStopEngine;

    //===================================

    public void CustomAwake()
    {
      bikeBody = GetComponent<BikeBody>();
      bikeController = GetComponent<BikeController>();
    }

    public void CustomStart() { }

    private void Update()
    {
      if (bikeController.IsInCar)
      {
        // ������� ���������
        if (!IsEngineRunning && tempNumberClicksLaunch > 0)
        {
          tempEngineStartTime += Time.deltaTime;

          if (tempEngineStartTime >= _engineStartTime)
          {
            BikeEngine_OnStartEngine();
          }
        }
      }

      // ���������� ���������
      if (IsEngineRunning && bikeController.Throttle == 0 && !bikeController.IsMoving)
      {
        tempEngineShutdownTime += Time.deltaTime;
        Debug.Log($"�������� ��������� �����: {_engineShutdownTime - tempEngineShutdownTime} ���.");
        if (tempEngineShutdownTime >= _engineShutdownTime)
        {
          StopEngine();
        }
      }
      else
      {
        BikeEngine_OnStopEngine();
      }
    }

    private void OnEnable()
    {
      bikeBody.OnGetInCar += BikeBody_OnGetInCar;
      bikeBody.OnGetOutCar += BikeBody_OnGetOutCar;

      OnStartEngine += BikeEngine_OnStartEngine;
      OnStopEngine += BikeEngine_OnStopEngine;
    }

    private void OnDisable()
    {
      bikeBody.OnGetInCar -= BikeBody_OnGetInCar;
      bikeBody.OnGetOutCar -= BikeBody_OnGetOutCar;

      OnStartEngine -= BikeEngine_OnStartEngine;
      OnStopEngine -= BikeEngine_OnStopEngine;
    }

    //===================================

    public void StartEngine()
    {
      tempNumberClicksLaunch++;
      _animatorQButton.SetTrigger("IsClick");

      if (tempNumberClicksLaunch < _numberClicksLaunch)
        return;

      IsEngineRunning = true;

      _animatorQButton.gameObject.SetActive(false);

      OnStartEngine?.Invoke();
    }

    public void StopEngine()
    {
      IsEngineRunning = false;

      if (bikeController.IsInCar)
        _animatorQButton.gameObject.SetActive(true);

      OnStopEngine?.Invoke();
    }

    //===================================

    private void BikeBody_OnGetInCar()
    {
      if (IsEngineRunning)
        return;

      _animatorQButton.gameObject.SetActive(true);
    }

    private void BikeBody_OnGetOutCar()
    {
      _animatorQButton.gameObject.SetActive(false);
    }

    private void BikeEngine_OnStartEngine()
    {
      tempNumberClicksLaunch = 0;
      tempEngineStartTime = 0;
    }

    private void BikeEngine_OnStopEngine()
    {
      tempEngineShutdownTime = 0;
    }

    //===================================
  }
}
using System;
using UnityEngine;
using Zenject;

using TLT.Bike.Bike;
using TLT.Sound;

namespace TLT.Bike
{
  public class BikeEngine : MonoBehaviour, IBikeBootstrap
  {
    [SerializeField, Min(0)] private int _numberClicksLaunch;
    [SerializeField, Min(0)] private float _engineStartTime;

    [Space]
    [SerializeField, Min(0)] private float _engineShutdownTime;

    [Space]
    [SerializeField] private AudioClip _audioStartBike;

    [Space]
    [SerializeField] private Animator _animatorQButton;
    [SerializeField] private Animator _animatorSuccess;

    //-----------------------------------

    private BikeBody bikeBody;
    private BikeController bikeController;

    private SoundManager soundManager;

    private int tempNumberClicksLaunch = 0;
    private float tempEngineStartTime = 0;

    private float tempEngineShutdownTime = 0;

    private bool isEngineTryingStart = false;

    //===================================

    public bool IsEngineRunning {  get; private set; }

    //===================================

    [Inject]
    private void Construct(SoundManager parSoundManager)
    {
      soundManager = parSoundManager;
    }

    //===================================

    public event Action OnStartEngine;

    public event Action OnStopEngine;

    //===================================

    public void CustomAwake()
    {
      bikeBody = GetComponent<BikeBody>();
      bikeController = GetComponent<BikeController>();
    }

    public void CustomStart()
    {
      if (!bikeController.IsInCar)
        _animatorQButton.gameObject.SetActive(false);
    }

    private void Update()
    {
      if (bikeController.IsInCar)
      {
        // Завести двигатель
        if (!IsEngineRunning && tempNumberClicksLaunch > 0)
        {
          tempEngineStartTime += Time.deltaTime;

          if (tempEngineStartTime >= _engineStartTime)
          {
            BikeEngine_OnStartEngine();
          }
        }
      }

      // Остановить двигатель
      if (IsEngineRunning && bikeController.Throttle == 0 && !bikeController.IsMoving)
      {
        tempEngineShutdownTime += Time.deltaTime;
        Debug.Log($"Мотоцикл заглохнет через: {_engineShutdownTime - tempEngineShutdownTime} сек.");
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
      if (isEngineTryingStart)
        return;

      bikeController.Animator.SetTrigger("IsStart");

      _animatorQButton.SetTrigger("IsClick");

      isEngineTryingStart = true;
    }

    public void StopEngine()
    {
      IsEngineRunning = false;

      if (bikeController.BalanceMiniGameManager.IsGameRunning)
        bikeController.BalanceMiniGameManager.EndGame();

      if (bikeController.IsInCar)
        _animatorQButton.gameObject.SetActive(true);

      OnStopEngine?.Invoke();
    }

    //===================================

    private void SoundStartBike()
    {
      soundManager.PlaySound(_audioStartBike, transform.position, 0.5f);
    }

    private void IncreaseClick()
    {
      tempNumberClicksLaunch++;

      isEngineTryingStart = false;

      if (tempNumberClicksLaunch < _numberClicksLaunch)
        return;

      IsEngineRunning = true;

      _animatorQButton.gameObject.SetActive(false);

      _animatorSuccess.gameObject.SetActive(true);

      OnStartEngine?.Invoke();
    }

    private void BikeBody_OnGetInCar()
    {
      if (IsEngineRunning)
        return;

      _animatorQButton.gameObject.SetActive(true);

      _animatorSuccess.gameObject.SetActive(false);
    }

    private void BikeBody_OnGetOutCar()
    {
      _animatorQButton.gameObject.SetActive(false);

      _animatorSuccess.gameObject.SetActive(false);
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
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using Random = UnityEngine.Random;

namespace TLT.Bike.Bike
{
  public class BalanceMiniGameManager : MonoBehaviour
  {
    [SerializeField, Min(0)] private float _speedTriangle;

    [Space]
    [Header("SLIDER")]
    [SerializeField, Range(0, 1), Tooltip("How much more does it get if we hit")] private float _biggerHitSlider;
    [SerializeField, Range(0, 1), Tooltip("How much smaller does it get if we miss")] private float _smallerMissSlider;
    [SerializeField, Min(0), Tooltip("The speed of reducing the slider")] private float _speedReducingSlider = 0.9f;
    [SerializeField, Min(0)] private float _speedGainSlider = 1.1f;
    [SerializeField] private Vector2 _sliderSize = new(0.5f, 0.9f);
    [SerializeField] private Vector2 _sliderSpeed = new(0.5f, 0.9f);

    [Space]
    [SerializeField] private Vector2 _coefficientsA = new(1.0f, 5.0f);
    [SerializeField] private Vector2 _coefficientsB = new(-3.0f, 3.0f);

    [Space]
    [Header("UI")]
    [SerializeField] private GameObject _miniGame;
    [SerializeField] private Scrollbar _scrollbarArrow;
    [SerializeField] private Scrollbar _scrollbarSlider;

    //-----------------------------------

    private float currentSpeedTriangle;

    private float currentSizeSlider;
    private float initialSizeSlider;
    private float currentSpeedSlider;
    private float initialSpeedSlider;

    // Coefficients
    private Vector2 currentCoefficientsSlider;
    private Vector2 currentCoefficientsTriangle;

    private float timeSpeedTriangle;
    private float timeSpeedSlider;
    private float elapsedTimeSlider;

    private BikeController bikeController;

    //===================================

    public bool IsGameRunning { get; private set; }

    //===================================

    public event Action OnWinGame;
    public event Action OnEndGame;

    //===================================

    private void Awake()
    {
      bikeController = GetComponentInParent<BikeController>();
    }

    /*private void Start()
    {
      Initialize(0.5f);
    }*/

    private void OnEnable()
    {
      bikeController.InputHandler.AI_Player.Vehicle.Engine.performed += IsInZone;
    }

    private void OnDisable()
    {
      bikeController.InputHandler.AI_Player.Vehicle.Engine.performed -= IsInZone;
    }

    private void OnValidate()
    {
      if (_sliderSize.x > _sliderSize.y)
        _sliderSize.x = _sliderSize.y;
    }

    private void Update()
    {
      if (!IsGameRunning)
        return;

      SpeedTriangle();

      ChangeSpeedSlider();

      if (_scrollbarSlider.size <= 0.05f)
      {        
        EndGame();
        return;
      }

      if (_scrollbarSlider.size >= 0.9f)
      {
        WinGame();
        return;
      }
    }

    //===================================

    public void Initialize(float parSlider)
    {
      if (IsGameRunning)
        return;

      _scrollbarArrow.value = 0.5f;
      _scrollbarSlider.value = Random.Range(0.0f, 1.0f);

      currentSpeedSlider = parSlider;
      initialSpeedSlider = parSlider;

      currentSizeSlider = parSlider;
      initialSizeSlider = parSlider * _speedReducingSlider;

      StartGame();
    }

    //===================================

    private void StartGame()
    {
      if (IsGameRunning)
        return;

      _scrollbarSlider.size = currentSizeSlider;

      RandomCoefficients();

      bikeController.Animator.SetBool("IsMotoBalance", true);

      _miniGame.SetActive(true);

      IsGameRunning = true;
    }

    private void WinGame()
    {
      IsGameRunning = false;

      bikeController.Animator.SetBool("IsMotoBalance", false);

      _miniGame.SetActive(false);

      OnWinGame?.Invoke();
    }

    private void EndGame()
    {
      IsGameRunning = false;

      bikeController.Animator.SetBool("IsMotoBalance", false);

      _miniGame.SetActive(false);

      OnEndGame?.Invoke();
    }

    private void IsInZone(InputAction.CallbackContext obj)
    {
      if (!IsGameRunning)
        return;

      float scrollbarArrowValue = _scrollbarArrow.value;
      float scrollbarArrowStart = scrollbarArrowValue + 0.03f;
      float scrollbarArrowEnd = scrollbarArrowValue - 0.03f;

      scrollbarArrowStart = Mathf.Clamp(scrollbarArrowStart, 0, 1);
      scrollbarArrowEnd = Mathf.Clamp(scrollbarArrowEnd, 0, 1);

      bool isInRange = scrollbarArrowStart >= _scrollbarSlider.handleRect.anchorMin.x && scrollbarArrowEnd <= _scrollbarSlider.handleRect.anchorMax.x;

      if (isInRange)
      {
        _scrollbarSlider.size += (1 - _scrollbarSlider.size) * _biggerHitSlider;
      }
      else
      {
        _scrollbarSlider.size -= _scrollbarSlider.size * _smallerMissSlider;
      }

      currentSizeSlider = _scrollbarSlider.size;
      initialSizeSlider = _scrollbarSlider.size * _speedReducingSlider;

      if (_scrollbarSlider.size >= 0.9f)
      {
        _scrollbarSlider.size = 1;
        WinGame();
      }
    }

    private void SpeedTriangle()
    {
      timeSpeedTriangle += _speedTriangle * Time.deltaTime;

      currentSpeedTriangle = X(timeSpeedTriangle, currentCoefficientsTriangle.x, currentCoefficientsTriangle.y);

      _scrollbarArrow.value = Mathf.Clamp01(currentSpeedTriangle);
    }

    private void ChangeSpeedSlider()
    {
      timeSpeedSlider += currentSpeedSlider * Time.deltaTime;
      elapsedTimeSlider += Time.deltaTime;

      currentSpeedSlider = initialSpeedSlider * Mathf.Pow(_speedGainSlider, elapsedTimeSlider);

      if (elapsedTimeSlider >= 1)
      {
        currentSizeSlider = _scrollbarSlider.size;
        initialSizeSlider = _scrollbarSlider.size * _speedReducingSlider;
        elapsedTimeSlider = 0;
      }

      _scrollbarSlider.value = Mathf.Clamp01(X(timeSpeedSlider, currentCoefficientsSlider.x, currentCoefficientsSlider.y));
      _scrollbarSlider.size = Mathf.Lerp(currentSizeSlider, initialSizeSlider, elapsedTimeSlider / 1);
    }

    private void RandomCoefficients()
    {
      float aSlider = Random.Range(_coefficientsA.x, _coefficientsA.y);
      float bSlider = Random.Range(_coefficientsB.x, _coefficientsB.y);

      currentCoefficientsSlider = new(aSlider, bSlider);

      float aTriangle = Random.Range(_coefficientsA.x, _coefficientsA.y);
      float bTriangle = Random.Range(_coefficientsB.x, _coefficientsB.y);

      while (aTriangle == aSlider)
      {
        aTriangle = Random.Range(_coefficientsA.x, _coefficientsA.y);
      }

      while (bTriangle == bSlider)
      {
        bTriangle = Random.Range(_coefficientsB.x, _coefficientsB.y);
      }

      currentCoefficientsTriangle = new(aTriangle, bTriangle);
    }

    /// <summary>
    /// The formula for the movement of the slider and triangle
    /// </summary>
    private float X(float parSpeed, float parA, float parB)
    {
      return Mathf.Sin(parSpeed * parB + Mathf.Sin(parSpeed * parA)) * 0.5f + 0.5f;
    }

    //===================================
  }
}
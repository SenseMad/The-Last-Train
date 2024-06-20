using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

using TLT.Input;

namespace TLT.TimeDilation
{
  public class TimeDilation : MonoBehaviour
  {
    [SerializeField, Range(0, 1)] private float _strength = 0.5f;
    [SerializeField] private float _transitionDuration = 1f;

    //-----------------------------------

    private InputHandler inputHandler;

    //===================================

    [Inject]
    private void Construct(InputHandler parInputHandler)
    {
      inputHandler = parInputHandler;
    }

    //===================================

    private void OnEnable()
    {
      inputHandler.AI_Player.Time.TimeDilation.performed += OnTimeDilation;
      inputHandler.AI_Player.Time.TimeDilation.started += OnTimeDilation;

      inputHandler.AI_Player.Time.TimeDilation.canceled += OnTimeDilation_Canceled;
    }

    private void OnDisable()
    {
      inputHandler.AI_Player.Time.TimeDilation.performed -= OnTimeDilation;
      inputHandler.AI_Player.Time.TimeDilation.started -= OnTimeDilation;

      inputHandler.AI_Player.Time.TimeDilation.canceled -= OnTimeDilation_Canceled;
    }

    //===================================

    private void OnTimeDilation(InputAction.CallbackContext parContext)
    {
      StartCoroutine(ChangeTimeScale(_strength, _transitionDuration));
    }

    private void OnTimeDilation_Canceled(InputAction.CallbackContext parContext)
    {
      StartCoroutine(ChangeTimeScale(1, _transitionDuration));
    }

    //===================================

    private IEnumerator ChangeTimeScale(float target, float duration)
    {
      float start = Time.timeScale;
      float elapsed = 0f;

      while (elapsed < duration)
      {
        Time.timeScale = Mathf.Lerp(start, target, elapsed / duration);
        elapsed += Time.unscaledDeltaTime;
        yield return null;
      }

      Time.timeScale = target;
    }

    //===================================
  }
}
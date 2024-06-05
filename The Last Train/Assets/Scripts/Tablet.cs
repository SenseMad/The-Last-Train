using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace TLT.InteractionObjects
{
  public class Tablet : ObjectInteraction
  {
    [SerializeField] private GameObject _objectCanvas;

    [SerializeField] private string _text;

    [SerializeField] private TextMeshProUGUI _textMeshPro;

    //-----------------------------------

    private bool isTimerRunning = false;

    //===================================

    public event Action OnOpenDialogBox;

    //===================================

    private void OnEnable()
    {
      OnInteract += OpenDialobBox;
    }

    private void OnDisable()
    {
      OnInteract -= OpenDialobBox;
    }

    //===================================

    public void OpenDialobBox()
    {
      if (isTimerRunning)
        return;

      OnOpenDialogBox?.Invoke();

      StartCoroutine(Timer());

      //Debug.Log("Табличка");
    }

    //===================================

    private IEnumerator Timer()
    {
      _objectCanvas.SetActive(true);

      _textMeshPro.text = $"{_text}";

      isTimerRunning = true;

      yield return new WaitForSeconds(2);

      _objectCanvas.SetActive(false);

      isTimerRunning = false;
    }

    //===================================
  }
}
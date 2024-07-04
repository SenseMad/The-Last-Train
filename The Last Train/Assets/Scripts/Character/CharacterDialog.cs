using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace TLT.CharacterManager
{
  public class CharacterDialog : MonoBehaviour
  {
    [SerializeField] private GameObject _dialogBox;

    [SerializeField] private TextMeshProUGUI _textMeshPro;

    //-----------------------------------

    private Coroutine coroutine;

    private Character character;

    private RectTransform _rectTransform;

    //===================================

    public event Action OnDialogueBegun;
    public event Action OnDialogueOver;

    //===================================

    private void Awake()
    {
      character = GetComponent<Character>();

      _rectTransform = _dialogBox.GetComponent<RectTransform>();
    }

    private void Update()
    {
      if (!_dialogBox.activeSelf)
        return;

      _rectTransform.localScale = new(1 * character.Direction, 1, 1);
    }

    //===================================

    public void DisplayText(string parText)
    {
      _textMeshPro.text = parText;
      _dialogBox.SetActive(true);

      OnDialogueBegun?.Invoke();

      if (coroutine != null)
        StopCoroutine(coroutine);

      coroutine = StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
      yield return new WaitForSeconds(2);

      _dialogBox.SetActive(false);

      coroutine = null;

      OnDialogueOver?.Invoke();
    }

    //===================================
  }
}
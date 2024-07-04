using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

using TLT.CharacterManager;

public class DeadMan : ObjectInteraction
{
  [SerializeField] private string _textMessage;
  [SerializeField] private string _nameScene;

  //-----------------------------------

  private Character character;

  //===================================

  [Inject]
  private void Construct(Character parCharacter)
  {
    character = parCharacter;
  }

  //===================================

  private void OnEnable()
  {
    OnInteract += DeadMan_OnInteract;

    character.CharacterDialog.OnDialogueOver += CharacterDialog_OnDialogueOver;
  }

  private void OnDisable()
  {
    OnInteract -= DeadMan_OnInteract;

    character.CharacterDialog.OnDialogueOver += CharacterDialog_OnDialogueOver;
  }

  //===================================

  private void DeadMan_OnInteract()
  {
    character.CharacterDialog.DisplayText(_textMessage);
  }

  private void CharacterDialog_OnDialogueOver()
  {
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
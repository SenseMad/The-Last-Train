using UnityEngine;
using Zenject;

using TLT.CharacterManager;
using TLT.Save;

public class DeadMan : ObjectInteraction
{
  [SerializeField] private string _textMessage;

  [Space]
  [SerializeField] private NamesScenes _namesScenes;

  //-----------------------------------

  private Character character;
  private GameManager gameManager;
  private LoadingScene loadingScene;

  //===================================

  [Inject]
  private void Construct(Character parCharacter, GameManager parGameManager, LoadingScene parLoadingScene)
  {
    character = parCharacter;
    gameManager = parGameManager;
    loadingScene = parLoadingScene;
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
    gameManager.SaveManager.DeleteSaveGame();

    loadingScene.LoadScene($"{_namesScenes}");
  }

  //===================================
}
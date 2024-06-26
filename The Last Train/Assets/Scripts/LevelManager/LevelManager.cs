using UnityEngine;
using System;
using Zenject;

using TLT.CharacterManager;

public class LevelManager : MonoBehaviour
{
  //===================================

  public Character Character { get; set; }

  //===================================

  public event Action OnChangeCharacter;

  //===================================

  [Inject]
  private void Construct(Character parCharacter)
  {
    Character = parCharacter;
  }

  //===================================

  public void ChangeCharacter()
  {
    //_character = parNewCharacter;

    OnChangeCharacter?.Invoke();
  }

  //===================================
}
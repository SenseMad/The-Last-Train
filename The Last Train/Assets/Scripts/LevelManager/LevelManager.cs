using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TLT.CharacterManager;
using System;

public class LevelManager : MonoBehaviour
{
  [SerializeField] private Character _character;

  //===================================

  public Character Character => _character;

  //===================================

  public event Action OnChangeCharacter;

  //===================================

  public void ChangeCharacter(Character parNewCharacter)
  {
    _character = parNewCharacter;

    OnChangeCharacter?.Invoke();
  }

  //===================================
}
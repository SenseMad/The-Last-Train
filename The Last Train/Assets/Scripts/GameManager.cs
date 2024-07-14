using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

using TLT.Save;
using TLT.Data;

public sealed class GameManager : MonoBehaviour
{
  public SaveManager SaveManager { get; private set; }

  //===================================

  private void Awake()
  {
    Init();
  }

  //===================================

  public void Init()
  {
    SaveManager = new();

    SaveManager.Init();
  }

  //===================================
}
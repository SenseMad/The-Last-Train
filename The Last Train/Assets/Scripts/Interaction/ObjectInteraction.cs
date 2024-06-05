using System;
using TLT.CharacterManager;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
  private SpriteOutline spriteOutline;

  //===================================

  public event Action OnInteract;
  public event Action<Character> OnInteractCharacter;

  //===================================

  private void Awake()
  {
    spriteOutline = GetComponent<SpriteOutline>();
  }

  //===================================

  public void Interact()
  {
    OnInteract?.Invoke();
  }
  
  public void InteractCharacter(Character parCharacter)
  {
    OnInteractCharacter?.Invoke(parCharacter);
  }

  public void Select()
  {
    spriteOutline.UpdateOutline(true);
  }

  public void DeSelect()
  {
    spriteOutline.UpdateOutline(false);
  }

  //===================================
}
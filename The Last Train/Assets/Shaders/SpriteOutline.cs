﻿using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
  [SerializeField] private SpriteRenderer spriteRenderer;

  [SerializeField] private Color _color = Color.white;

  //===================================

  private void Awake()
  {
    if (spriteRenderer == null)
      spriteRenderer = GetComponent<SpriteRenderer>();
  }

  //===================================

  public void UpdateOutline(bool outline)
  {
    MaterialPropertyBlock mpb = new();
    spriteRenderer.GetPropertyBlock(mpb);
    mpb.SetFloat("_Outline", outline ? 1f : 0);
    mpb.SetColor("_OutlineColor", _color);
    spriteRenderer.SetPropertyBlock(mpb);
  }

  //===================================
}
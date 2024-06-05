using UnityEngine;

public class CursorControl : MonoBehaviour
{
  private static CursorControl _instance;

  //===================================

  [SerializeField] private Texture2D _cursorTexture;

  [SerializeField] private CursorMode _cursorMode = CursorMode.ForceSoftware;

  //===================================

  private void Awake()
  {
    if (_instance == null)
    {
      _instance = this;
      DontDestroyOnLoad(gameObject);
      SetCursor();
    }
    else
    {
      Destroy(gameObject);
    }
  }

  //===================================

  private void SetCursor()
  {
    Cursor.SetCursor(_cursorTexture, Vector2.zero, _cursorMode);
  }

  //===================================
}
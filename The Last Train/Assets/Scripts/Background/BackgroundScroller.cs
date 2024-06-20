using UnityEngine;

namespace TLT.Background
{
  public class BackgroundScroller : MonoBehaviour
  {
    [SerializeField] private Transform _background_1;
    [SerializeField] private Transform _background_2;

    [SerializeField, Min(0)] private float _scrollSpeed;

    //-----------------------------------

    private float bgWidth;

    //===================================

    private void Start()
    {
      if (_background_1 == null)
        return;

      bgWidth = _background_1.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
    }

    private void Update()
    {
      Move();
    }

    //===================================

    private void Move()
    {
      if (_background_1 == null && _background_2 == null)
        return;

      _background_1.position = new Vector3(_background_1.position.x - _scrollSpeed * Time.deltaTime, _background_1.position.y, _background_2.position.z);
      _background_2.position -= new Vector3(_scrollSpeed * Time.deltaTime, 0f, 0f);

      if (_background_1.position.x < bgWidth - 1)
        _background_1.position += new Vector3(bgWidth * 2f, 0f, 0f);

      if (_background_1.position.x < bgWidth - 1)
        _background_2.position += new Vector3(bgWidth * 2f, 0f, 0f);
    }

    //===================================
  }
}
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace TLT.CameraManager
{
  public class CameraController : MonoBehaviour
  {
    [Header("ZOOM")]
    [SerializeField, Min(0)] private float _zoomInDistance = 0.8f;
    [SerializeField, Min(0)] private float _zoomOutDistance = 1f;
    [SerializeField, Min(0)] private float _zoomSpeed = 2f;

    [Space]
    [Header("DIRECTION")]
    [SerializeField, Min(0)] private float _transitionSpeed = 2.0f;

    //===================================

    private CinemachineCamera cinemachineCamera;
    private CinemachinePositionComposer cinemachinePositionComposer;

    private Vector2 targetScreenPosition;
    private Vector2 currentScreenPosition;
    //private CinemachineRecomposer recomposer;

    //===================================

    [Inject]
    private void Construct(CinemachineCamera parCinemachineCamera, CinemachinePositionComposer parCinemachinePositionComposer)
    {
      cinemachineCamera = parCinemachineCamera;
      cinemachinePositionComposer = parCinemachinePositionComposer;
    }

    //===================================

    private void Start()
    {
      //recomposer = cinemachineCamera.GetComponent<CinemachineRecomposer>();
    }

    private void Update()
    {
      currentScreenPosition = Vector2.Lerp(currentScreenPosition, targetScreenPosition, Time.deltaTime * _transitionSpeed);
      cinemachinePositionComposer.Composition.ScreenPosition = currentScreenPosition;
    }

    //===================================

    public void ChangeDirection(int parDirection)
    {
      targetScreenPosition = new(-0.25f * parDirection, 0.2f);
    }

    public void Zoom(bool parValue)
    {
      float targetWidth = parValue ? _zoomInDistance : _zoomOutDistance;

      //recomposer.ZoomScale = Mathf.Lerp(recomposer.ZoomScale, targetWidth, Time.deltaTime * _zoomSpeed);
    }

    //===================================
  }
}
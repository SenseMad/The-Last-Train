using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace TLT.CameraManager
{
  public class CameraController : MonoBehaviour
  {
    [SerializeField, Min(0)] private float _zoomInDistance = 0.8f;
    [SerializeField, Min(0)] private float _zoomOutDistance = 1f;
    [SerializeField, Min(0)] private float _zoomSpeed = 2f;

    //===================================

    private CinemachineCamera cinemachineCamera;
    private CinemachineRecomposer recomposer;

    //===================================

    [Inject]
    private void Construct(CinemachineCamera parCinemachineCamera)
    {
      cinemachineCamera = parCinemachineCamera;
    }

    //===================================

    private void Start()
    {
      recomposer = cinemachineCamera.GetComponent<CinemachineRecomposer>();
    }

    //===================================

    public void Zoom(bool parValue)
    {
      float targetWidth = parValue ? _zoomInDistance : _zoomOutDistance;

      recomposer.ZoomScale = Mathf.Lerp(recomposer.ZoomScale, targetWidth, Time.deltaTime * _zoomSpeed);

      //cinemachineFollowZoom.Width = Mathf.Lerp(cinemachineFollowZoom.Width, targetWidth, Time.deltaTime * _zoomSpeed);
    }

    //===================================
  }
}
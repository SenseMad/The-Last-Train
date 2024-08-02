using TLT.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace TLT.CharacterManager
{
  public class CharacterLantern : MonoBehaviour
  {
    [SerializeField] private GameObject _light;

    //-----------------------------------

    private TurningHand turningHand;

    private InputHandler inputHandler;

    private bool isLanternOn = false;

    //===================================

    private void Awake()
    {
      turningHand = GetComponent<TurningHand>();
    }

    private void Start()
    {
      _light.SetActive(isLanternOn);
    }

    private void Update()
    {
      if (!_light.activeSelf)
        return;

      _light.transform.localRotation = Quaternion.Euler(0, 0, -90 * turningHand.Turn);
    }

    //===================================

    [Inject]
    private void Construct(InputHandler parInputHandler)
    {
      inputHandler = parInputHandler;
    }

    //===================================

    private void OnEnable()
    {
      inputHandler.AI_Player.Player.Lantern.performed += Lantern_performed;
    }

    private void OnDisable()
    {
      inputHandler.AI_Player.Player.Lantern.performed -= Lantern_performed;
    }

    //===================================

    private void Lantern_performed(InputAction.CallbackContext obj)
    {
      isLanternOn = !isLanternOn;

      _light.SetActive(isLanternOn);
    }

    //===================================
  }
}
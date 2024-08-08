using UnityEngine;
using Zenject;

using TLT.Input;
using TLT.UI;

namespace TLT.Bike.Bike
{
  public class BikeDash : MonoBehaviour, IBikeBootstrap
  {
    [SerializeField, Min(0)] private float _speedDash = 21.0f;

    [SerializeField, Min(0)] private float _minSpeedDash = 10.0f;

    [SerializeField, Min(0)] private float _timeDash = 0.7f;

    [Space]
    [SerializeField] private LayerMask _ignoreLayer;

    [Space]
    [SerializeField] private Sprite _dashSpriteGreen;
    [SerializeField] private Sprite _dashSpriteRed;

    //-----------------------------------

    private InputHandler inputHandler;

    private BikeBody bikeBody;
    private BikeController bikeController;

    private UIGame uIGame;

    private float timeDash;

    //===================================

    public bool IsDashing { get; private set; }

    public bool IsDashingAnimator { get; private set; }

    public float SpeedDash => _speedDash;

    //===================================

    [Inject]
    private void Construct(InputHandler parInputHandler, UIGame parUIGame)
    {
      inputHandler = parInputHandler;
      uIGame = parUIGame;
    }

    //===================================

    public void CustomAwake()
    {
      bikeBody = GetComponent<BikeBody>();
      bikeController = GetComponent<BikeController>();
    }

    public void CustomStart()
    {
      if (_minSpeedDash > bikeBody.BikeData.MaxVelocity)
        _minSpeedDash = bikeBody.BikeData.MaxVelocity;
    }

    private void OnEnable()
    {
      inputHandler.AI_Player.Vehicle.Dash.performed += Dash_performed;

      bikeBody.OnGetInCar += BikeBody_OnGetInCar;
      bikeBody.OnGetOutCar += BikeBody_OnGetOutCar;
    }

    private void OnDisable()
    {
      inputHandler.AI_Player.Vehicle.Dash.performed -= Dash_performed;

      bikeBody.OnGetInCar -= BikeBody_OnGetInCar;
      bikeBody.OnGetOutCar -= BikeBody_OnGetOutCar;
    }

    private void Update()
    {
      if (bikeController.IsInCar)
      {
        if (bikeBody.VelocityMagnitude >= _minSpeedDash)
          uIGame.UpdateDashImage(_dashSpriteGreen);
        else
          uIGame.UpdateDashImage(_dashSpriteRed);
      }

      EndDash();
    }

    //===================================

    private void EndDash()
    {
      if (!IsDashing)
        return;

      timeDash += Time.deltaTime;

      if (timeDash >= _timeDash)
      {
        timeDash = 0;
        IsDashing = false;

        for (int i = 0; i < 32; i++)
        {
          if ((_ignoreLayer.value & (1 << i)) != 0)
          {
            Physics2D.IgnoreLayerCollision(gameObject.layer, i, false);
          }
        }
      }
    }

    private void EndAnimDash()
    {
      IsDashingAnimator = false;
    }

    private void Dash_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
      if (!bikeController.IsInCar)
        return;

      if (IsDashing)
        return;

      if (bikeBody.VelocityMagnitude < _minSpeedDash)
        return;

      IsDashing = true;

      for (int i = 0; i < 32; i++)
      {
        if ((_ignoreLayer.value & (1 << i)) != 0)
        {
          Physics2D.IgnoreLayerCollision(gameObject.layer, i, true);
        }
      }

      IsDashingAnimator = true;
      bikeController.Animator.SetTrigger(BikeAnimations.IS_DASH);
    }

    //===================================

    private void BikeBody_OnGetInCar()
    {
      uIGame.UpdateDashImage(true, _dashSpriteRed);
    }

    private void BikeBody_OnGetOutCar()
    {
      uIGame.UpdateDashImage(false);
    }

    //===================================
  }
}
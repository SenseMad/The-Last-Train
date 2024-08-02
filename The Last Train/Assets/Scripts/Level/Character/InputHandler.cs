using UnityEngine;
using UnityEngine.InputSystem;

namespace TLT.Input
{
  public sealed class InputHandler : MonoBehaviour
  {
    public AI_Player AI_Player { get; private set; }

    public bool IsInputHorizontal { get; set; } = true;

    public bool IsInputVertical { get; set; } = true;

    //===================================

    private void Awake()
    {
      AI_Player = new AI_Player();
    }

    private void OnEnable()
    {
      AI_Player.Enable();
    }

    private void OnDisable()
    {
      AI_Player.Disable();
    }

    //===================================

    private bool CanInput()
    {
      return AI_Player != null;
    }

    //===================================

    public Vector2 GetMousePosition()
    {
      return Mouse.current.position.ReadValue();
    }

    public int GetInputVertical()
    {
      return CanInput() && IsInputVertical ? Mathf.RoundToInt(AI_Player.Player.Move.ReadValue<Vector2>().y) : 0;
    }

    public int GetInputHorizontal()
    {
      return CanInput() && IsInputHorizontal ? Mathf.RoundToInt(AI_Player.Player.Move.ReadValue<Vector2>().x) : 0;
    }

    public int GetInputVehicle()
    {
      return CanInput() ? Mathf.RoundToInt(AI_Player.Vehicle.Move.ReadValue<Vector2>().y) : 0;
    }

    public int GetInputVehicleFlip()
    {
      return CanInput() ? Mathf.RoundToInt(AI_Player.Vehicle.Move.ReadValue<Vector2>().x) : 0;
    }

    //===================================
  }
}
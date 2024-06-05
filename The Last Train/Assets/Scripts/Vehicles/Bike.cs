using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLT.Vehicles
{
  public sealed class Bike : VehicleController
  {
    private float targetRotation;

    //===================================

    protected override void Update()
    {
      base.Update();

      AxisTorsion();
    }

    //===================================

    public override void Move()
    {
      int input = isInCar && isGrounded ? InputHandler.GetInputVertical() : 0;

      // Определяем направление взгляда
      bool facingRight = transform.localRotation.eulerAngles.y == 0;
      if (!facingRight)
        input = -input;

      float targetSpeed = _vehicleData.Speed * input;

      // Определяем, движемся ли задним ходом
      bool isMovingBackward = (facingRight && targetSpeed < 0) || (!facingRight && targetSpeed > 0);
      if (isMovingBackward)
        targetSpeed = _vehicleData.ReverseSpeed * input;

      float acceleration = _vehicleData.Speed / _vehicleData.AccelerationTimeMaxSpeed;
      float deceleration = _vehicleData.Speed / _vehicleData.DeccelerationTime;

      float currentSpeed = Rigidbody2D.velocity.x;
      if ((currentSpeed > 0 && input < 0) || (currentSpeed < 0 && input > 0))
        deceleration *= 2;

      float speedChangeRate = (Mathf.Abs(input) > 0) ? acceleration : deceleration;

      float moveVelocity = Mathf.MoveTowards(currentSpeed, targetSpeed, speedChangeRate * Time.deltaTime);

      Vector2 targetVelocity = new(moveVelocity, Rigidbody2D.velocity.y);
      Rigidbody2D.velocity = targetVelocity;
    }

    //===================================

    private void AxisTorsion()
    {
      int input = isInCar && !isGrounded ? InputHandler.GetInputHorizontal() : 0;

      if (input == 0)
        return;

      if (input < 0)
        targetRotation += 200f * Time.deltaTime;

      if (input > 0)
        targetRotation -= 200f * Time.deltaTime;

      float currentRotation = transform.rotation.eulerAngles.z;
      float newRotation = Mathf.LerpAngle(currentRotation, targetRotation, Time.deltaTime * 200f);

      transform.rotation = Quaternion.Euler(0, 0, newRotation);
    }

    //===================================
  }
}
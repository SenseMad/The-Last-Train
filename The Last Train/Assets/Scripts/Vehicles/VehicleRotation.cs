using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLT.Vehicles
{
  public sealed class VehicleRotation : MonoBehaviour
  {
    [SerializeField, Min(0)] private float _rotationSpeed = 200f;

    //===================================

    private float targetRotation;

    private bool isFlipped;

    private VehicleController vehicleController;

    //===================================

    private void Awake()
    {
      vehicleController = GetComponent<VehicleController>();
    }

    private void Update()
    {
      isFlipped = transform.localScale.x < 0;
    }

    private void FixedUpdate()
    {
      AxisTorsion();
    }

    //===================================

    private void AxisTorsion()
    {
      int input = vehicleController.IsInCar && !vehicleController.IsGrounded ? vehicleController.InputHandler.GetInputHorizontal() : 0;

      if (input == 0)
        return;

      Vector3 currentEulerAngles = transform.rotation.eulerAngles;

      if (input < 0)
      {
        if (currentEulerAngles.y > 0)
          targetRotation -= _rotationSpeed * Time.deltaTime;
        else
          targetRotation += _rotationSpeed * Time.deltaTime;
      }

      if (input > 0)
      {
        if (currentEulerAngles.y > 0)
          targetRotation += _rotationSpeed * Time.deltaTime;
        else
          targetRotation -= _rotationSpeed * Time.deltaTime;
      }

      float currentRotation = transform.rotation.eulerAngles.z;
      float newRotation = Mathf.LerpAngle(currentRotation, targetRotation, Time.deltaTime * _rotationSpeed);
      transform.rotation = Quaternion.Euler(currentEulerAngles.x, currentEulerAngles.y, newRotation);

      /*int input = vehicleController.IsInCar && !vehicleController.IsGrounded ? vehicleController.InputHandler.GetInputHorizontal() : 0;

      if (input == 0)
        return;

      // Определяем направление вращения в зависимости от ориентации мотоцикла и ввода
      float rotationAmount = _rotationSpeed * Time.fixedDeltaTime * input;
      if (isFlipped)
      {
        rotationAmount *= -1;
      }

      // Получаем текущий угол поворота вокруг оси Z
      float currentRotationZ = transform.rotation.eulerAngles.z;

      // Определяем целевой угол поворота
      float targetRotationZ = currentRotationZ + rotationAmount;

      // Плавное изменение угла поворота
      float smoothedRotationZ = Mathf.LerpAngle(currentRotationZ, targetRotationZ, Time.fixedDeltaTime * _rotationSpeed);

      // Применение нового угла поворота
      transform.rotation = Quaternion.Euler(0, 0, smoothedRotationZ);*/
    }

    //===================================
  }
}
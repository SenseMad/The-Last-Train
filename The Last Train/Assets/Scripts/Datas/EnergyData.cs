using UnityEngine;
using System.Collections;

using TLT.CharacterManager;

public class EnergyData : MonoBehaviour
{
  [SerializeField, Min(0)] private float _oscillationFrequency = 2f;
  [SerializeField, Min(0)] private float _oscillationAmplitude = 0.2f;

  //-----------------------------------

  private float timeToReachCharacter;

  private float elapsedTime = 0f;

  private Vector2 initialPosition;

  private Character character;
  private bool isMoveCharacter = false;

  //===================================

  private void Start()
  {
    initialPosition = transform.position;
  }

  private void Update()
  {
    if (character == null)
      return;

    if (!isMoveCharacter)
      return;

    elapsedTime += Time.deltaTime;
    float t = elapsedTime / timeToReachCharacter;

    if (t >= 1f)
      Destroy(gameObject);
    else
    {
      Vector2 linearPosition = Vector2.Lerp(initialPosition, character.transform.position, t);

      float oscillation = Mathf.Sin(t * Mathf.PI * _oscillationFrequency) * _oscillationAmplitude;
      Vector2 offset = new(0, oscillation);

      transform.position = linearPosition + offset;
    }
  }

  //===================================

  public void Initialize(Character parCharacter, float parTimeToReachCharacter, Vector2 parRandomPosition)
  {
    character = parCharacter;
    timeToReachCharacter = parTimeToReachCharacter;

    StartCoroutine(MoveToRandomPosition(parRandomPosition));
  }

  //===================================

  private IEnumerator MoveToRandomPosition(Vector2 parRandomPosition)
  {
    while ((Vector2)transform.position != parRandomPosition)
    {
      transform.position = Vector2.MoveTowards(transform.position, parRandomPosition, Time.deltaTime);
      yield return null;
    }

    isMoveCharacter = true;
  }

  //===================================
}
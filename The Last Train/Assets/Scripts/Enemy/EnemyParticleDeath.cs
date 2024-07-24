using UnityEngine;
using Zenject;

using TLT.CharacterManager;
using TLT.HealthManager;

namespace TLT.Enemy
{
  public class EnemyParticleDeath : MonoBehaviour
  {
    [SerializeField] private EnergyData _energyDataPrefab;

    [SerializeField, Min(0)] private float _duration;
    [SerializeField, Min(0)] private int _energyAmount;

    [SerializeField] private Vector2 _minMaxRadiusCenter = new(0.1f, 2.5f);

    //-----------------------------------

    private Character character;

    private EnemyAgent _agent;

    //===================================

    [Inject]
    private void Construct(Character parCharacter)
    {
      character = parCharacter;
    }

    //===================================

    private void Awake()
    {
      _agent = GetComponent<EnemyAgent>();

      if (_agent.Health == null)
        _agent.Health = GetComponent<Health>();
    }

    private void OnEnable()
    {
      _agent.Health.OnInstantlyKill += CreateObjectsEnergy;
    }

    private void OnDisable()
    {
      _agent.Health.OnInstantlyKill -= CreateObjectsEnergy;
    }

    //===================================

    private void CreateObjectsEnergy()
    {
      for (int i = 0; i < _energyAmount; i++)
      {
        EnergyData energyDataObject = Instantiate(_energyDataPrefab, transform);
        energyDataObject.transform.SetParent(null);
        energyDataObject.transform.position = transform.position;

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(_minMaxRadiusCenter.x, _minMaxRadiusCenter.y);
        Vector2 randomPosition = (Vector2)transform.position + randomDirection * randomDistance;

        energyDataObject.Initialize(character, _duration, randomPosition);
      }
    }

    //===================================
  }
}
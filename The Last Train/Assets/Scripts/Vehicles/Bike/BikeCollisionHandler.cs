using UnityEngine;

using TLT.CharacterManager;
using TLT.Enemy;

namespace TLT.Vehicles.Bike
{
  public class BikeCollisionHandler : MonoBehaviour
  {
    [SerializeField] private BikeController _bikeController;
    [SerializeField] private BikeManager _bikeManager;
    [SerializeField] private BikeBody _bikeBody;
    [SerializeField] private Character _character;

    [Space]
    [SerializeField] private float _slowDownFactor = 0.5f;
    [SerializeField] private LayerMask _enemyLayer;

    //-----------------------------------

    public BoxCollider2D _boxCollider2D;

    //===================================

    private void Update()
    {
      /*Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1.2f, _enemyLayer);

      foreach (var hitCollider in hitColliders)
      {
        if (hitCollider.TryGetComponent(out EnemyAgent parEnemyAgent))
        {
          if (_bikeManager.Grounded || _bikeManager.OnlyFrontGrounded)
          {
            parEnemyAgent.Health.TakeHealth(1);
            _character.Health.TakeHealth(1);
            _bikeController.Animator.SetTrigger("IsHurt");
            return;
          }

          if (_bikeManager.OnlyBackGrounded)
          {
            parEnemyAgent.Health.TakeHealth(1);
            return;
          }

          if (!_bikeManager.OnlyBackGrounded && !_bikeManager.OnlyFrontGrounded && !_bikeManager.Grounded)
          {
            parEnemyAgent.Health.InstantlyKill();
            return;
          }
        }
      }*/
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
      if (!collision.TryGetComponent(out EnemyAgent parEnemyAgent))
        return;

      if (_bikeManager.Grounded || _bikeManager.OnlyFrontGrounded)
      {
        parEnemyAgent.Health.TakeHealth(1);
        _character.Health.TakeHealth(1);
        _bikeController.Animator.SetTrigger("IsHurt");
        return;
      }

      if (_bikeManager.OnlyBackGrounded)
      {
        parEnemyAgent.Health.TakeHealth(1);
        return;
      }

      if (!_bikeManager.OnlyBackGrounded && !_bikeManager.OnlyFrontGrounded && !_bikeManager.Grounded)
      {
        parEnemyAgent.Health.InstantlyKill();
        return;
      }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
      if (!collision.collider.TryGetComponent(out EnemyAgent parEnemyAgent))
        return;

      if (_bikeManager.Grounded || _bikeManager.OnlyFrontGrounded)
      {
        parEnemyAgent.Health.TakeHealth(1);
        _character.Health.TakeHealth(1);
        _bikeController.Animator.SetTrigger("IsHurt");
        return;
      }

      if (_bikeManager.OnlyBackGrounded)
      {
        parEnemyAgent.Health.TakeHealth(1);
        return;
      }

      if (!_bikeManager.Grounded)
      {
        parEnemyAgent.Health.InstantlyKill();
        return;
      }
    }

    //===================================
  }
}
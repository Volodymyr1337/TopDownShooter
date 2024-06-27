using System;
using Application;
using Gameplay.Units.Player;
using UnityEngine;

namespace Gameplay.Units.Enemy
{
    public class EnemyController : BaseViewController<EnemyView>
    {
        private EnemyConfiguration _configuration;
        private PlayerController _playerController;
        private float _atkDelay = 0f;
        private float _health;
        private Action<EnemyController> _onKilled;
        
        public EnemyController(string assetName, EnemyConfiguration config, 
            PlayerController playerController, Action<EnemyController> onKilled) : base(assetName)
        {
            _configuration = config;
            _playerController = playerController;
            _onKilled = onKilled;
        }

        public override void Initialize()
        {
            base.Initialize();
            View.SetConfiguration(_configuration);
            View.Init(OnHit);
        }

        private void OnHit(float damage)
        {
            _health -= damage;
            if (_health <= 0f)
            {
                Die();
            }
        }

        public Vector3 GetPosition()
        {
            return View.transform.position;
        }
        
        public float GetUnitRadius()
        {
            return View.transform.localScale.x / 2f;
        }

        public void Spawn(Vector3 position)
        {
            View.gameObject.SetActive(true);
            View.transform.position = position;
            View.SpawnAnim(OnSpawnAnimCompleted);
        }

        private void OnSpawnAnimCompleted()
        {
            MonoService.OnUpdate += OnUpdate;
            _health = _configuration.health;
        }

        private void OnUpdate(float dt)
        {
            Vector3 direction = _playerController.GetPosition() - View.transform.position;
            View.OnUpdate(dt, direction.normalized);
            View.LookAt(direction.normalized);
            Attack(dt);
        }

        private void Die()
        {
            Stop();
            View.DieAnim(OnDieAnimCompleted);
        }

        public void OnDieAnimCompleted()
        {
            _onKilled?.Invoke(this);
            View.gameObject.SetActive(false);
        }

        public void Stop()
        {
            MonoService.OnUpdate -= OnUpdate;
        }

        private void Attack(float dt)
        {
            float distance = Vector3.Distance(_playerController.GetPosition(), View.transform.position);
            if (distance < _configuration.attackRange && _atkDelay <= 0)
            {
                _playerController.ReceiveDamage(_configuration.attackStrength);
                _atkDelay = _configuration.attackDelay;
            }
            else if (_atkDelay > 0)
            {
                _atkDelay -= dt;
            }
        }
    }
}

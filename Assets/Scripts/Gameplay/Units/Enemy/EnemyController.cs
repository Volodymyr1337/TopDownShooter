using System;
using Application;
using UnityEngine;

namespace Gameplay.Units.Enemy
{
    public class EnemyController : BaseViewController<EnemyView>, IUnitPosition
    {
        private EnemyConfiguration _configuration;
        private IUnitPosition _target;
        private float _health;
        private Action _onKilled;
        private Action<EnemyController> _onDieAnimCompleted;
        
        public EnemyController(string assetName, EnemyConfiguration config, 
            IUnitPosition target, Action onKilled, Action<EnemyController> onDieAnimCompleted) : base(assetName)
        {
            _configuration = config;
            _target = target;
            _onKilled = onKilled;
            _onDieAnimCompleted = onDieAnimCompleted;
        }

        public override void Initialize()
        {
            base.Initialize();
            View.SetConfiguration(_configuration);
            View.Init(OnHit);
        }

        private void OnHit(float damage)
        {
            if (_health < 0) return;
            
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
        
        public float GetUnitSize()
        {
            return View.transform.localScale.x * 0.1f;
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
            Vector3 direction = _target.GetPosition() - View.transform.position;
            if (Vector3.Distance(_target.GetPosition(), View.transform.position) < GetUnitSize())
                return;
            
            View.OnUpdate(dt, direction.normalized);
            View.LookAt(direction.normalized);
        }

        private void Die()
        {
            Stop();
            View.DieAnim(OnDieAnimCompleted);
            _onKilled?.Invoke();
        }

        public void OnDieAnimCompleted()
        {
            View.gameObject.SetActive(false);
            _onDieAnimCompleted?.Invoke(this);
        }

        public void Stop()
        {
            MonoService.OnUpdate -= OnUpdate;
        }
    }
}

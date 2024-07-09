using System;
using Application;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Bullet
{
    public class BulletController : BaseViewController<BulletView>
    {
        private Vector3 _position;
        private Vector3 _direction;
        private BulletConfiguration _configuration;
        private Tween _despawnTimer;
        
        public event Action<BulletController> OnDespawn;

        public BulletController(BulletConfiguration configuration) : base("Gameplay/Bullets/BulletView")
        {
            _configuration = configuration;
        }

        public override void Initialize()
        {
            base.Initialize();
            View.SetConfiguration(_configuration);
            View.gameObject.SetActive(false);
        }

        public override void Dispose()
        {
            _despawnTimer?.Kill();
            MonoService.OnUpdate -= OnUpdate;
            base.Dispose();
        }

        private void OnUpdate(float dt)
        {
            View.OnUpdate(dt, _direction);
        }

        public void Spawn(Vector3 position, Vector3 direction)
        {
            _position = position;
            _direction = direction.normalized;
            _despawnTimer = DOVirtual.DelayedCall(_configuration.lifetime, Despawn);
            MonoService.OnUpdate += OnUpdate;
            View.Init(_position, _direction, Despawn);
            View.gameObject.SetActive(true);
        }

        private void Despawn()
        {
            _despawnTimer?.Kill();
            MonoService.OnUpdate -= OnUpdate;
            View.gameObject.SetActive(false);
            OnDespawn?.Invoke(this);
        }
    }
}

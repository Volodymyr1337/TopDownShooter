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
            _direction = direction;
            DOVirtual.DelayedCall(_configuration.lifetime, Despawn);
            View.gameObject.SetActive(true);
            View.Init(_position, _direction, Despawn);
            MonoService.OnUpdate += OnUpdate;
        }

        private void Despawn()
        {
            MonoService.OnUpdate -= OnUpdate;
            View.gameObject.SetActive(false);
            OnDespawn?.Invoke(this);
        }
    }
}

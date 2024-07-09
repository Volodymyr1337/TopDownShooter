using System.Collections.Generic;
using Application;
using Gameplay.Units;
using Gameplay.Units.Player;
using Input;
using UnityEngine;

namespace Gameplay.Bullet
{
    public class BulletPoolController : BaseController
    {
        private IInput _aimInput;
        private IUnitPosition _playerPosition;
        private float _delay = 0f;
        private bool _isPointerDown = false;
        private BulletConfiguration _bulletConfiguration;
        private Queue<BulletController> _bulletPool = new Queue<BulletController>();

        public BulletPoolController(IInput aimInput, IUnitPosition playerPosition)
        {
            _aimInput = aimInput;
            _playerPosition = playerPosition;
        }
        
        public override void Initialize()
        {
            _aimInput.OnPressed += OnPressed;
            _aimInput.OnReleased += OnReleased;
            MonoService.OnUpdate += OnUpdate;
            
            string configPath = "Gameplay/Bullets/BulletConfiguration";
            _bulletConfiguration = Resources.Load<BulletConfiguration>(configPath);
        }

        public override void Dispose()
        {
            _aimInput.OnPressed -= OnPressed;
            _aimInput.OnReleased -= OnReleased;
            MonoService.OnUpdate -= OnUpdate;
            
            base.Dispose();
        }

        private void OnReleased()
        {
            _isPointerDown = false;
        }

        private void OnUpdate(float dt)
        {
            if (!_isPointerDown) return;
            
            if (_delay <= 0f)
            {
                SpawnBullet();
                _delay = _bulletConfiguration.attackDelay;
            }
            
            _delay -= dt;
        }

        private void OnPressed()
        {
            _isPointerDown = true;
        }

        private void SpawnBullet()
        {
            int poolSize = _bulletPool.Count;
            BulletController bullet;
            if (poolSize == 0)
            {
                bullet = CreateController(new BulletController(_bulletConfiguration));
                bullet.Initialize();
                bullet.OnDespawn += OnDespawnBullet;
            }
            else
            {
                bullet = _bulletPool.Dequeue();
            }
            
            bullet.Spawn(_playerPosition.GetPosition(), _aimInput.GetDirection());
        }

        private void OnDespawnBullet(BulletController bullet)
        {
            _bulletPool.Enqueue(bullet);
        }
    }
}

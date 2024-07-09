using System.Collections.Generic;
using Application;
using Gameplay.Units.Player;
using Input;
using UnityEngine;

namespace Gameplay.Bullet
{
    public class BulletPoolController : BaseController
    {
        private IInput _aimInput;
        private PlayerController _playerController;
        private float _delay = 0f;
        private bool _isPointerDown = false;
        private BulletConfiguration _bulletConfiguration;
        private Queue<BulletController> _bulletPool = new Queue<BulletController>();

        public BulletPoolController(IInput aimInput, PlayerController playerController)
        {
            _aimInput = aimInput;
            _playerController = playerController;
        }
        
        public override void Initialize()
        {
            _aimInput.OnPressed += OnPressed;
            _aimInput.OnReleased += OnReleased;
            
            string configPath = "Gameplay/Bullets/BulletConfiguration";
            _bulletConfiguration = Resources.Load<BulletConfiguration>(configPath);
        }

        public override void Dispose()
        {
            _aimInput.OnPressed -= OnPressed;
            _aimInput.OnReleased -= OnReleased;
            
            base.Dispose();
        }

        private void OnReleased()
        {
            MonoService.OnUpdate -= OnUpdate;
            _isPointerDown = false;
        }

        private void OnUpdate(float dt)
        {
            if (_delay <= 0f)
            {
                SpawnBullet();
                _delay = _playerController.Configuration.attackDelay;
            }
            
            _delay -= dt;
        }

        private void OnPressed()
        {
            _isPointerDown = true;
            MonoService.OnUpdate += OnUpdate;
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
            
            bullet.Spawn(_playerController.GetPosition(), _aimInput.GetDirection());
        }

        private void OnDespawnBullet(BulletController bullet)
        {
            _bulletPool.Enqueue(bullet);
        }
    }
}

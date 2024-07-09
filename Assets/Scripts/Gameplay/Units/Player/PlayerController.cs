using System;
using Application;
using Input;
using UnityEngine;

namespace Gameplay.Units.Player
{
    public class PlayerController : BaseViewController<PlayerView>, IUnitPosition
    {
        private IInput _movementInput;
        private IInput _aimInput;
        private float _health;
        private Action _onDied;
        private bool _isDead = false;
        public PlayerConfiguration Configuration { get; private set; }
        private GameplayConfiguration _gameplayConfiguration;

        public event Action<float> OnHealthUpdated;
        
        public PlayerController(IInput movementInput, IInput aimInput, GameplayConfiguration gameplayConfiguration, Action onDied) :
            base("Gameplay/Player/PlayerView")
        {
            _movementInput = movementInput;
            _gameplayConfiguration = gameplayConfiguration;
            _aimInput = aimInput;
            _onDied = onDied;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            string configPath = "Gameplay/Player/PlayerConfiguration";
            Configuration = Resources.Load<PlayerConfiguration>(configPath);
            View.SetConfiguration(Configuration);
            View.OnHit += ReceiveDamage;
            View.SetMovementBounds(_gameplayConfiguration.mapSize);
        }

        public override void Dispose()
        {
            View.OnHit -= ReceiveDamage;
            if (MonoService != null)
                MonoService.OnUpdate -= OnUpdate;

            base.Dispose();
        }

        public Vector3 GetPosition()
        {
            return View.transform.position;
        }

        private void OnUpdate(float dt)
        {
            View.OnUpdate(dt, _movementInput.GetDirection());
            View.LookAt(_aimInput.GetDirection());
        }

        private void ReceiveDamage(float damage)
        {
            if (_isDead) return;
            
            _health -= damage;
            OnHealthUpdated?.Invoke(_health/ Configuration.health);
            if (_health <= 0)
            {
                _isDead = true;
                Die();
            }
        }

        public void Spawn()
        {
            _isDead = false;
            MonoService.OnUpdate += OnUpdate;
            _health = View.Config.health;
            OnHealthUpdated?.Invoke(_health/ Configuration.health);
            View.transform.position = Vector3.zero;
            View.SpawnAnim();
        }

        private void Die()
        {
            _onDied?.Invoke();
            View.DieAnim();
            MonoService.OnUpdate -= OnUpdate;
        }
    }
}

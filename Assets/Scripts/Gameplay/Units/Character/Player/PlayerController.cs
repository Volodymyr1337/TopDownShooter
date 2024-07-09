using System;
using Application;
using Input;
using UnityEngine;

namespace Gameplay.Units.Character.Player
{
    public class PlayerController : BaseViewController<PlayerView>, ICharacterPosition
    {
        private IInput _movementInput;
        private IInput _aimInput;
        
        private float _health;
        private Action _onDied;
        
        private bool _isDead = false;

        private PlayerConfiguration _configuration;
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
            _configuration = Resources.Load<PlayerConfiguration>(configPath);
            View.SetConfiguration(_configuration);
            View.OnHit += ReceiveDamage;
            View.SetMovementBounds(_gameplayConfiguration.mapSize);
            MonoService.OnUpdate += OnUpdate;
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
            if (_isDead) return;
            
            View.OnUpdate(dt, _movementInput.GetDirection());
            View.LookAt(_aimInput.GetDirection());
        }

        private void ReceiveDamage(float damage)
        {
            if (_isDead) return;
            
            _health -= damage;
            OnHealthUpdated?.Invoke(_health/ _configuration.health);
            if (_health <= 0)
            {
                _isDead = true;
                Die();
            }
        }

        public void Spawn()
        {
            _isDead = false;
            _health = View.Config.health;
            OnHealthUpdated?.Invoke(_health/ _configuration.health);
            View.transform.position = Vector3.zero;
            View.SpawnAnim();
        }

        private void Die()
        {
            _onDied?.Invoke();
            View.DieAnim();
        }
    }
}

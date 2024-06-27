using Application;
using Gameplay.Bullet;
using Gameplay.UI;
using Gameplay.Units.Enemy;
using Gameplay.Units.Player;
using Input;
using UnityEngine;

namespace Gameplay
{
    public class GameplayController : BaseController
    {
        private EnemyPoolController _enemyPoolController;
        private BulletPoolController _bulletPoolController;
        private PlayerController _playerController;
        private GameplayHUDController _gameplayHUDController;
        private GameplayConfiguration _gameplayConfiguration;
        public override void Initialize()
        {
            _gameplayConfiguration = Resources.Load<GameplayConfiguration>("Gameplay/GameplayConfiguration");
            
            JoystickController movementJoystick = CreateController(new JoystickController("UI/MovementJoystick"));
            JoystickController aimJoystick = CreateController(new JoystickController("UI/AimJoystick"));
            _playerController = CreateController(new PlayerController(movementJoystick, aimJoystick, OnPlayerKilled));
            _enemyPoolController = CreateController(new EnemyPoolController(_gameplayConfiguration, _playerController));
            _bulletPoolController = CreateController(new BulletPoolController(aimJoystick, _playerController));
            _gameplayHUDController = CreateController(new GameplayHUDController());
            _enemyPoolController.OnEnemyKilled += OnEnemyKilled;
            _playerController.OnHealthUpdated += OnHealthUpdated;
            
            _gameplayHUDController.Initialize();
            _bulletPoolController.Initialize();
            _enemyPoolController.Initialize();
            movementJoystick.Initialize();
            aimJoystick.Initialize();
            _playerController.Initialize();
            Start();
        }

        public override void Dispose()
        {
            _enemyPoolController.OnEnemyKilled -= OnEnemyKilled;
            _playerController.OnHealthUpdated -= OnHealthUpdated;
            base.Dispose();
        }

        private void OnPlayerKilled()
        {
            _enemyPoolController.Active(false);
            GameOverScreenController gameOverScreenController = CreateController(new GameOverScreenController(OnRestartBtnClick));
            gameOverScreenController.Initialize();
        }

        private void OnRestartBtnClick()
        {
            _enemyPoolController.DespawnEnemies();
            Start();
        }

        private void Start()
        {
            _gameplayHUDController.UpdateKillCount(0);
            _playerController.Spawn();
            _enemyPoolController.Active(true);
        }
        
        
        private void OnEnemyKilled(int kills)
        {
            _gameplayHUDController.UpdateKillCount(kills);
            if (kills >= _gameplayConfiguration.requiredKills)
            {
                OnPlayerKilled();
            }
        }

        private void OnHealthUpdated(float health)
        {
            _gameplayHUDController.UpdatePlayerHp(health);
        }
    }
}

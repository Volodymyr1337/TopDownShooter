using Application;
using Gameplay.Bullet;
using Gameplay.Camera;
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
        private JoystickController _movementJoystick;
        private JoystickController _aimJoystick;
        private int _kills;
        
        public override void Initialize()
        {
            _gameplayConfiguration = Resources.Load<GameplayConfiguration>("Gameplay/GameplayConfiguration");
            
            _movementJoystick = CreateController(new JoystickController("UI/MovementJoystick"));
            _aimJoystick = CreateController(new JoystickController("UI/AimJoystick"));
            _playerController = CreateController(new PlayerController(_movementJoystick, _aimJoystick, _gameplayConfiguration, OnGameOver));
            _enemyPoolController = CreateController(new EnemyPoolController(_gameplayConfiguration, _playerController));
            _bulletPoolController = CreateController(new BulletPoolController(_aimJoystick, _playerController));
            _gameplayHUDController = CreateController(new GameplayHUDController());
            CameraController cameraController = CreateController(new CameraController(_playerController));
            _enemyPoolController.OnEnemyKilled += OnEnemyKilled;
            _playerController.OnHealthUpdated += OnHealthUpdated;
            
            _gameplayHUDController.Initialize();
            _bulletPoolController.Initialize();
            _enemyPoolController.Initialize();
            _movementJoystick.Initialize();
            _aimJoystick.Initialize();
            _playerController.Initialize();
            cameraController.Initialize();
            Start();
        }

        public override void Dispose()
        {
            _enemyPoolController.OnEnemyKilled -= OnEnemyKilled;
            _playerController.OnHealthUpdated -= OnHealthUpdated;
            base.Dispose();
        }

        private void OnGameOver()
        {
            _enemyPoolController.Active(false);
            GameOverScreenController gameOverScreenController = CreateController(new GameOverScreenController(OnRestartBtnClick));
            gameOverScreenController.Initialize();
            _movementJoystick.Release();
            _aimJoystick.Release();
        }

        private void OnRestartBtnClick()
        {
            _enemyPoolController.DespawnEnemies();
            Start();
        }

        private void Start()
        {
            _kills = 0;
            _gameplayHUDController.UpdateKillCount(_kills);
            _playerController.Spawn();
            _enemyPoolController.Active(true);
        }
        
        
        private void OnEnemyKilled()
        {
            _kills++;
            _gameplayHUDController.UpdateKillCount(_kills);
            if (_kills >= _gameplayConfiguration.requiredKills)
            {
                OnGameOver();
            }
        }

        private void OnHealthUpdated(float health)
        {
            _gameplayHUDController.UpdatePlayerHp(health);
        }
    }
}

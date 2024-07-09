using System;
using System.Collections.Generic;
using System.Linq;
using Application;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Units.Character.Enemy
{
    public class EnemyPoolController : BaseController
    {
        private List<EnemyController> _enemyPool = new List<EnemyController>();
        private List<EnemyController> _activeEnemyControllers = new List<EnemyController>();
        private GameplayConfiguration _gameplayConfiguration;
        private List<EnemyConfiguration> _enemyConfigurations = new List<EnemyConfiguration>();
        private bool _isActive;
        private ICharacterPosition _playerPosition;
        public event Action OnEnemyKilled;
        
        public EnemyPoolController(GameplayConfiguration gameplayConfiguration, ICharacterPosition playerPosition)
        {
            _gameplayConfiguration = gameplayConfiguration;
            _playerPosition = playerPosition;
        }
        
        public override void Initialize()
        {
            _enemyConfigurations = Resources.LoadAll<EnemyConfiguration>("Gameplay/Enemies/").ToList();
            SpawnEnemies();
        }

        public void Active(bool active)
        {
            _isActive = active;
            if (active)
            {
                SpawnEnemies();
            }
            else
            {
                StopEnemies();
            }
        }

        public void DespawnEnemies()
        {
            int count = _activeEnemyControllers.Count;
            for (int i = 0; i < count; i++)
            {
                _activeEnemyControllers[0].OnDieAnimCompleted();
            }
        }

        private void SpawnEnemies()
        {
            if (_isActive && _activeEnemyControllers.Count <= _gameplayConfiguration.maxEnemiesOnMap)
            {
                SpawnEnemy();
                if (_gameplayConfiguration.enemySpawnDelay > 0)
                {
                    DOVirtual.DelayedCall(_gameplayConfiguration.enemySpawnDelay, SpawnEnemies);
                }
                else
                {
                    SpawnEnemies();
                }
            }
        }

        private void SpawnEnemy()
        {
            int enemyPoolCount = _enemyPool.Count;
            if (enemyPoolCount == 0)
            {
                _enemyPool.Add(CreateRandomEnemy());
            }
            int randomIndex = Random.Range(0, enemyPoolCount);
            var enemyController = _enemyPool[randomIndex];
            enemyController.Spawn(GetRandomPosition());
            _enemyPool.RemoveAt(randomIndex);
            _activeEnemyControllers.Add(enemyController);
        }

        private EnemyController CreateRandomEnemy()
        {
            int randomIndex = Random.Range(0, _enemyConfigurations.Count);
            
            EnemyController enemyController = CreateController(new EnemyController(_gameplayConfiguration.enemyAssetName, 
                _enemyConfigurations[randomIndex], _playerPosition, OnKilled, OnDieAnimCompleted));
            
            enemyController.Initialize();
            return enemyController;
        }

        private void OnKilled()
        {
            OnEnemyKilled?.Invoke();
        }
        
        private void OnDieAnimCompleted(EnemyController controller)
        {
            _activeEnemyControllers.Remove(controller);
            _enemyPool.Add(controller);
        }

        private void StopEnemies()
        {
            foreach (EnemyController enemyController in _activeEnemyControllers)
            {
                enemyController.Stop();
            }
        }
        
        private Vector3 GetRandomPosition()
        {
            int maxAttempts = 10000;
            int attempts = 0;
            Vector3 position = Vector3.zero;
            do
            {
                float x = Random.Range(-_gameplayConfiguration.mapSize.x / 2, _gameplayConfiguration.mapSize.x / 2);
                float y = Random.Range(-_gameplayConfiguration.mapSize.y / 2, _gameplayConfiguration.mapSize.y / 2);
                position = new Vector3(x, y, 0);
                attempts++;
            } while (!IsValidPosition(position) && attempts < maxAttempts);
            return position;
        }

        private bool IsValidPosition(Vector3 position)
        {
            if (Vector3.Distance(position, _playerPosition.GetPosition()) < _gameplayConfiguration.minSpawnDistance)
            {
                return false;
            }

            foreach (EnemyController enemy in _activeEnemyControllers)
            {
                if (Vector3.Distance(position, enemy.GetPosition()) < enemy.GetUnitSize())
                {
                    return false;
                }
            }

            return true;
        }
    }
}

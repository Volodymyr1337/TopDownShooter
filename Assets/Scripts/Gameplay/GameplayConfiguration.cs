using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu]
    public class GameplayConfiguration : ScriptableObject
    {
        public string enemyAssetName;
        public float enemySpawnDelay;
        public int maxEnemiesOnMap;
        public int minSpawnDistance;
        public int requiredKills;
        public Vector2 mapSize;
    }
}

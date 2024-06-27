using UnityEngine;

namespace Gameplay.Units.Enemy
{
    [CreateAssetMenu]
    public class EnemyConfiguration : BaseUnitConfiguration
    {
        public float attackStrength;
        public float attackRange;
        public EnemyType enemyType;
    }

    public enum EnemyType
    {
        Fast,
        Slow
    }
}

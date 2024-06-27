using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Bullet
{
    [CreateAssetMenu]
    public class BulletConfiguration : BaseUnitConfiguration
    {
        public float lifetime;
        public float attackStrength;
    }
}

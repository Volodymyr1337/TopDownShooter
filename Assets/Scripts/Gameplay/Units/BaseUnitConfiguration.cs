using UnityEngine;

namespace Gameplay.Units
{
    public abstract class BaseUnitConfiguration : ScriptableObject
    {
        public int health;
        public float attackDelay;
        public float movementSpeed;
    }
}

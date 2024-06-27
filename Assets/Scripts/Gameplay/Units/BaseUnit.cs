using UnityEngine;

namespace Gameplay.Units
{
    public abstract class BaseUnit<TConfig> : MonoBehaviour where TConfig : BaseUnitConfiguration
    {
        public TConfig Config { get; private set; }

        public void SetConfiguration(TConfig config)
        {
            Config = config;
        }
        
        public void LookAt(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        
        public void OnUpdate(float dt, Vector2 direction)
        {
            transform.position += (Vector3)(Config.movementSpeed * direction * dt);
        }
    }
}

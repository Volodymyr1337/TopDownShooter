using UnityEngine;

namespace Gameplay.Units
{
    public abstract class BaseUnit<TConfig> : MonoBehaviour where TConfig : BaseUnitConfiguration
    {
        public TConfig Config { get; private set; }
        private Vector2 _bounds;

        public void SetConfiguration(TConfig config)
        {
            Config = config;
        }
        
        public void LookAt(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        public void SetMovementBounds(Vector2 bounds)
        {
            _bounds = bounds;
        }
        
        public void OnUpdate(float dt, Vector2 direction)
        {
            Vector3 position = transform.position + (Vector3)(Config.movementSpeed * direction * dt);
            if (_bounds.magnitude > 0)
            {
                ClampUnitPosition(position);
                return;
            }
            
            transform.position = position;
        }
        
        private void ClampUnitPosition(Vector3 position)
        {
            float halfWidth = _bounds.x / 2;
            float halfHeight = _bounds.y / 2;

            float minX = -halfWidth;
            float maxX = halfWidth;
            float minY = -halfHeight;
            float maxY = halfHeight;

            float clampedX = Mathf.Clamp(position.x, minX, maxX);
            float clampedY = Mathf.Clamp(position.y, minY, maxY);

            transform.position = new Vector3(clampedX, clampedY, position.z);
        }
    }
}

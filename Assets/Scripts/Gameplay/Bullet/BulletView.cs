using System;
using Gameplay.Units;
using Gameplay.Units.Enemy;
using UnityEngine;

namespace Gameplay.Bullet
{
    public class BulletView : BaseUnit<BulletConfiguration>
    {
        private Action _onHit;
        private bool _hit = false;
        
        public void Init(Vector3 position, Vector3 direction, Action onHit)
        {
            _hit = false;
            transform.position = position;
            LookAt(new Vector2(direction.y, -direction.x));
            _onHit = onHit;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_hit) return;
            
            if (col.gameObject.CompareTag("Enemy"))
            {
                _hit = true;
                col.gameObject.GetComponent<IDamageable>().TakeDamage(Config.attackStrength);
                _onHit?.Invoke();
            }
        }
    }
}

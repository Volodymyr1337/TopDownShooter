using System;
using Gameplay.Units;
using Gameplay.Units.Enemy;
using UnityEngine;

namespace Gameplay.Bullet
{
    public class BulletView : BaseUnit<BulletConfiguration>
    {
        private Action _onHit;
        
        public void Init(Vector3 position, Vector3 direction, Action onHit)
        {
            transform.position = position;
            LookAt(new Vector2(direction.y, -direction.x));
            _onHit = onHit;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                col.gameObject.GetComponent<EnemyView>().Hit(Config.attackStrength);
                _onHit?.Invoke();
            }
        }
    }
}

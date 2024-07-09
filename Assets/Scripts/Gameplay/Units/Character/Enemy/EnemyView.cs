using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Units.Character.Enemy
{
    public class EnemyView : BaseCharacter<EnemyConfiguration>
    {
        private float _originalSize;
        private Action<float> _onHit;
        private bool _isDealingDamage;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _isDealingDamage = true;
                Attack(collision.GetComponent<IDamageable>());
            }
        }

        private void Attack(IDamageable player)
        {
            if (!_isDealingDamage) return;
            
            player.TakeDamage(Config.attackStrength);
            
            DOVirtual.DelayedCall(Config.attackDelay, () => Attack(player));
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _isDealingDamage = false;
            }
        }
    }
}

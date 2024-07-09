using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Units.Character.Enemy
{
    public class EnemyView : BaseCharacter<EnemyConfiguration>
    {
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

        public void SetAvatar(Sprite avatar)
        {
            _image.sprite = avatar;
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

        public void Stop()
        {
            _isDealingDamage = false;
        }
    }
}

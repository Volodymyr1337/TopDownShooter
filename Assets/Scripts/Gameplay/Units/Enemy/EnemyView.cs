using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Units.Enemy
{
    public class EnemyView : BaseUnit<EnemyConfiguration>, IDamageable
    {
        [SerializeField] private SpriteRenderer _image;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private float _hitAnimTime = 0.5f;
        [SerializeField] private float _spawnAnimTime = 0.4f;
        
        private float _originalSize;
        private Action<float> _onHit;
        private bool _isDealingDamage;

        private void Awake()
        {
            _originalSize = transform.localScale.x;
        }

        public void Init(Action<float> onHit)
        {
            _onHit = onHit;
        }
        
        public void SpawnAnim(TweenCallback onComplete)
        {
            transform.DOScale(_originalSize, _spawnAnimTime).From(0f).OnComplete(onComplete);
        }
        
        public void DieAnim(TweenCallback onComplete)
        {
            transform.DOScale(0f, _spawnAnimTime).From(_originalSize).OnComplete(onComplete);
        }
        
        public void TakeDamage(float damage)
        {
            _image.DOGradientColor(_gradient, _hitAnimTime);
            _onHit?.Invoke(damage);
        }
        
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

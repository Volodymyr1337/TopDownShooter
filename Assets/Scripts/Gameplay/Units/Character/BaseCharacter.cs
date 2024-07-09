using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Units.Character
{
    public class BaseCharacter<TConfig> : BaseUnit<TConfig>, IDamageable where TConfig : BaseUnitConfiguration
    {
        [SerializeField] protected SpriteRenderer _image;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private float _hitAnimTime = 0.5f;
        [SerializeField] private float _spawnAnimTime = 0.4f;
        
        private float _originalSize;
        public event Action<float> OnHit;

        private void Awake()
        {
            _originalSize = transform.localScale.x;
        }
        
        public void SpawnAnim(TweenCallback onComplete = null)
        {
            transform.DOScale(_originalSize, _spawnAnimTime).From(0f).OnComplete(onComplete);
        }
        
        public void DieAnim(TweenCallback onComplete = null)
        {
            transform.DOScale(0f, _spawnAnimTime).From(_originalSize).OnComplete(onComplete);
        }

        public void TakeDamage(float damage)
        {
            _image.DOGradientColor(_gradient, _hitAnimTime);
            OnHit?.Invoke(damage);
        }
    }
}

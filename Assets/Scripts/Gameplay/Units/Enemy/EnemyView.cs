using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Units.Enemy
{
    public class EnemyView : BaseUnit<EnemyConfiguration>
    {
        private float _originalSize;
        private Action<float> _onHit;

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
            transform.DOScale(_originalSize, 0.5f).From(0f).OnComplete(onComplete);
        }
        
        public void DieAnim(TweenCallback onComplete)
        {
            transform.DOScale(0f, 0.5f).From(_originalSize).OnComplete(onComplete);
        }

        public void Hit(float damage)
        {
            _onHit?.Invoke(damage);
        }
    }
}

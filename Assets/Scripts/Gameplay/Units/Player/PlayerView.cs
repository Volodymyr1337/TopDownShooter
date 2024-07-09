using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Units.Player
{
    public class PlayerView : BaseUnit<PlayerConfiguration>, IDamageable
    {
        private float _originalSize = 1f;
        public event Action<float> OnHit;

        private void Awake()
        {
            _originalSize = transform.localScale.x;
        }

        public void SpawnAnim()
        {
            transform.DOScale(_originalSize, .5f).From(0f);
        }

        public void DieAnim()
        {
            transform.DOScale(0, .5f).From(_originalSize);
        }

        public void TakeDamage(float damage)
        {
            OnHit?.Invoke(damage);
        }
    }
}

using System;
using UnityEngine;

namespace Services.Mono
{
    public class MonoComponent : MonoBehaviour
    {
        public event Action OnUpdate;
        // other mono events

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }
    }
}

using System;
using UnityEngine;

namespace Services.Mono
{
    public class MonoService : IService
    {
        private MonoComponent _monoComponent;

        public event Action<float> OnUpdate;
        
        public void Initialize()
        {
            _monoComponent = UnityEngine.Object.Instantiate(new GameObject("Mono")).AddComponent<MonoComponent>();
            _monoComponent.OnUpdate += Update;
        }

        public void Dispose()
        {
            if (_monoComponent != null)
                _monoComponent.OnUpdate -= Update;
        }

        private void Update()
        {
            OnUpdate?.Invoke(Time.deltaTime);
        }
    }
}

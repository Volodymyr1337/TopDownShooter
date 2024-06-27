using System;

namespace Services
{
    public interface IService : IDisposable
    {
        void Initialize();
    }
}

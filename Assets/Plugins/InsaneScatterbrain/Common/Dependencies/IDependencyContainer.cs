using System;

namespace InsaneScatterbrain.Dependencies
{
    public interface IDependencyContainer
    {
        void Register<T>(Func<T> obj) where T : class;
        T Get<T>() where T : class;
    }
}
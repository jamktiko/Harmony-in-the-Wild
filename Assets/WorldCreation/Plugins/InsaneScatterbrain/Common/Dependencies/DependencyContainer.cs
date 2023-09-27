using System;
using System.Collections.Generic;

namespace InsaneScatterbrain.Dependencies
{
    public class DependencyContainer : IDependencyContainer
    {
        private readonly Dictionary<Type, Func<object>> objs = new Dictionary<Type, Func<object>>();
        public void Register<T>(Func<T> getObjFunc) where T : class => objs.Add(typeof(T), getObjFunc);

        public T Get<T>() where T : class
        {
            var type = typeof(T);
            if (!objs.ContainsKey(type))
            {
                throw new DependencyNotFoundException(type);
            }
            
            return objs[type]() as T;
        }

        public virtual void Clear() => objs.Clear();
    }
}
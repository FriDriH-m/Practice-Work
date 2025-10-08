using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils 
{
    public class DIContainer
    {
        private static readonly Lazy<DIContainer> _instance = new Lazy<DIContainer>(() => new DIContainer());
        public static DIContainer Instance => _instance.Value;

        private readonly Dictionary<(Type, string), object> services = new Dictionary<(Type, string), object>();
        private readonly Dictionary<Type, object> singletons = new Dictionary<Type, object>();
        private DIContainer() { }
        
        public void Register<T>(T service, string name = "", bool isSingleton = false) where T : class
        {
            if (service == null)
            {
                Debug.LogWarning("DIContainer: service" + typeof(T) + " is null");
                return;
            }

            Type type = typeof(T);

            if (isSingleton)
            {
                if (singletons.ContainsKey(type))
                {
                    Debug.LogWarning($"Singleton of type {type} is already registered. Overwriting the existing instance.");
                    singletons[type] = service;
                }
                else singletons.Add(type, service);
                return;
            }
            var key = (type, name);
            if (services.ContainsKey(key))
            {
                Debug.LogWarning($"Service of type {type} with name '{name}' is already registered. Overwriting.");
                services[key] = service;
            }
            else
            {
                services.Add(key, service);
            }
        }
        public T Get<T>(string name = "") where T : class
        {
            if (singletons.TryGetValue(typeof(T), out var singleton))
            {
                return singleton as T;
            }
            if (services.TryGetValue((typeof(T), name), out var service))
            {
                return service as T;
            }
            Debug.LogWarning($"Service of type {typeof(T)} with name '{name}' is not registered.");
            return null;
        }
        public bool TryGet<T>(out T service, string name = "") where T : class
        {
            service = Get<T>(name);
            return service != null;
        }
        public void Unregister<T>(T service, string name = "") where T : class
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            services.Remove((typeof(T), name));
            singletons.Remove(typeof(T));
        }
        public void Clear()
        {
            singletons.Clear();
            services.Clear();
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator Instance { get; private set; }

    private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

    private void Awake()
    {
        if (Instance == null)
        {
            Debug.LogWarning("нулл");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Debug.LogWarning("не нулл");
            Destroy(gameObject);
            return;
        }
    }
    private void OnEnable()
    {
        //Register<XRInput>(new XRInput());
        //Get<XRInput>().Enable();
    }
    private void OnDisable()
    {
        //Unregister<XRInput>(new XRInput());
        //Get<XRInput>().Disable();
    }
    public void Register<T>(T wrapper) where T : class
    {
        if (wrapper == null) throw new ArgumentNullException(nameof(wrapper));
        services[typeof(T)] = wrapper;
    }
    public T Get<T>() where T : class
    {
        return services[typeof(T)] as T;
    }

    public bool TryGet<T>(out T service) where T : class
    {
        service = Get<T>();
        return service != null;
    }

    public void Unregister<T>(T behaviour) where T : class
    {
        services.Remove(typeof(T));
    }

    public void Clear() => services.Clear();
}

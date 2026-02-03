using System.Collections.Generic;
using UnityEngine;
using System;
public static class ObjectResolver 
{
    private static readonly Dictionary<Type, object> _instancePerTypeMap = new();

    public static void RegisterInstance<T>(T instance)
    {
        _instancePerTypeMap[typeof(T)] = instance;
    }

    public static void UnregisterInstance<T>() 
    { 
        _instancePerTypeMap.Remove(typeof(T));
    }

    public static T Resolve<T>()
    {
        Type instanceType = typeof(T);
        if(_instancePerTypeMap.TryGetValue(instanceType, out var value))
        {
            return (T)value;
        }
        else
        {
            Debug.LogError($"Could not resolve type {instanceType}");
            return default;
        }
    }
}

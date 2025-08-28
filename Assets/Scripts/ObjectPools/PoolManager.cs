using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private readonly Dictionary<Type, object> poolsDictionary = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one PoolManager!");
        }

        Instance = this;
    }

    public void RegisterPool<T>(ObjectPool<T> pool)
        where T : Component, IObjectItemPoolable
    {
        poolsDictionary[typeof(T)] = pool;
    }

    public ObjectPool<T> GetPool<T>()
        where T : Component, IObjectItemPoolable
    {
        if (poolsDictionary.TryGetValue(typeof(T), out object pool))
        {
            return (ObjectPool<T>)pool;
        }

        Debug.LogError($"Pool for type {typeof(T).Name} not found!");
        return null;
    }

    public T Get<T>()
        where T : Component, IObjectItemPoolable
    {
        return GetPool<T>().GetObject();
    }

    public void Return<T>(T obj)
        where T : Component, IObjectItemPoolable
    {
        GetPool<T>().ReturnToPool(obj);
    }
}

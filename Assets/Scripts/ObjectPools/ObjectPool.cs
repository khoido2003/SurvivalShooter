using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : IObjectPoolable<T>
    where T : Component, IObjectItemPoolable
{
    /*
         * NOTE FOR FUTURE ME:
         * -------------------
         * We're using an INSTANCE pool for bullets (and other objects),
         * instead of a global PoolManager.
         *
         * Why?
         *  1. Simpler: each pool is self-contained and easy to debug in scene.
         *  2. Explicit: BulletPool.Instance makes it clear where bullets come from.
         *  3. Flexible enough: ObjectPool<T> + IObjectItemPoolable already gives
         *     us abstraction if we want to swap implementations later.
         *
         * Only switch to a central PoolManager if:
         *  - We have MANY different pools and need unified management
         *  - Or if we want to dynamically spawn/destroy pools at runtime
         *
         * For now, Instance-based pools keep the project easier to read & maintain.
    */

    private readonly T prefab;
    private readonly Queue<T> objectsQueue = new();
    private readonly Transform parent;

    public ObjectPool(T prefab, int initSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initSize; i++)
        {
            SpawnObjectToPool();
        }
    }

    private T SpawnObjectToPool()
    {
        T newObj = Object.Instantiate(prefab, parent);
        newObj.gameObject.SetActive(false);

        objectsQueue.Enqueue(newObj);

        return newObj;
    }

    public T GetObject()
    {
        // If run out of object, just create a new one then add back to queue later
        if (objectsQueue.Count == 0)
        {
            SpawnObjectToPool();
        }

        T obj = objectsQueue.Dequeue();
        obj.gameObject.SetActive(true);
        obj.transform.parent = null;

        if (obj is IObjectItemPoolable poolable)
        {
            poolable.OnSpawn();
        }

        return obj;
    }

    public void ReturnToPool(T obj)
    {
        if (obj is IObjectItemPoolable poolable)
        {
            poolable.OnDespawn();
        }

        obj.gameObject.SetActive(false);
        obj.transform.SetParent(parent);
        objectsQueue.Enqueue(obj);
    }
}

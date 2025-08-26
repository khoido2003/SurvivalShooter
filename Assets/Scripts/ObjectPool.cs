using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private int poolSize = 50;

    private Queue<GameObject> bulletPool;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of ObjectPool!");
        }

        Instance = this;
    }

    private void Start()
    {
        bulletPool = new Queue<GameObject>();

        CreateInitialPool();
    }

    private void CreateInitialPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateObject();
        }
    }

    private void CreateObject()
    {
        GameObject newBullet = Instantiate(bulletPrefab, transform);
        newBullet.SetActive(false);
        bulletPool.Enqueue(newBullet);
    }

    public GameObject GetBullet()
    {
        if (bulletPool.Count == 0)
        {
            CreateObject();
        }

        GameObject bulletToGet = bulletPool.Dequeue();

        bulletToGet.transform.parent = null;
        bulletToGet.SetActive(true);
        return bulletToGet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bullet.transform.parent = transform;
        bulletPool.Enqueue(bullet);
    }
}

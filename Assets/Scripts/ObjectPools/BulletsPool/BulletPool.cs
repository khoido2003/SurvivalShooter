using UnityEngine;

public class BulletPool : MonoBehaviour, IObjectPoolable<Bullet>
{
    [SerializeField]
    private Bullet bulletPrefab;

    [SerializeField]
    private int poolSize = 30;

    private ObjectPool<Bullet> pool;

    public static BulletPool Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one BulletPool");
        }

        Instance = this;

        pool = new ObjectPool<Bullet>(bulletPrefab, poolSize, transform);
    }

    private void Start()
    {
        PoolManager.Instance.RegisterPool(pool);
    }

    public Bullet GetObject()
    {
        return pool.GetObject();
    }

    public void ReturnToPool(Bullet obj)
    {
        pool.ReturnToPool(obj);
    }
}

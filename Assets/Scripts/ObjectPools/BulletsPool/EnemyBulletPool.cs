using UnityEngine;

public class EnemyBulletPool : MonoBehaviour, IObjectPoolable<EnemyBullet>
{
    [SerializeField]
    private EnemyBullet bulletPrefab;

    [SerializeField]
    private int poolSize = 30;

    private ObjectPool<EnemyBullet> pool;

    private void Awake()
    {
        pool = new(bulletPrefab, poolSize, transform);
    }

    private void Start()
    {
        PoolManager.Instance.RegisterPool(pool);
    }

    public EnemyBullet GetObject()
    {
        return pool.GetObject();
    }

    public void ReturnToPool(EnemyBullet obj)
    {
        pool.ReturnToPool(obj);
    }
}

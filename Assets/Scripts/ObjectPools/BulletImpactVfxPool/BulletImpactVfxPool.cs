using UnityEngine;

public class BulletImpactVfxPool : MonoBehaviour, IObjectPoolable<BulletImpactVfx>
{
    [SerializeField]
    private BulletImpactVfx bulletImpactVfxPrefab;

    [SerializeField]
    private int poolSize = 30;

    private ObjectPool<BulletImpactVfx> pool;

    private void Awake()
    {
        pool = new(bulletImpactVfxPrefab, poolSize, transform);
    }

    private void Start()
    {
        PoolManager.Instance.RegisterPool(pool);
    }

    public BulletImpactVfx GetObject()
    {
        return pool.GetObject();
    }

    public void ReturnToPool(BulletImpactVfx obj)
    {
        pool.ReturnToPool(obj);
    }
}

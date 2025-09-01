using UnityEngine;

public class AmmoPickupObjectPool : MonoBehaviour, IObjectPoolable<PickupAmmo>
{
    [SerializeField]
    private PickupAmmo pickupAmmoPrefabs;

    [SerializeField]
    private int poolSize;

    private ObjectPool<PickupAmmo> pool;

    private void Start()
    {
        pool = new(pickupAmmoPrefabs, poolSize, transform);

        PoolManager.Instance.RegisterPool(pool);
    }

    public PickupAmmo GetObject()
    {
        return pool.GetObject();
    }

    public void ReturnToPool(PickupAmmo obj)
    {
        pool.ReturnToPool(obj);
    }
}

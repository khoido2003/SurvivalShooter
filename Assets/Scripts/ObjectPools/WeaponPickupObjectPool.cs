using UnityEngine;

public class WeaponPickupObjectPool : MonoBehaviour, IObjectPoolable<PickUpWeapon>
{
    [SerializeField]
    private PickUpWeapon weaponPickupObject;

    [SerializeField]
    private int poolSize;

    private ObjectPool<PickUpWeapon> pool;

    public WeaponPickupObjectPool Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are more than one intstance of  WeaponPickupObjectPool");
            return;
        }

        Instance = this;

        pool = new(weaponPickupObject, poolSize, transform);
    }

    private void Start()
    {
        PoolManager.Instance.RegisterPool(pool);
    }

    public PickUpWeapon GetObject()
    {
        return pool.GetObject();
    }

    public void ReturnToPool(PickUpWeapon obj)
    {
        pool.ReturnToPool(obj);
    }
}

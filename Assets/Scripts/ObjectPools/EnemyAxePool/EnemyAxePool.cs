using UnityEngine;

public class EnemyAxePool : MonoBehaviour, IObjectPoolable<EnemyAxe>
{
    [SerializeField]
    private EnemyAxe axeThrowPrefab;

    [SerializeField]
    private int poolSize = 10;

    private ObjectPool<EnemyAxe> pool;

    private void Awake()
    {
        pool = new(axeThrowPrefab, poolSize, transform);
    }

    private void Start()
    {
        PoolManager.Instance.RegisterPool(pool);
    }

    public EnemyAxe GetObject()
    {
        return pool.GetObject();
    }

    public void ReturnToPool(EnemyAxe obj)
    {
        pool.ReturnToPool(obj);
    }
}

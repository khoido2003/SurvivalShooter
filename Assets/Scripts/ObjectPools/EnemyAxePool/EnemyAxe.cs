using Unity.VisualScripting;
using UnityEngine;

public class EnemyAxe : MonoBehaviour, IObjectItemPoolable
{
    private Transform player;

    private float flySpeed = 5f;
    private float rotationSpeed = 1600f;
    private Vector3 direction;

    [SerializeField]
    private Transform axeVisuals;

    [SerializeField]
    private Rigidbody rb;

    private float timer = 1.5f;

    private Transform originalParent;

    private void Awake()
    {
        originalParent = transform.parent;
    }

    public void AxeSetup(float flySpeed, Transform player, float timer)
    {
        this.flySpeed = flySpeed;
        this.player = player;
        this.timer = timer;
    }

    public void Update()
    {
        axeVisuals.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);

        timer -= Time.deltaTime;

        if (timer > 0)
        {
            direction = player.position + Vector3.up - transform.position;
        }
        else
        {
            BulletImpactVfx bulletImpactVfx = PoolManager.Instance.Get<BulletImpactVfx>();

            bulletImpactVfx.transform.position = transform.position;
            PoolManager.Instance.Return(this);
        }

        rb.linearVelocity = direction.normalized * flySpeed;
        transform.forward = rb.linearVelocity;
    }

    public void OnDespawn()
    {
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public void OnSpawn()
    {
        transform.parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        Player player = other.GetComponent<Player>();

        if (bullet != null || player != null)
        {
            BulletImpactVfx bulletImpactVfx = PoolManager.Instance.Get<BulletImpactVfx>();

            bulletImpactVfx.transform.position = transform.position;

            PoolManager.Instance.Return(this);
        }
    }
}

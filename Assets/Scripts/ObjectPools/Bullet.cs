using UnityEngine;

public class Bullet : MonoBehaviour, IObjectItemPoolable
{
    private Rigidbody rigidBody;

    [SerializeField]
    private GameObject bulletImpactVfx;

    [SerializeField]
    private TrailRenderer trailRenderer;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private BoxCollider boxCollider;

    private Vector3 startPosition;
    private float flyDistance;
    private bool bulletDisabled;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateTrailVisual();
        CheckIfBulletDisabled();
        CheckIfBulletNeededReturnToThePool();
    }

    private void CheckIfBulletNeededReturnToThePool()
    {
        if (trailRenderer.time < 0)
        {
            PoolManager.Instance.Return<Bullet>(this);
        }
    }

    private void CheckIfBulletDisabled()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && !bulletDisabled)
        {
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
            bulletDisabled = true;
        }
    }

    private void UpdateTrailVisual()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5f)
        {
            float timeTrailRenderFaded = 2f;

            trailRenderer.time -= timeTrailRenderFaded * Time.deltaTime;
        }
    }

    public void BulletSetup(float flyDistance)
    {
        float extraFlyDistance = 2f;
        this.flyDistance = flyDistance + extraFlyDistance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        CreateImpactBulletFx(collision);

        PoolManager.Instance.Return<Bullet>(this);
    }

    private void CreateImpactBulletFx(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];

            BulletImpactVfx impactVfx = PoolManager.Instance.Get<BulletImpactVfx>();

            impactVfx.transform.position = contact.point;
            impactVfx.transform.rotation = Quaternion.LookRotation(contact.normal);
        }
    }

    public void OnSpawn()
    {
        bulletDisabled = false;
        boxCollider.enabled = true;
        meshRenderer.enabled = true;

        trailRenderer.time = 0.25f;
        startPosition = transform.position;
    }

    public void OnDespawn()
    {
        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;

        trailRenderer.Clear();
    }
}

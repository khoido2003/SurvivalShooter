using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rigidBody;

    [SerializeField]
    private GameObject bulletImpactVfx;

    [SerializeField]
    private Rigidbody rigidbody;

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
            ObjectPool.Instance.ReturnBullet(gameObject);
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
        bulletDisabled = false;
        boxCollider.enabled = true;
        meshRenderer.enabled = true;

        trailRenderer.time = .25f;
        startPosition = transform.position;

        float extraFlyDistance = 2f;
        this.flyDistance = flyDistance + extraFlyDistance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        CreateImpactBulletFx(collision);

        ObjectPool.Instance.ReturnBullet(gameObject);
    }

    private void CreateImpactBulletFx(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];

            GameObject newImpactFx = Instantiate(
                bulletImpactVfx,
                contact.point,
                Quaternion.LookRotation(contact.normal)
            );

            Destroy(newImpactFx, 1f);
        }
    }
}

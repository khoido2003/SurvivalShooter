using System.Collections;
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

    public float impactForce;

    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start() { }

    protected virtual void Update()
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

    protected virtual void CheckIfBulletDisabled()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && !bulletDisabled)
        {
            bulletDisabled = true;

            StartCoroutine(FadeOutAndReturn());
        }
    }

    private IEnumerator FadeOutAndReturn()
    {
        float fadeDuration = trailRenderer.time;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        PoolManager.Instance.Return<Bullet>(this);
    }

    private void UpdateTrailVisual()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance)
        {
            float timeTrailRenderFaded = 2f;

            trailRenderer.time -= timeTrailRenderFaded * Time.deltaTime;
        }
    }

    public void BulletSetup(float flyDistance = 100, float impactForce = 100)
    {
        this.impactForce = impactForce;
        float extraFlyDistance = 2f;
        this.flyDistance = flyDistance + extraFlyDistance;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        CreateImpactBulletFx(collision);

        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();

        EnemyShield shield = collision.gameObject.GetComponentInChildren<EnemyShield>();

        // Debug.Log(
        //     "Bullet collided with: "
        //         + collision.gameObject.name
        //         + " on layer "
        //         + LayerMask.LayerToName(collision.gameObject.layer)
        // );

        // If the enemy have shield then bullet only damage the shield
        if (shield != null)
        {
            shield.ReduceDurability();

            PoolManager.Instance.Return<Bullet>(this);
            return;
        }

        if (enemy != null)
        {
            Vector3 force = rigidBody.linearVelocity.normalized * impactForce;

            Rigidbody hitRigidBody = collision.collider.attachedRigidbody;

            enemy.GetHit();
            enemy.DeadHitImpact(force, collision.contacts[0].point, hitRigidBody);
        }

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

    public Rigidbody GetRigidbody()
    {
        return rigidBody;
    }

    public void OnSpawn()
    {
        bulletDisabled = false;
        boxCollider.enabled = true;
        meshRenderer.enabled = true;

        trailRenderer.time = 0.5f;
        startPosition = transform.position;
    }

    public void OnDespawn()
    {
        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;

        trailRenderer.Clear();
    }
}

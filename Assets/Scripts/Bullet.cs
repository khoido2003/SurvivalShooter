using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rigidbody;

    [SerializeField]
    private GameObject bulletImpactVfx;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        CreateImpactBulletFx(collision);
        Destroy(gameObject);
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

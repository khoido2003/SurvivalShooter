using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    [SerializeField]
    private Transform ragdollParent;

    [SerializeField]
    private Collider[] ragdollColliders;

    [SerializeField]
    private Rigidbody[] ragdollRigidBodies;

    private void Awake()
    {
        ragdollColliders = System.Array.FindAll(
            ragdollColliders,
            c => c.GetComponent<EnemyShield>() == null
        );
        ragdollRigidBodies = GetComponentsInChildren<Rigidbody>();

        RagDollActive(false);
    }

    public void RagDollActive(bool active)
    {
        // When in ragdoll mode, disable the kinematic so the gravity pull the doll to the ground
        foreach (Rigidbody rigidbody in ragdollRigidBodies)
        {
            rigidbody.isKinematic = !active;
        }
    }

    public void ColliderActive(bool active)
    {
        foreach (Collider collider in ragdollColliders)
        {
            if (collider != null)
                collider.enabled = active;
        }
    }
}

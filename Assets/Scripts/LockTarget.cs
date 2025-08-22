using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LockTarget : MonoBehaviour
{
    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }
}

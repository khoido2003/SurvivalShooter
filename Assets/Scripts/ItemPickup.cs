using Unity.VisualScripting;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    private Weapon weapon;

    private void OnTriggerEnter(Collider collider)
    {
        collider.GetComponent<PlayerWeaponController>()?.PickUpWeapon(weapon);
    }
}

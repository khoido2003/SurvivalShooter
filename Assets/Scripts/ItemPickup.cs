using Unity.VisualScripting;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    private WeaponData weaponData;

    private void OnTriggerEnter(Collider collider)
    {
        collider.GetComponent<PlayerWeaponController>()?.PickUpWeapon(weaponData);
    }
}

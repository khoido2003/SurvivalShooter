using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    // Default speed from nass formula is derived
    private const float REFERENCE_BULLET_SPEED = 20f;

    private Animator animator;
    private PlayerInputAction playerInputAction;
    private Player player;

    [SerializeField]
    private Weapon currentWeapon;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private Transform gunPoint;

    [SerializeField]
    private Transform weaponHolder;

    [Header("Inventory")]
    [SerializeField]
    private List<Weapon> weaponSlots;

    [SerializeField]
    private int maxSlots = 2;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();
    }

    private void Start()
    {
        playerInputAction = Player.Instance.GetPlayerInputAction();
        AssignEvent();
        PrepareWeapon();
    }

    #region Slots Management

    private void DropWeapon()
    {
        if (HasOnlyOneWeapon())
        {
            return;
        }

        weaponSlots.Remove(currentWeapon);

        // Equip the first one in the backpack
        EquipedWeapon(0);
    }

    private void PrepareWeapon()
    {
        foreach (Weapon weapon in weaponSlots)
        {
            weapon.bulletsInMagazine = weapon.magazineCapacity;
        }

        EquipedWeapon(0);
    }

    private void EquipedWeapon(int i)
    {
        currentWeapon = weaponSlots[i];
        PlayWeaponEquipAnimation();
    }

    private void PlayWeaponEquipAnimation()
    {
        player.weaponVisualController.PlayWeaponEquipAnimation();
    }

    public void PickUpWeapon(Weapon newWeapon)
    {
        if (weaponSlots.Count >= maxSlots)
        {
            Debug.Log("No slots available!");
            return;
        }
        weaponSlots.Add(newWeapon);

        player.weaponVisualController.SwitchOnBackupWeaponModel();
    }
    #endregion


    private void Shoot()
    {
        if (!currentWeapon.CanShoot())
        {
            return;
        }

        // GameObject newBullet = Instantiate(
        //     bulletPrefab,
        //     gunPoint.position,
        //     Quaternion.LookRotation(gunPoint.forward)
        // );

        GameObject newBullet = ObjectPool.Instance.GetBullet();
        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        // Make sure at what ever speed, it always interact the same to the mass of the collison object
        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.linearVelocity = BulletDirection() * bulletSpeed;

        newBullet.GetComponent<Rigidbody>().linearVelocity = BulletDirection() * bulletSpeed;

        animator.SetTrigger("Fire");
    }

    public Vector3 BulletDirection()
    {
        Transform aim = player.playerAim.GetAim();

        Vector3 direction = (aim.position - gunPoint.position).normalized;

        if (!player.playerAim.CanAimPrecise() && player.playerAim.GetLockTargetTransform() == null)
        {
            direction.y = 0;
        }

        // TODO: Refactor later
        // weaponHolder.LookAt(aim);
        // gunPoint.LookAt(aim);

        return direction;
    }

    public Transform GetGunPoint()
    {
        return gunPoint;
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public Weapon GetBackupWeapon()
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon != currentWeapon)
            {
                return weapon;
            }
        }

        return null;
    }

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;

    #region Input Events
    private void AssignEvent()
    {
        playerInputAction.character.Fire.performed += (ctx) =>
        {
            Shoot();
        };
        playerInputAction.character.EquipedSlotFirst.performed += (ctx) =>
        {
            EquipedWeapon(0);
        };

        playerInputAction.character.EquipedSlotSecond.performed += (ctx) =>
        {
            EquipedWeapon(1);
        };

        playerInputAction.character.DropWeapon.performed += (ctx) =>
        {
            DropWeapon();
        };

        playerInputAction.character.Reload.performed += (ctx) =>
        {
            if (currentWeapon.CanReload())
            {
                player.weaponVisualController.PlayReloadAnimation();
            }
        };
    }
    #endregion
    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25);
    //
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawLine(gunPoint.position, gunPoint.position + BulletDirection() * 25);
    // }
}

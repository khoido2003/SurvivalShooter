using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    // Default speed from nass formula is derived
    private const float REFERENCE_BULLET_SPEED = 20f;

    private PlayerInputAction playerInputAction;
    private Player player;

    [SerializeField]
    private WeaponData weaponData;

    [SerializeField]
    private Weapon currentWeapon;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private Transform weaponHolder;

    [Header("Inventory")]
    [SerializeField]
    private List<Weapon> weaponSlots;

    [SerializeField]
    private int maxSlots = 2;

    [SerializeField]
    private bool isWeaponReady;

    private bool isShooting;

    //////////////////////////////////////////////////

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        playerInputAction = Player.Instance.GetPlayerInputAction();
        AssignEvent();
        PrepareWeapon();
    }

    private void Update()
    {
        if (isShooting)
        {
            Shoot();
        }
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
        // Create a weapon from ScriptableObject
        Weapon startingWeapon = new Weapon(weaponData);

        // Make sure we have something in slots
        weaponSlots.Clear();
        weaponSlots.Add(startingWeapon);

        // Equip it
        EquipedWeapon(0);

        // Mark weapon as ready
        SetWeaponReady(true);
    }

    private void EquipedWeapon(int i)
    {
        if (i >= weaponSlots.Count)
        {
            return;
        }

        currentWeapon = weaponSlots[i];

        StartCoroutine(PlayWeaponEquipAnimation());

        CameraManager.Instance.ChangeCamearaDistance(currentWeapon.cameraDistance);
    }

    private IEnumerator PlayWeaponEquipAnimation()
    {
        yield return null;
        player.weaponVisualController.PlayWeaponEquipAnimation();
    }

    public void PickUpWeapon(WeaponData newWeaponData)
    {
        Weapon newWeapon = new Weapon(newWeaponData);

        if (HasWeaponTypeInventory(newWeapon.weaponType) != null)
        {
            Weapon weapon = HasWeaponTypeInventory(newWeapon.weaponType);
            weapon.totalReserveAmmo += newWeapon.bulletsInMagazine;

            return;
        }

        if (weaponSlots.Count >= maxSlots)
        {
            int weaponIndex = weaponSlots.IndexOf(currentWeapon);

            player.weaponVisualController.SwitchOffWeaponModel();

            weaponSlots[weaponIndex] = newWeapon;
            EquipedWeapon(weaponIndex);
            return;
        }

        weaponSlots.Add(newWeapon);

        player.weaponVisualController.SwitchOnBackupWeaponModel();
    }

    #endregion

    public Weapon HasWeaponTypeInventory(WeaponType weaponType)
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon.weaponType == weaponType)
            {
                return weapon;
            }
        }

        return null;
    }

    private void ReloadWeapon()
    {
        SetWeaponReady(false);
        player.weaponVisualController.PlayReloadAnimation();
    }

    private IEnumerator BustFire()
    {
        SetWeaponReady(false);

        for (int i = 0; i < currentWeapon.bulletsPerShot; i++)
        {
            FireSingleBullet();

            yield return new WaitForSeconds(currentWeapon.burstFireDelay);
        }

        SetWeaponReady(true);
    }

    private void Shoot()
    {
        if (!GetIsWeaponReady())
        {
            return;
        }

        if (!currentWeapon.CanShoot())
        {
            return;
        }

        player.weaponVisualController.PlayFireAnimation();

        if (currentWeapon.shootType == ShootType.Single)
        {
            isShooting = false;
        }

        if (currentWeapon.BustActivated())
        {
            StartCoroutine(BustFire());
            return;
        }
        else
        {
            FireSingleBullet();
        }
    }

    private void FireSingleBullet()
    {
        currentWeapon.bulletsInMagazine--;

        Bullet newBullet = PoolManager.Instance.Get<Bullet>();

        newBullet.transform.position = GetGunPoint().position;
        newBullet.transform.rotation = Quaternion.LookRotation(GetGunPoint().forward);

        // Make sure at what ever speed, it always interact the same to the mass of the collison object
        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.BulletSetup(currentWeapon.gunDistance);

        // Add some bullet spread in random direction
        Vector3 randomBulletsDirection = currentWeapon.ApplySpread(BulletDirection());

        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.linearVelocity = randomBulletsDirection * bulletSpeed;
    }

    public Vector3 BulletDirection()
    {
        Transform aim = player.playerAim.GetAim();

        Vector3 direction = (aim.position - GetGunPoint().position).normalized;

        if (!player.playerAim.CanAimPrecise() && player.playerAim.GetLockTargetTransform() == null)
        {
            direction.y = 0;
        }

        return direction;
    }

    public Transform GetGunPoint()
    {
        return player.weaponVisualController.GetCurrentWeaponModel().gunPoint;
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

    public void SetWeaponReady(bool ready) => isWeaponReady = ready;

    public bool GetIsWeaponReady() => isWeaponReady;

    #region Input Events
    private void AssignEvent()
    {
        playerInputAction.character.Fire.performed += (ctx) =>
        {
            isShooting = true;
        };

        playerInputAction.character.Fire.canceled += (ctx) =>
        {
            isShooting = false;
        };

        playerInputAction.character.EquipedSlotFirst.performed += (ctx) =>
        {
            EquipedWeapon(0);
        };

        playerInputAction.character.EquipedSlotSecond.performed += (ctx) =>
        {
            EquipedWeapon(1);
        };

        playerInputAction.character.EquipedSlotThird.performed += (ctx) =>
        {
            EquipedWeapon(2);
        };

        playerInputAction.character.EquipedSlotFourth.performed += (ctx) =>
        {
            EquipedWeapon(3);
        };
        playerInputAction.character.EquipedSlotFifth.performed += (ctx) =>
        {
            EquipedWeapon(4);
        };
        playerInputAction.character.DropWeapon.performed += (ctx) =>
        {
            DropWeapon();
        };

        playerInputAction.character.ToggleWeaponMode.performed += (ctx) =>
        {
            currentWeapon.ToggleBust();
        };

        playerInputAction.character.Reload.performed += (ctx) =>
        {
            if (currentWeapon.CanReload() && GetIsWeaponReady())
            {
                ReloadWeapon();
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

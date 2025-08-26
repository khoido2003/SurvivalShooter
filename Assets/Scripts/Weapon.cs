using System;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle,
}

public enum ShootType
{
    Single,
    Auto,
}

[Serializable]
public class Weapon
{
    public WeaponType weaponType;

    public ShootType shootType;

    [Header("Shooting specifics")]
    public float fireRate = 1;
    private float lastShootTime;

    [Header("Magazine details")]
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Range(1, 2)]
    public float reloadSpeed = 1;

    [Range(1, 2)]
    public float equipSpeed = 1;

    ////////////////////////////////////////////


    public bool CanShoot()
    {
        bool isCanShoot = HaveEnoughBullets() && IsReadyToFire();

        if (isCanShoot)
        {
            bulletsInMagazine--;
        }

        return isCanShoot;
    }

    private bool IsReadyToFire()
    {
        float timeSinceLastShot = Time.time - lastShootTime;

        float cooldown = 1f / fireRate;

        bool canShoot = timeSinceLastShot >= cooldown;

        if (canShoot)
        {
            lastShootTime = Time.time;
        }

        return canShoot;
    }

    #region Reload method

    private bool HaveEnoughBullets()
    {
        return bulletsInMagazine > 0;
    }

    public bool CanReload()
    {
        if (bulletsInMagazine == magazineCapacity)
        {
            return false;
        }
        return totalReserveAmmo > 0;
    }

    public void ReloadBullets()
    {
        // // This way will return the current not used bullets back to the bag
        // totalReserveAmmo += bulletsInMagazine;

        int bulletsReload = magazineCapacity;

        if (bulletsReload > totalReserveAmmo)
        {
            bulletsReload = totalReserveAmmo;
        }
        totalReserveAmmo -= bulletsReload;
        bulletsInMagazine = bulletsReload;

        if (totalReserveAmmo < 0)
        {
            totalReserveAmmo = 0;
        }
    }
    #endregion
}

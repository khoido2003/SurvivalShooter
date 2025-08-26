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

[Serializable]
public class Weapon
{
    public WeaponType weaponType;
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Range(1, 2)]
    public float reloadSpeed = 1;

    [Range(1, 2)]
    public float equipSpeed = 1;

    public bool CanShoot()
    {
        return HaveEnoughBullets();
    }

    private bool HaveEnoughBullets()
    {
        if (bulletsInMagazine > 0)
        {
            bulletsInMagazine--;
            return true;
        }
        return false;
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
}

using System;
using UnityEngine;
using Random = UnityEngine.Random;

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

    [Header("Spread")]
    public float baseSpread = 1;
    private float currentSpread = 1;
    public float maximumSpread = 3;
    public float spreadIncreaseRate = 0.15f;
    private float lastSpreadUpdateTime;
    private float spreadCooldown = 1f;

    [Header("Shooting specifics")]
    public float fireRate = 1;
    private float lastShootTime;

    [Header("Magazine details")]
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Header("Burst Fire")]
    public int bulletsPerShot;
    public bool bustAvailable;
    public bool bustActive;
    public float burstFireDelay = .1f;

    [Range(1, 2)]
    public float reloadSpeed = 1;

    [Range(1, 2)]
    public float equipSpeed = 1;

    [Range(2, 12)]
    public float gunDistance = 4f;

    ////////////////////////////////////////////

    #region Bust Method


    public bool BustActivated()
    {
        if (weaponType == WeaponType.Shotgun)
        {
            bustActive = true;
        }

        return bustActive;
    }

    public void ToggleBust()
    {
        if (weaponType == WeaponType.Shotgun)
        {
            return;
        }

        if (!bustAvailable)
        {
            return;
        }
        bustActive = !bustActive;
    }

    #endregion


    #region Spread Method

    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        UpdateSpread();

        float randomizedValue = Random.Range(-currentSpread, currentSpread);

        Quaternion spreadRotation = Quaternion.Euler(
            randomizedValue,
            randomizedValue,
            randomizedValue
        );
        return spreadRotation * originalDirection;
    }

    private void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maximumSpread);
    }

    private void UpdateSpread()
    {
        if (Time.time > lastSpreadUpdateTime + spreadCooldown)
        {
            currentSpread = baseSpread;
        }
        else
        {
            IncreaseSpread();
        }
        lastSpreadUpdateTime = Time.time;
    }

    #endregion


    public bool CanShoot()
    {
        bool isCanShoot = HaveEnoughBullets() && IsReadyToFire();

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
        int neededBullets = magazineCapacity - bulletsInMagazine;

        if (neededBullets <= 0)
            return;

        int bulletsToLoad = Mathf.Min(neededBullets, totalReserveAmmo);

        bulletsInMagazine += bulletsToLoad;
        totalReserveAmmo -= bulletsToLoad;
    }
    #endregion
}

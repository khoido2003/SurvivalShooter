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
    public ShootType shootType;
    public WeaponType weaponType;

    [Header("Spread Shot Settings")]
    private float baseSpread = 1;
    private float currentSpread = 1;
    private float maximumSpread = 3;
    private float spreadIncreaseRate = 0.15f;
    private float lastSpreadUpdateTime;
    private float spreadCooldown = 1f;

    [Header("Regular Shot Settings")]
    public float fireRate { get; private set; }
    private float lastShootTime;
    public int bulletsPerShot { get; private set; }

    [Header("Magazine details")]
    public int bulletsInMagazine;
    public int magazineCapacity { get; private set; }
    public int totalReserveAmmo;

    [Header("Burst Shot Settings")]
    private bool bustAvailable;
    public bool bustActive;
    public float burstFireDelay { get; private set; }

    [Header("Weapon General Settings")]
    public float reloadSpeed { get; private set; }
    public float equipSpeed { get; private set; }
    public float gunDistance { get; private set; }
    public float cameraDistance { get; private set; }

    ////////////////////////////////////////////

    public Weapon(WeaponData weaponData)
    {
        shootType = weaponData.shootType;
        bulletsPerShot = weaponData.bulletsPerShot;

        fireRate = weaponData.fireRate;
        weaponType = weaponData.weaponType;
        baseSpread = weaponData.baseSpread;
        maximumSpread = weaponData.maxSpread;

        reloadSpeed = weaponData.reloadSpeed;
        equipSpeed = weaponData.equipSpeed;
        cameraDistance = weaponData.cameraDistance;
        gunDistance = weaponData.gunDistance;

        bustActive = weaponData.bustActive;
        bustAvailable = weaponData.bustAvailable;
        bulletsPerShot = weaponData.bulletsPerShot;
        burstFireDelay = weaponData.burstFireDelay;

        magazineCapacity = weaponData.magazineCapacity;
        totalReserveAmmo = weaponData.totalReserveAmmo;
        bulletsInMagazine = weaponData.bulletsInMagazine;
    }

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

        bulletsPerShot = 1;
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

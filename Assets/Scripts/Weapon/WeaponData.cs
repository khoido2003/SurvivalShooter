using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon System")]
public class WeaponData : ScriptableObject
{
    public WeaponType weaponType;
    public string weaponName;

    [Header("Regular Shot Settings")]
    public float fireRate;
    public ShootType shootType;

    [Header("Burst Shot Settings")]
    public int bulletsPerShot;
    public bool bustAvailable;
    public bool bustActive;
    public float burstFireDelay = .1f;

    [Header("Spread Shot Settings")]
    public float baseSpread;
    public float maxSpread;
    public float spreadIncreaseRate = .15f;

    [Header("Weapon General Settings")]
    [Range(1, 3)]
    public float reloadSpeed = 1;

    [Range(1, 3)]
    public float equipSpeed = 1;

    [Range(2, 12)]
    public float gunDistance = 4f;

    [Range(4, 8)]
    public float cameraDistance = 6;

    [Header("Magazine details")]
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;
}

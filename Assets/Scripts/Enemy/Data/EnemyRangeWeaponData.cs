using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Enemy Range/Weapon Data")]
public class EnemyRangeWeaponData : ScriptableObject
{
    [Header("Weapon Details")]
    public EnemyRangeWeaponModelType weaponModelType;

    public float fireRate = 1f;

    public int minBulletPerAttack = 1;
    public int maxBulletPerAttack = 1;

    public float minWeaponCooldown = 2;
    public float maxWeaponCooldown = 3;

    [Header("Bullet Details")]
    public float bulletSpeed;
    public float weaponSpread;

    public int GetBulletsPerAttack() => Random.Range(minBulletPerAttack, maxBulletPerAttack);

    public float GetWeaponCoolDown() => Random.Range(minWeaponCooldown, maxWeaponCooldown);

    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        float randomizedValue = Random.Range(-weaponSpread, weaponSpread);

        Quaternion spreadRotation = Quaternion.Euler(
            randomizedValue,
            randomizedValue,
            randomizedValue
        );
        return spreadRotation * originalDirection;
    }
}

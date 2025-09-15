using UnityEngine;

public enum HoldTypeEnemyRangeWeapon
{
    Common,
    LowHold,
    HighHold,
}

public class EnemyRangeWeaponModel : MonoBehaviour
{
    public EnemyRangeWeaponModelType weaponModelType;

    public HoldTypeEnemyRangeWeapon weaponHoldType;
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Melee Weapon Data", menuName = "Enemy Melee/Weapon Data")]
public class EnemyMeleeWeaponData : ScriptableObject
{
    public List<EnemyMeleeAttackData> attackDatas;
}

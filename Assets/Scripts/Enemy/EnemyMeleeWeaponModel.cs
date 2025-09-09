using UnityEngine;

public class EnemyMeleeWeaponModel : MonoBehaviour
{
    public EnemyMeleeWeaponModelType weaponModelType;

    public AnimatorOverrideController overrideController;

    [SerializeField]
    private GameObject[] trailEffects;

    private void Awake()
    {
        EnableTrailEffect(false);
    }

    public void EnableTrailEffect(bool enable)
    {
        foreach (var effect in trailEffects)
        {
            effect.SetActive(enable);
        }
    }
}

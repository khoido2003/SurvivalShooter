using System.Collections.Generic;
using UnityEngine;

public enum EnemyMeleeWeaponModelType
{
    OneHand,
    Throw,
}

public class EnemyVisual : MonoBehaviour
{
    [Header("Weapon Model")]
    [SerializeField]
    private EnemyMeleeWeaponModel[] weaponModels;

    [SerializeField]
    private EnemyMeleeWeaponModelType enemyWeaponModelType;

    public GameObject currentWeaponModel { get; private set; }

    [Header("Skin Color")]
    [SerializeField]
    private Texture[] colorTexture;

    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer;

    private void Start()
    {
        weaponModels = GetComponentsInChildren<EnemyMeleeWeaponModel>();

        // Change color every 0.5s
        InvokeRepeating(nameof(SetupLook), 0, 1.5f);

        InvokeRepeating(nameof(SetupRandomWeapon), 0, 1.5f);
    }

    public void SetupLook()
    {
        SetupRandomColor();
        SetupRandomWeapon();
    }

    public void SetupWeaponType(EnemyMeleeWeaponModelType type)
    {
        enemyWeaponModelType = type;
    }

    private void SetupRandomWeapon()
    {
        foreach (var weaponModels in weaponModels)
        {
            weaponModels.gameObject.SetActive(false);
        }

        List<EnemyMeleeWeaponModel> filterEnemyWeaponModels = new();

        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponModelType == enemyWeaponModelType)
            {
                filterEnemyWeaponModels.Add(weaponModel);
            }
        }

        int randomIndex = Random.Range(0, filterEnemyWeaponModels.Count);

        filterEnemyWeaponModels[randomIndex].gameObject.SetActive(true);

        currentWeaponModel = filterEnemyWeaponModels[randomIndex].gameObject;
    }

    private void SetupRandomColor()
    {
        int randomIndex = Random.Range(0, colorTexture.Length);

        Material newMaterial = new(skinnedMeshRenderer.material);

        newMaterial.mainTexture = colorTexture[randomIndex];

        skinnedMeshRenderer.material = newMaterial;
    }
}

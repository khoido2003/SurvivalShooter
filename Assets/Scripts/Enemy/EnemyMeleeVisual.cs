using System.Collections.Generic;
using UnityEngine;

public enum EnemyMeleeWeaponModelType
{
    OneHand,
    Throw,
}

public class EnemyVisual : MonoBehaviour
{
    [Header("Corruption Visuals")]
    private GameObject[] corruptionCrystalsGameObject;

    [SerializeField]
    private int corruptionAmount;

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

    private void Awake()
    {
        weaponModels = GetComponentsInChildren<EnemyMeleeWeaponModel>();

        CollectCorruptionCrystals();
    }

    private void Start()
    {
        // Change color every 0.5s
        // InvokeRepeating(nameof(SetupLook), 0, 1.5f);
    }

    public void SetupLook()
    {
        SetupRandomColor();
        SetupRandomWeapon();
        SetupRandomCorruption();
    }

    public void SetupWeaponType(EnemyMeleeWeaponModelType type)
    {
        enemyWeaponModelType = type;
    }

    private void SetupRandomCorruption()
    {
        if (corruptionCrystalsGameObject == null || corruptionCrystalsGameObject.Length == 0)
        {
            return;
        }

        foreach (GameObject crystal in corruptionCrystalsGameObject)
        {
            crystal.SetActive(false);
        }

        int amount = Mathf.Clamp(corruptionAmount, 0, corruptionCrystalsGameObject.Length);

        List<GameObject> shuffled = new(corruptionCrystalsGameObject);

        for (int i = 0; i < shuffled.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffled.Count);

            (shuffled[i], shuffled[randomIndex]) = (shuffled[randomIndex], shuffled[i]);
        }

        for (int i = 0; i < amount; i++)
        {
            shuffled[i].SetActive(true);
        }
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

    private void CollectCorruptionCrystals()
    {
        EnemyCorruptionCrystal[] corruptionCrystals =
            GetComponentsInChildren<EnemyCorruptionCrystal>();

        corruptionCrystalsGameObject = new GameObject[corruptionCrystals.Length];

        for (int i = 0; i < corruptionCrystals.Length; i++)
        {
            corruptionCrystalsGameObject[i] = corruptionCrystals[i].gameObject;
        }
    }
}

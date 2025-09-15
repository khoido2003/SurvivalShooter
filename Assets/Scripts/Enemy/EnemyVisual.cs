using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public enum EnemyMeleeWeaponModelType
{
    OneHand,
    Throw,
    UnArmed,
}

public enum EnemyRangeWeaponModelType
{
    Pistol,
    Revolver,
    Shotgun,
    AutoRifle,
    Rifle,
}

public class EnemyVisual : MonoBehaviour
{
    [Header("Corruption Visuals")]
    private GameObject[] corruptionCrystalsGameObject;

    [SerializeField]
    private int corruptionAmount;

    public GameObject currentWeaponModel { get; private set; }

    [Header("Skin Color")]
    [SerializeField]
    private Texture[] colorTexture;

    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer;

    [Header("Rig Reerences")]
    [SerializeField]
    private Transform leftHandLk;

    [SerializeField]
    private Transform leftElbowLk;

    [SerializeField]
    Rig rig;

    private void Awake() { }

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

    public void EnableWeaponTrail(bool enable)
    {
        EnemyMeleeWeaponModel enemyMeleeWeaponModel =
            currentWeaponModel.GetComponent<EnemyMeleeWeaponModel>();

        enemyMeleeWeaponModel.EnableTrailEffect(enable);
    }

    private void SetupRandomCorruption()
    {
        corruptionCrystalsGameObject = CollectCorruptionCrystals();

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
        bool isEnemyMelee = GetComponent<EnemyMelee>() != null;
        bool isEnemyRange = GetComponent<EnemyRange>() != null;

        if (isEnemyMelee)
        {
            currentWeaponModel = FindMeleeWeaponModel();
        }

        if (isEnemyRange)
        {
            currentWeaponModel = FindRangeWeaponModel();
        }

        currentWeaponModel.SetActive(true);

        // Override animator
        OverrideAnimatorController();
    }

    private GameObject FindRangeWeaponModel()
    {
        EnemyRangeWeaponModel[] weaponModels = GetComponentsInChildren<EnemyRangeWeaponModel>();

        EnemyRangeWeaponModelType weaponModelType = GetComponent<EnemyRange>().weaponModelType;

        foreach (var weaponModel in weaponModels)
        {
            weaponModel.gameObject.SetActive(false);
        }

        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponModelType == weaponModelType)
            {
                SwitchAnimationLayer((int)weaponModel.weaponHoldType);

                SetupLeftHandLk(weaponModel.leftHandTarget, weaponModel.leftElbowTarget);

                return weaponModel.gameObject;
            }
        }
        Debug.LogError("No Range Weapon Model found!");
        return null;
    }

    private GameObject FindMeleeWeaponModel()
    {
        EnemyMeleeWeaponModel[] weaponModels = GetComponentsInChildren<EnemyMeleeWeaponModel>(true);

        List<EnemyMeleeWeaponModel> filterEnemyWeaponModels = new();

        EnemyMeleeWeaponModelType enemyWeaponModelType =
            GetComponent<EnemyMelee>().enemyWeaponModelType;

        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponModelType == enemyWeaponModelType)
            {
                filterEnemyWeaponModels.Add(weaponModel);
            }
        }

        int randomIndex = Random.Range(0, filterEnemyWeaponModels.Count);

        currentWeaponModel = filterEnemyWeaponModels[randomIndex].gameObject;

        return filterEnemyWeaponModels[randomIndex].gameObject;
    }

    private void OverrideAnimatorController()
    {
        EnemyMeleeWeaponModel meleeWeaponModel =
            currentWeaponModel.GetComponent<EnemyMeleeWeaponModel>();

        if (meleeWeaponModel != null && meleeWeaponModel.overrideController != null)
        {
            GetComponentInChildren<Animator>().runtimeAnimatorController =
                meleeWeaponModel.overrideController;
        }
    }

    private void SetupRandomColor()
    {
        int randomIndex = Random.Range(0, colorTexture.Length);

        Material newMaterial = new(skinnedMeshRenderer.material);

        newMaterial.mainTexture = colorTexture[randomIndex];

        skinnedMeshRenderer.material = newMaterial;
    }

    private GameObject[] CollectCorruptionCrystals()
    {
        EnemyCorruptionCrystal[] corruptionCrystals =
            GetComponentsInChildren<EnemyCorruptionCrystal>();

        GameObject[] corruptionCrystalsGameObject = new GameObject[corruptionCrystals.Length];

        for (int i = 0; i < corruptionCrystals.Length; i++)
        {
            corruptionCrystalsGameObject[i] = corruptionCrystals[i].gameObject;
        }

        return corruptionCrystalsGameObject;
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        Animator animator = GetComponentInChildren<Animator>();

        for (int i = 2; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }
        animator.SetLayerWeight(layerIndex, 1);
    }

    public void EnableLk(bool enable)
    {
        rig.weight = enable ? 1 : 0;
    }

    private void SetupLeftHandLk(Transform leftHandTarget, Transform leftElbowTarget)
    {
        leftHandLk.localPosition = leftHandTarget.localPosition;
        leftHandLk.rotation = leftHandTarget.rotation;

        leftElbowLk.localPosition = leftElbowTarget.localPosition;
        leftElbowLk.rotation = leftElbowTarget.rotation;
    }
}

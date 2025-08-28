using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponVisualController : MonoBehaviour
{
    private Animator animator;
    private Player player;

    [SerializeField]
    private WeaponModel[] weaponModelArray;

    [SerializeField]
    private Transform leftHandLkTarget;

    [SerializeField]
    private TwoBoneIKConstraint lefthandLk;

    [SerializeField]
    private float rigWeightIncreaseRate;

    [SerializeField]
    private float leftHandLkWeightIncreaseRate;

    [SerializeField]
    private BackUpWeaponModel[] backUpWeaponModelArray;

    private bool shouldIncreaseRigWeight;

    private bool shouldIncreaseLeftHandLkWeight;

    private Rig rig;

    private void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        weaponModelArray = GetComponentsInChildren<WeaponModel>(true);
        backUpWeaponModelArray = GetComponentsInChildren<BackUpWeaponModel>(true);
    }

    private void Start()
    {
        SwitchOffBackupWeaponModel();
    }

    private void Update()
    {
        UpdateRigWeight();
        UpdateLeftHandLk();
    }

    public WeaponModel GetCurrentWeaponModel()
    {
        WeaponModel weaponModel = null;

        WeaponType weaponType = player.playerWeaponController.GetCurrentWeapon().weaponType;

        foreach (WeaponModel weaponModelItem in weaponModelArray)
        {
            if (weaponModelItem.weaponType == weaponType)
            {
                weaponModel = weaponModelItem;
            }
        }

        return weaponModel;
    }

    public void PlayFireAnimation()
    {
        animator.SetTrigger("Fire");
    }

    ////////////////////////////////////////

    #region Animation Rigging Method

    public void PlayReloadAnimation()
    {
        float reloadSpeed = player.playerWeaponController.GetCurrentWeapon().reloadSpeed;

        animator.SetTrigger("Reload");
        animator.SetFloat("ReloadSpeed", reloadSpeed);

        ReduceRigWeight();
    }

    private void UpdateRigWeight()
    {
        if (shouldIncreaseRigWeight)
        {
            rig.weight += rigWeightIncreaseRate * Time.deltaTime;

            if (rig.weight >= 1)
            {
                shouldIncreaseRigWeight = false;
            }
        }
    }

    private void UpdateLeftHandLk()
    {
        if (shouldIncreaseLeftHandLkWeight)
        {
            lefthandLk.weight += leftHandLkWeightIncreaseRate * Time.deltaTime;
        }
    }

    private void ReduceRigWeight()
    {
        rig.weight = 0.15f;
    }

    public void MaximizeRigWeight()
    {
        shouldIncreaseRigWeight = true;
    }

    public void MaximizeWeightLeftHandLk()
    {
        shouldIncreaseLeftHandLkWeight = true;
    }

    private void AttachLeftHand()
    {
        Transform targetTransform = GetCurrentWeaponModel().holdPoint;

        leftHandLkTarget.localPosition = targetTransform.localPosition;
        leftHandLkTarget.localRotation = targetTransform.localRotation;
    }

    #endregion

    /////////////////////////////////////////////////

    public void PlayWeaponEquipAnimation()
    {
        EquipType equipType = GetCurrentWeaponModel().equipType;
        lefthandLk.weight = 0;

        float equipSpeed = player.playerWeaponController.GetCurrentWeapon().equipSpeed;

        animator.SetFloat("weaponEquipType", (float)equipType);
        animator.SetTrigger("weaponEquip");
        animator.SetFloat("EquipSpeed", equipSpeed);

        ReduceRigWeight();
    }

    public void SwitchOnCurrentWeaponModel()
    {
        int animationIndex = (int)GetCurrentWeaponModel().holdType;

        SwitchAnimationLayer(animationIndex);

        SwitchOffBackupWeaponModel();
        SwitchOffWeaponModel();

        if (!player.playerWeaponController.HasOnlyOneWeapon())
        {
            SwitchOnBackupWeaponModel();
        }

        float delayBeforeShowWeapon = 0f;
        StartCoroutine(EnableGunAfterDelay(delayBeforeShowWeapon));
    }

    private IEnumerator EnableGunAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        GetCurrentWeaponModel().gameObject.SetActive(true);

        AttachLeftHand();
    }

    private void SwitchOffWeaponModel()
    {
        foreach (WeaponModel weaponTransform in weaponModelArray)
        {
            weaponTransform.gameObject.SetActive(false);
        }
    }

    private void SwitchOffBackupWeaponModel()
    {
        foreach (BackUpWeaponModel backUpWeaponModel in backUpWeaponModelArray)
        {
            backUpWeaponModel.Activate(false);
        }
    }

    public void SwitchOnBackupWeaponModel()
    {
        SwitchOffBackupWeaponModel();

        BackUpWeaponModel lowHangWeapon = null;
        BackUpWeaponModel backHangWeapon = null;
        BackUpWeaponModel sideHangWeapon = null;

        foreach (BackUpWeaponModel backUpWeaponModel in backUpWeaponModelArray)
        {
            if (
                backUpWeaponModel.weaponType
                == player.playerWeaponController.GetCurrentWeapon().weaponType
            )
            {
                continue;
            }

            if (player.playerWeaponController.HasWeaponTypeInventory(backUpWeaponModel.weaponType))
            {
                switch (backUpWeaponModel.GetHangType())
                {
                    default:
                    case HangType.LowBackHang:
                        lowHangWeapon = backUpWeaponModel;
                        break;
                    case HangType.BackHang:
                        backHangWeapon = backUpWeaponModel;
                        break;
                    case HangType.SideHang:
                        backHangWeapon = backUpWeaponModel;
                        break;
                }
            }
        }

        lowHangWeapon?.Activate(true);
        backHangWeapon?.Activate(true);
        sideHangWeapon?.Activate(true);
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 2; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }
        animator.SetLayerWeight(layerIndex, 1);
    }
}

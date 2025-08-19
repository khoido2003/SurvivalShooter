using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public enum GrabType
{
    SideGrab,
    BackGrab,
}

public class WeaponVisualController : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private Transform[] guns;

    [SerializeField]
    private Transform pistol;

    [SerializeField]
    private Transform revolver;

    [SerializeField]
    private Transform rifle;

    [SerializeField]
    private Transform autoRifle;

    [SerializeField]
    private Transform shotgun;

    [SerializeField]
    private Transform leftHandLkTarget;

    [SerializeField]
    private TwoBoneIKConstraint lefthandLk;

    [SerializeField]
    private float rigWeightIncreaseRate;

    [SerializeField]
    private float leftHandLkWeightIncreaseRate;

    private bool shouldIncreaseRigWeight;

    private bool shouldIncreaseLeftHandLkWeight;

    private bool isGrabbingWeapon;

    private Transform currentGun;

    private Rig rig;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();

        SwitchOn(pistol);
    }

    private void Update()
    {
        CheckWeaponSwitch();

        if (Input.GetKeyDown(KeyCode.R) && isGrabbingWeapon == false)
        {
            animator.SetTrigger("Reload");
            ReduceRigWeight();
        }

        UpdateRigWeight();
        UpdateLeftHandLk();
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

    private void PlayWeaponGrabAnimation(GrabType grabType)
    {
        lefthandLk.weight = 0;

        animator.SetFloat("weaponGrabType", (float)grabType);
        animator.SetTrigger("weaponGrab");

        ReduceRigWeight();
        SetBusyGrabbingWeaponTo(true);
    }

    public void SetBusyGrabbingWeaponTo(bool busy)
    {
        isGrabbingWeapon = busy;
        animator.SetBool("busyGrabbingWeapon", isGrabbingWeapon);
    }

    public void MaximizeRigWeight()
    {
        shouldIncreaseRigWeight = true;
    }

    public void MaximizeWeightLeftHandLk()
    {
        shouldIncreaseLeftHandLkWeight = true;
    }

    private void SwitchOn(Transform gunsTransform)
    {
        SwitchOffGun();
        currentGun = gunsTransform;

        float delayBeforeShowWeapon = 0.6f;
        StartCoroutine(EnableGunAfterDelay(delayBeforeShowWeapon));
    }

    private IEnumerator EnableGunAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentGun.gameObject.SetActive(true);
        AttachLeftHand();
    }

    private void SwitchOffGun()
    {
        foreach (Transform gunTransform in guns)
        {
            gunTransform.gameObject.SetActive(false);
        }
    }

    private void CheckWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(pistol);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(revolver);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn(rifle);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn(autoRifle);
            SwitchAnimationLayer(3);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOn(shotgun);
            SwitchAnimationLayer(2);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
    }

    private void AttachLeftHand()
    {
        Transform targetTransform = currentGun
            .GetComponentInChildren<LeftHandTargetTransform>()
            .transform;
        leftHandLkTarget.localPosition = targetTransform.localPosition;
        leftHandLkTarget.localRotation = targetTransform.localRotation;
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }
        animator.SetLayerWeight(layerIndex, 1);
    }
}

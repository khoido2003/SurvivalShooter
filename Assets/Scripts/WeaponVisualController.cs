using UnityEngine;
using UnityEngine.Animations.Rigging;

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
    private float rigIncreaseStep;

    private bool rigShouldBeIncreased;

    private Transform currentGun;

    private Rig rig;

    private void Start()
    {
        SwitchOn(pistol);
        animator = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
    }

    private void Update()
    {
        CheckWeaponSwitch();

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Reload");
            rig.weight = 0f;
        }

        if (rigShouldBeIncreased)
        {
            rig.weight += rigIncreaseStep * Time.deltaTime;

            if (rig.weight >= 1)
            {
                rigShouldBeIncreased = false;
            }
        }
    }

    public void ReturnRightWeightToOne()
    {
        rigShouldBeIncreased = true;
    }

    private void SwitchOn(Transform gunsTransform)
    {
        SwitchOffGun();
        gunsTransform.gameObject.SetActive(true);
        currentGun = gunsTransform;
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
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(revolver);

            SwitchAnimationLayer(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn(rifle);

            SwitchAnimationLayer(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn(autoRifle);

            SwitchAnimationLayer(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOn(shotgun);

            SwitchAnimationLayer(2);
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

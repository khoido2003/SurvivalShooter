using UnityEngine;

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

    private Transform currentGun;

    private void Start()
    {
        SwitchOn(pistol);

        animator = GetComponentInParent<Animator>();
    }

    private void Update()
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

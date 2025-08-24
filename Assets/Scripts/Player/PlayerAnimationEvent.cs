using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private WeaponVisualController weaponVisualController;
    private PlayerWeaponController playerWeaponController;

    private void Start()
    {
        weaponVisualController = GetComponentInParent<WeaponVisualController>();
        playerWeaponController = GetComponentInParent<PlayerWeaponController>();
    }

    public void ReloadIsOver()
    {
        playerWeaponController.GetCurrentWeapon().ReloadBullets();
        weaponVisualController.MaximizeRigWeight();
    }

    public void ReturnRig()
    {
        weaponVisualController.MaximizeRigWeight();
        weaponVisualController.MaximizeWeightLeftHandLk();
    }

    public void WeaponGrabIsOver()
    {
        weaponVisualController.SetBusyGrabbingWeaponTo(false);
    }
}

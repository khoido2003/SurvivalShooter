using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private WeaponVisualController weaponVisualController;

    private void Start()
    {
        weaponVisualController = GetComponentInParent<WeaponVisualController>();
    }

    public void ReloadIsOver()
    {
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

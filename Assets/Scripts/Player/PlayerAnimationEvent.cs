using System;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private WeaponVisualController weaponVisualController;
    private PlayerWeaponController playerWeaponController;

    public static PlayerAnimationEvent Instance { get; private set; }

    public event EventHandler OnSwitchOnWeaponModel;
    public event EventHandler OnWeaponGrabIsOver;

    private void Awake()
    {
        Instance = this;
    }

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

        OnWeaponGrabIsOver?.Invoke(this, EventArgs.Empty);
    }

    public void SwitchOnWeaponModel()
    {
        weaponVisualController.SwitchOnCurrentWeaponModel();

        OnSwitchOnWeaponModel?.Invoke(this, EventArgs.Empty);
    }
}

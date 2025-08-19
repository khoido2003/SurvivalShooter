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
        weaponVisualController.ReturnRightWeightToOne();
    }
}

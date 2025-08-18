using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Animator animator;
    private PlayerInputAction playerInputAction;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

        playerInputAction = Player.Instance.GetPlayerInputAction();
        AssignEvent();
    }

    private void AssignEvent()
    {
        playerInputAction.character.Fire.performed += (ctx) =>
        {
            Shoot();
        };
    }

    private void Shoot()
    {
        animator.SetTrigger("Fire");
    }
}

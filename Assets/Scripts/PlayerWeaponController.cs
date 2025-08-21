using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Animator animator;
    private PlayerInputAction playerInputAction;
    private Player player;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private Transform gunPoint;

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private Transform aim;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();
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
        GameObject newBullet = Instantiate(
            bulletPrefab,
            gunPoint.position,
            Quaternion.LookRotation(gunPoint.forward)
        );

        newBullet.GetComponent<Rigidbody>().linearVelocity = BulletDirection() * bulletSpeed;

        Destroy(newBullet, 10);

        animator.SetTrigger("Fire");
    }

    public Vector3 BulletDirection()
    {
        Vector3 direction = (aim.position - gunPoint.position).normalized;

        if (!player.playerAim.CanAimPrecise() && player.playerAim.GetLockTargetTransform() == null)
        {
            direction.y = 0;
        }

        weaponHolder.LookAt(aim);
        gunPoint.LookAt(aim);

        return direction;
    }

    public Transform GetGunPoint()
    {
        return gunPoint;
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25);
    //
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawLine(gunPoint.position, gunPoint.position + BulletDirection() * 25);
    // }
}

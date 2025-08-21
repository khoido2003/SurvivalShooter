using System;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerInputAction playerInputActions;

    private Vector2 aimInput;

    private RaycastHit lastKnownMouseHit;

    [SerializeField]
    LayerMask aimLayerMask;

    [SerializeField]
    private Transform cameraTarget;

    [SerializeField]
    private Transform aim;

    [Range(0.5f, 1f)]
    [SerializeField]
    private float minCameraDistance = 1.5f;

    [Range(1f, 1.5f)]
    [SerializeField]
    private float maxCameraDistance = 4f;

    [Range(1.5f, 3f)]
    [SerializeField]
    private float cameraSensitivity = 2.5f;

    [SerializeField]
    private bool isAimingPrecise;

    [SerializeField]
    private bool isLockingTarget;

    [SerializeField]
    private LineRenderer aimLaser;

    private void Start()
    {
        player = GetComponent<Player>();

        AssignInputEvents();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isAimingPrecise = !isAimingPrecise;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            isLockingTarget = !isLockingTarget;
        }

        UpdateAimLaser();
        UpdateAimPosition();
        UpdateCameraPosition();
    }

    private void UpdateAimLaser()
    {
        float gunDistance = 4f;
        float laserTipLength = .5f;

        Transform gunPoint = player.playerWeaponController.GetGunPoint();
        Vector3 laserDirection = player.playerWeaponController.BulletDirection();

        Vector3 endPoint = gunPoint.position + laserDirection * gunDistance;

        if (Physics.Raycast(gunPoint.position, laserDirection, out RaycastHit hit, gunDistance))
        {
            endPoint = hit.point;
            laserTipLength = 0;
        }

        aimLaser.SetPosition(0, gunPoint.position);
        aimLaser.SetPosition(1, endPoint);

        // make the tip of laser transparent(optional)
        aimLaser.SetPosition(2, endPoint + laserDirection * laserTipLength);
    }

    private void UpdateAimPosition()
    {
        aim.position = GetMouseHitInfo().point;

        Transform lockTargetTransform = GetLockTargetTransform();

        if (lockTargetTransform != null && isLockingTarget)
        {
            aim.position = lockTargetTransform.position;
            return;
        }

        if (!isAimingPrecise)
        {
            aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
        }
    }

    private void UpdateCameraPosition()
    {
        cameraTarget.position = Vector3.Lerp(
            cameraTarget.position,
            DesiredCameraPosition(),
            cameraSensitivity * Time.deltaTime
        );
    }

    private void AssignInputEvents()
    {
        playerInputActions = player.playerInputActions;

        playerInputActions.character.Aim.performed += (ctx) =>
        {
            aimInput = ctx.ReadValue<Vector2>();
        };

        playerInputActions.character.Aim.canceled += (ctx) =>
        {
            aimInput = Vector2.zero;
        };
    }

    private Vector3 DesiredCameraPosition()
    {
        float actualMaxCameraDistance =
            player.playerMovement.moveInput.y < -.5f ? minCameraDistance : maxCameraDistance;

        Vector3 mouseWorldPosition = GetMouseHitInfo().point;

        Vector3 cameraDirection = (mouseWorldPosition - transform.position).normalized;

        float distanceToMouse = Vector3.Distance(transform.position, mouseWorldPosition);

        float clampedDistance = Mathf.Clamp(
            distanceToMouse,
            minCameraDistance,
            actualMaxCameraDistance
        );

        Vector3 desiredCameraPosition = transform.position + cameraDirection * clampedDistance;

        desiredCameraPosition.y = transform.position.y + 1;

        return desiredCameraPosition;
    }

    public Transform GetLockTargetTransform()
    {
        Transform lockTargetTransform = null;
        if (GetMouseHitInfo().transform.GetComponent<LockTarget>() != null)
        {
            lockTargetTransform = GetMouseHitInfo().transform;
        }
        return lockTargetTransform;
    }

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnownMouseHit = hitInfo;
            return hitInfo;
        }

        return lastKnownMouseHit;
    }

    public bool CanAimPrecise()
    {
        if (isAimingPrecise)
        {
            return true;
        }

        return false;
    }
}

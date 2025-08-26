using System;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerInputAction playerInputActions;

    private Vector2 mouseInput;

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
        PlayerAnimationEvent.Instance.OnSwitchOnWeaponModel +=
            PlayerAnimationEvent_OnSwitchOnWeaponModel;
        PlayerAnimationEvent.Instance.OnWeaponGrabIsOver += PlayerAnimationEvent_OnWeaponGrabIsOver;

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

        UpdateAimLaserVisuals();
        UpdateAimPosition();
        UpdateCameraPosition();
    }

    private void UpdateAimLaserVisuals()
    {
        aimLaser.enabled = player.playerWeaponController.GetIsWeaponReady();

        if (aimLaser.enabled == false)
        {
            return;
        }

        WeaponModel weaponModel = player.weaponVisualController.GetCurrentWeaponModel();

        weaponModel.gunPoint.LookAt(aim);
        weaponModel.transform.LookAt(aim);

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
            if (lockTargetTransform.GetComponent<Renderer>() != null)
            {
                aim.position = lockTargetTransform.GetComponent<Renderer>().bounds.center;
            }
            aim.position = lockTargetTransform.position;

            return;
        }

        if (!isAimingPrecise)
        {
            aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
        }
    }

    private void AssignInputEvents()
    {
        playerInputActions = player.playerInputActions;

        playerInputActions.character.Aim.performed += (ctx) =>
        {
            mouseInput = ctx.ReadValue<Vector2>();
        };

        playerInputActions.character.Aim.canceled += (ctx) =>
        {
            mouseInput = Vector2.zero;
        };
    }

    #region CameraControl


    private void UpdateCameraPosition()
    {
        cameraTarget.position = Vector3.Lerp(
            cameraTarget.position,
            DesiredCameraPosition(),
            cameraSensitivity * Time.deltaTime
        );
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

    #endregion


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
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnownMouseHit = hitInfo;
            return hitInfo;
        }

        return lastKnownMouseHit;
    }

    public Transform GetAim()
    {
        return aim;
    }

    public bool CanAimPrecise()
    {
        return isAimingPrecise;
    }

    private void PlayerAnimationEvent_OnSwitchOnWeaponModel(object sender, EventArgs e)
    {
        Hide();
    }

    private void PlayerAnimationEvent_OnWeaponGrabIsOver(object sender, EventArgs e)
    {
        Show();
    }

    private void Hide()
    {
        aimLaser.gameObject.SetActive(false);
    }

    private void Show()
    {
        aimLaser.gameObject.SetActive(true);
    }
}

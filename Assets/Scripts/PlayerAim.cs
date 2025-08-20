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

    private void Start()
    {
        player = GetComponent<Player>();

        AssignInputEvents();
    }

    private void Update()
    {
        aim.position = GetMouseHitInfo().point;
        aim.position = new Vector3(aim.position.x, transform.position.y, aim.position.z);

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

        Vector3 aimDirection = (mouseWorldPosition - transform.position).normalized;

        float distanceToMouse = Vector3.Distance(transform.position, mouseWorldPosition);

        float clampedDistance = Mathf.Clamp(
            distanceToMouse,
            minCameraDistance,
            actualMaxCameraDistance
        );

        Vector3 desiredAimPosition = transform.position + aimDirection * clampedDistance;

        desiredAimPosition.y = transform.position.y + 1;

        return desiredAimPosition;
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
}

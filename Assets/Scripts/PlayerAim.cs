using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerInputAction playerInputActions;

    private Vector2 aimInput;

    [SerializeField]
    LayerMask aimLayerMask;

    [SerializeField]
    private Transform aim;

    private void Start()
    {
        player = GetComponent<Player>();

        AssignInputEvents();
    }

    private void Update()
    {
        aim.position = new Vector3(GetMousePosition().x, aim.position.y, GetMousePosition().z);
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

    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, aimLayerMask))
        {
            return hitInfo.point;
        }

        return Vector3.zero;
    }
}

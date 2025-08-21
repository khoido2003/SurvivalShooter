using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInputAction playerInputActions { get; private set; }
    public PlayerAim playerAim { get; private set; }

    public static Player Instance { get; private set; }

    public PlayerMovement playerMovement { get; private set; }
    public PlayerWeaponController playerWeaponController { get; private set; }

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputAction();
        playerAim = GetComponent<PlayerAim>();
        playerMovement = GetComponent<PlayerMovement>();
        playerWeaponController = GetComponent<PlayerWeaponController>();
    }

    public PlayerInputAction GetPlayerInputAction()
    {
        return playerInputActions;
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }
}

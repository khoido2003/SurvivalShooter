using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInputAction playerInputActions;

    public static Player Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputAction();
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

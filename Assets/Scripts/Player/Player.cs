using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInputAction playerInputActions { get; private set; }
    public PlayerAim playerAim { get; private set; }

    public static Player Instance { get; private set; }

    public PlayerMovement playerMovement { get; private set; }
    public PlayerWeaponController playerWeaponController { get; private set; }

    public WeaponVisualController weaponVisualController { get; private set; }

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputAction();
        playerAim = GetComponent<PlayerAim>();
        playerMovement = GetComponent<PlayerMovement>();
        playerWeaponController = GetComponent<PlayerWeaponController>();
        weaponVisualController = GetComponent<WeaponVisualController>();

        // StartCoroutine(TestMethod());
    }

    public PlayerInputAction GetPlayerInputAction()
    {
        return playerInputActions;
    }

    private IEnumerator TestMethod()
    {
        yield return new WaitForSeconds(2);

        Debug.Log("I have waited for 2 seconds");
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

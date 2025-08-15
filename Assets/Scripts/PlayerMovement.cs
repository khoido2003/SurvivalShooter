using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private const float GRAVITY_SCALE = 9.81f;

    private PlayerInputAction inputActions;

    private Vector2 moveInput;
    private Vector2 aimInput;

    public Vector3 moveDirection;

    private float verticalVelocity;

    [SerializeField]
    private float walkSpeed = 5;

    [SerializeField]
    private CharacterController characterController;

    private void Awake()
    {
        inputActions = new PlayerInputAction();

        inputActions.character.Movement.performed += (ctx) =>
        {
            moveInput = ctx.ReadValue<Vector2>();
        };

        inputActions.character.Movement.canceled += (ctx) =>
        {
            moveInput = Vector2.zero;
        };

        inputActions.character.Aim.performed += (ctx) =>
        {
            aimInput = ctx.ReadValue<Vector2>();
        };

        inputActions.character.Aim.canceled += (ctx) =>
        {
            aimInput = Vector2.zero;
        };
    }

    private void Update()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        ApplyGravity();

        if (moveDirection.magnitude > 0)
        {
            characterController.Move(moveDirection * Time.deltaTime * walkSpeed);
        }
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            verticalVelocity -= GRAVITY_SCALE * Time.deltaTime;

            moveDirection.y = verticalVelocity;
        }
        else
        {
            verticalVelocity = -.5f;
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}

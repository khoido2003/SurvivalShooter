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

    private Vector3 lookAtDirection;

    private Animator animator;

    [SerializeField]
    LayerMask aimLayerMask;

    [SerializeField]
    private float walkSpeed = 5;

    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private Transform aim;

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

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        ApplyMovement();
        AimTowardMouse();
        AnimatorController();
    }

    private void AimTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lookAtDirection = hitInfo.point - transform.position;
            lookAtDirection.y = 0f;
            lookAtDirection.Normalize();

            transform.forward = lookAtDirection;
            aim.position = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
        }
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

    private void AnimatorController()
    {
        float xVelocity = Vector3.Dot(moveDirection.normalized, transform.right);

        float zVelocity = Vector3.Dot(moveDirection.normalized, transform.forward);

        float dampTime = .1f;
        animator.SetFloat("xVelocity", xVelocity, dampTime, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, dampTime, Time.deltaTime);
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

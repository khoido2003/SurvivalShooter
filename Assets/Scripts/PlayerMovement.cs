using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private const float GRAVITY_SCALE = 9.81f;

    private Player player;

    private PlayerInputAction playerInputActions;

    public Vector2 moveInput { get; private set; }

    public Vector3 moveDirection;

    private float verticalVelocity;

    private Animator animator;

    private bool isRunning;
    private float speed;

    [SerializeField]
    private float walkSpeed = 1.5f;

    [SerializeField]
    private float runSpeed = 3f;

    [SerializeField]
    private float turnSpeed = 10f;

    [SerializeField]
    private CharacterController characterController;

    private void Start()
    {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        speed = walkSpeed;
        playerInputActions = Player.Instance.GetPlayerInputAction();

        AssignEvent();
    }

    private void Update()
    {
        ApplyMovement();
        ApplyRotation();
        AnimatorController();
    }

    private void AssignEvent()
    {
        playerInputActions.character.Movement.performed += (ctx) =>
        {
            moveInput = ctx.ReadValue<Vector2>();
        };

        playerInputActions.character.Movement.canceled += (ctx) =>
        {
            moveInput = Vector2.zero;
        };

        playerInputActions.character.Run.performed += (ctx) =>
        {
            isRunning = true;
            speed = runSpeed;
        };

        playerInputActions.character.Run.canceled += (ctx) =>
        {
            isRunning = false;
            speed = walkSpeed;
        };
    }

    private void ApplyRotation()
    {
        Vector3 lookAtDirection = player.playerAim.GetMouseHitInfo().point - transform.position;
        lookAtDirection.y = 0f;
        lookAtDirection.Normalize();

        Quaternion desiredRotation = Quaternion.LookRotation(lookAtDirection);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            turnSpeed * Time.deltaTime
        );
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

        bool playRunningAnimation = isRunning && moveDirection.magnitude > 0;
        animator.SetBool("isRunning", playRunningAnimation);
    }

    #region New InputSystem
    private void ApplyMovement()
    {
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        ApplyGravity();

        if (moveDirection.magnitude > 0)
        {
            characterController.Move(moveDirection * Time.deltaTime * speed);
        }
    }

    #endregion
}

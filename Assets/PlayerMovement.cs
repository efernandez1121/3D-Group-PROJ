using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimplePlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;

    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float gravity = 20f;

    [Header("Look Settings")]
    public float lookSpeed = 2f;
    public float lookXLimit = 85f;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;

    // Animator
    private Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();  // Finds the cat model animator

        if (playerCamera == null)
            playerCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleLook();
    }

    // --------------------------
    // MOVEMENT
    // --------------------------
    void HandleMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Inputs
        float inputV = Input.GetAxisRaw("Vertical");
        float inputH = Input.GetAxisRaw("Horizontal");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = (inputV != 0 || inputH != 0);

        float targetSpeed = isRunning ? runSpeed : walkSpeed;

        float yVelocity = moveDirection.y;

        moveDirection = (forward * inputV + right * inputH).normalized * targetSpeed;

        // Gravity
        moveDirection.y = yVelocity;
        if (!controller.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;
        else
            moveDirection.y = -1f;

        controller.Move(moveDirection * Time.deltaTime);

        // --------------------------
        // ANIMATIONS (SMOOTHED)
        // --------------------------
        if (anim != null)
        {
            float targetAnimSpeed = 0f;

            if (isMoving)
            {
                targetAnimSpeed = isRunning ? 1f : 0.5f;  
                // 1f = run, 0.5f = walk, 0f = idle
            }

            // Smoothly transition between idle, walk, run
            anim.SetFloat("Speed", targetAnimSpeed, 0.15f, Time.deltaTime);

            anim.SetBool("Running", isRunning);
        }
    }

    // --------------------------
    // CAMERA LOOK
    // --------------------------
    void HandleLook()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.rotation *= Quaternion.Euler(0f, Input.GetAxis("Mouse X") * lookSpeed, 0f);
    }
}

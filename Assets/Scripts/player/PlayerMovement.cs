using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimplePlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public ActionDetector detector;

    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float gravity = 20f;

    [Header("Jump Settings")]
    public float jumpPower = 6f;          // normal jump
    public float maxJumpChargeTime = 1f;  // how long you can hold space
    public float highJumpMultiplier = 2f; // how much stronger a fully charged jump is

    [Header("Look Settings")]
    public float lookSpeed = 2f;
    public float lookXLimit = 85f;

    [Header("Knockback Settings")]
    public float knockBackStrength = 100f; //how far they're pushed
    public float endknockBack = 30f; //how long it takes for the knockback to stop
    private Vector3 knockBackSpeed = Vector3.zero;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;

    // High-jump state
    private bool isChargingJump = false;
    private float jumpChargeTimer = 0f;

    // Animator
    private Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        if (playerCamera == null)
            playerCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //assign the action detector for stamina manegement
        if (detector == null)
        {
            detector = FindObjectOfType<ActionDetector>();
            if (detector == null)
            {
                Debug.LogError("No ActionDetector found in the scene!");
            }
        }
    }
    private void OnEnable()
    {
        if (PersistentPlayerData.Instance != null)
        {
            transform.position = PersistentPlayerData.Instance.savedPlayerPosition; //sets the player to the saved position
            Debug.Log($"Spawning at {transform.position}");
        }
    }
    void Update()
    {
        HandleMovementAndJump();
        HandleLook();
    }

    // --------------------------
    // MOVEMENT + HIGH JUMP
    // --------------------------
    void HandleMovementAndJump()
    {
        // Basic movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float inputV = Input.GetAxisRaw("Vertical");
        float inputH = Input.GetAxisRaw("Horizontal");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = (inputV != 0 || inputH != 0);

        //for stamina
        detector.isWalking = isMoving;
        detector.isRunning = isRunning && isMoving;

        float targetSpeed = isRunning ? runSpeed : walkSpeed;

        // Keep previous Y velocity while we recalc horizontal
        float movementDirectionY = moveDirection.y;

        // Horizontal move
        moveDirection = (forward * inputV + right * inputH).normalized * targetSpeed;


        // ---------- HIGH JUMP LOGIC ----------
        if (controller.isGrounded)
        {
            // Start charging on first press
            if (Input.GetButtonDown("Jump")) // "Jump" axis is mapped to Space by default
            {
                isChargingJump = true;
                jumpChargeTimer = 0f;
            }

            // While holding, charge up
            if (isChargingJump && Input.GetButton("Jump"))
            {
                jumpChargeTimer += Time.deltaTime;
                jumpChargeTimer = Mathf.Min(jumpChargeTimer, maxJumpChargeTime);
            }

            // On release, perform jump
            if (isChargingJump && Input.GetButtonUp("Jump"))
            {
                float charge01 = Mathf.Clamp01(jumpChargeTimer / maxJumpChargeTime);
                float effectiveJump = jumpPower * Mathf.Lerp(1f, highJumpMultiplier, charge01);
                movementDirectionY = effectiveJump;
                isChargingJump = false;
            }
            else if (!isChargingJump)
            {
                // Keep us snapped to ground
                movementDirectionY = -1f;
            }
        }
        else
        {
            // In air: stop charging and let gravity act
            isChargingJump = false;
        }

        // Apply vertical velocity & gravity
        moveDirection.y = movementDirectionY;

        if (!controller.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Combine normal movement with potential knockback
        Vector3 totalMovement = moveDirection + knockBackSpeed;

        // Finally move the controller
        controller.Move(totalMovement * Time.deltaTime);

        // --------------------------
        // ANIMATIONS
        // --------------------------
        if (anim != null)
        {
            float targetAnimSpeed = 0f;

            if (isMoving && controller.isGrounded)   // only walk/run on ground
                targetAnimSpeed = isRunning ? 1f : 0.5f;

            // Smooth blend idle <-> walk <-> run
            anim.SetFloat("Speed", targetAnimSpeed, 0.15f, Time.deltaTime);
            anim.SetBool("Running", isRunning);
        }
        // ---------- KNOCKBACK FADE OUT ----------
        if (knockBackSpeed.sqrMagnitude > 0.001f)
        {
            knockBackSpeed = Vector3.Lerp(knockBackSpeed, Vector3.zero, endknockBack * Time.deltaTime);
        }
    }

    // --------------------------
    // APPLYING KNOCKBACK 
    // --------------------------
    public void KnockBack()
    {
        //get direction the player was coming from
        Vector3 xMove = new Vector3(moveDirection.x, 0f, moveDirection.z);

        if (xMove.sqrMagnitude < 0.001f)
        {
            //if not moving, uses the direction the player is facing
            xMove = transform.forward;
        }
        Vector3 knockBackDir = -xMove.normalized;
        knockBackSpeed = knockBackDir * knockBackStrength;
    }

    // --------------------------
    // GET CURRENT POSITION
    // --------------------------
    public Vector3 CurrPosition()
    {
        Vector3 currPos = transform.position;
        return currPos;
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
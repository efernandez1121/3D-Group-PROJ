using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;

    [Header("Movement")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    [Header("Crouch")]
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;
    public KeyCode crouchKey = KeyCode.R;

    [Header("Push / Pull")]
    [Tooltip("Layer(s) of objects you can grab/push. Create a 'Pushable' layer and assign it to boxes, etc.")]
    public LayerMask pushableMask;
    [Tooltip("Max distance for raycast to start a grab.")]
    public float interactDistance = 3f;
    [Tooltip("Where a grabbed object tries to stay relative to the camera.")]
    public float holdDistance = 2f;
    [Tooltip("Spring force that pulls the grabbed object toward the hold point.")]
    public float pullForce = 60f;
    [Tooltip("Damping to reduce oscillation of the grabbed object.")]
    public float pullDamping = 8f;
    [Tooltip("Max mass you are allowed to grab.")]
    public float maxGrabMass = 50f;
    [Tooltip("How strongly you shove rigidbodies when you run into them.")]
    public float pushPower = 2.0f;
    [Tooltip("Key to grab/release objects.")]
    public KeyCode interactKey = KeyCode.E;
    [Tooltip("Secondary release key (e.g., right mouse).")]
    public KeyCode altReleaseKey = KeyCode.Mouse1;

    [Header("High Jump (Charge)")]
    [Tooltip("Max time (seconds) you can charge while holding Space on ground.")]
    public float maxJumpChargeTime = 1.0f;
    [Tooltip("At full charge, jump power is multiplied by this.")]
    public float highJumpMultiplier = 1.8f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private bool canMove = true;

    // Push / Pull state
    private Rigidbody grabbedRb;
    private Transform holdPoint;

    // Jump charge state
    private float jumpChargeTimer = 0f;
    private bool isChargingJump = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // Create a hold point in front of camera if not set
        GameObject hold = new GameObject("HoldPoint");
        hold.transform.SetParent(playerCamera.transform);
        hold.transform.localPosition = new Vector3(0, 0, holdDistance);
        hold.transform.localRotation = Quaternion.identity;
        holdPoint = hold.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // --- Movement input ---
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float baseSpeed = isRunning ? runSpeed : walkSpeed;

        float curSpeedX = canMove ? baseSpeed * Input.GetAxis("Vertical") : 0f;
        float curSpeedY = canMove ? baseSpeed * Input.GetAxis("Horizontal") : 0f;

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // --- High jump (charge on ground, release to jump) ---
        if (characterController.isGrounded)
        {
            // Start or continue charging when holding Jump on ground
            if (Input.GetButtonDown("Jump"))
            {
                isChargingJump = true;
                jumpChargeTimer = 0f;
            }
            if (isChargingJump && Input.GetButton("Jump"))
            {
                jumpChargeTimer += Time.deltaTime;
                jumpChargeTimer = Mathf.Min(jumpChargeTimer, maxJumpChargeTime);
            }

            // Release to jump
            if (isChargingJump && Input.GetButtonUp("Jump"))
            {
                float charge01 = Mathf.Clamp01(jumpChargeTimer / maxJumpChargeTime);
                float effectiveJump = jumpPower * Mathf.Lerp(1f, highJumpMultiplier, charge01);
                moveDirection.y = effectiveJump;
                isChargingJump = false;
            }
            else
            {
                // If we never released (e.g., not pressing jump), keep last vertical vel
                moveDirection.y = movementDirectionY;
            }
        }
        else
        {
            isChargingJump = false; // lose charge in air
            moveDirection.y = movementDirectionY;
        }

        // Extra gravity while in air
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // --- Crouch toggle/hold ---
        if (Input.GetKey(crouchKey) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        // --- Apply motion ---
        characterController.Move(moveDirection * Time.deltaTime);

        // --- Look ---
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        // --- Push / Pull: start/stop grab ---
        if (Input.GetKeyDown(interactKey))
        {
            if (grabbedRb == null)
                TryGrab();
            else
                ReleaseGrab();
        }
        if (Input.GetKeyDown(altReleaseKey) && grabbedRb != null)
        {
            ReleaseGrab();
        }

        // --- While grabbing, pull the object toward hold point ---
        if (grabbedRb != null)
        {
            UpdateGrabbed();
        }
    }

    // Called automatically when CharacterController bumps something.
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Passive pushing: shove rigidbodies when you walk into them
        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb == null || rb.isKinematic) return;

        // Never push objects below us (e.g., standing on them)
        if (hit.moveDirection.y < -0.3f) return;

        // Apply a velocity in the horizontal move direction
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        rb.velocity = pushDir * pushPower;
    }

    // --- Push/Pull helpers ---
    void TryGrab()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, pushableMask, QueryTriggerInteraction.Ignore))
        {
            Rigidbody rb = hit.rigidbody;
            if (rb != null && !rb.isKinematic && rb.mass <= maxGrabMass)
            {
                grabbedRb = rb;
                grabbedRb.useGravity = true; // keep gravity so it feels natural
                grabbedRb.drag = 0.5f;       // a bit of drag helps stability
            }
        }
    }

    void ReleaseGrab()
    {
        if (grabbedRb == null) return;
        grabbedRb.drag = 0f;
        grabbedRb = null;
    }

    void UpdateGrabbed()
    {
        // Desired position in front of camera
        Vector3 targetPos = holdPoint.position;
        Vector3 toTarget = targetPos - grabbedRb.worldCenterOfMass;

        // Spring toward hold point with damping to avoid jitter
        Vector3 desiredAccel = toTarget * pullForce - grabbedRb.velocity * pullDamping;
        grabbedRb.AddForce(desiredAccel, ForceMode.Acceleration);

        // Optional: keep object roughly facing forward (gentle)
        grabbedRb.AddTorque(-grabbedRb.angularVelocity * 2f, ForceMode.Acceleration);

        // If object gets too far (line broken), auto release
        float breakDistance = Mathf.Max(holdDistance * 3f, 6f);
        if (toTarget.sqrMagnitude > breakDistance * breakDistance)
        {
            ReleaseGrab();
        }
    }
}

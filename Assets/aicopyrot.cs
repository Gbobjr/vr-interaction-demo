using UnityEngine;

public class DualJoystickCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform movementJoystickSource;    // Joystick controlling movement direction
    public float movementSpeed = 10f;
    public float movementDeadzone = 0.1f;
    public float movementAccelerationRate = 5f;
    public float movementDecelerationRate = 5f;

    [Header("Rotation Settings")]
    public Transform rotationJoystickSource;    // Joystick controlling rotation direction
    public float rotationSpeed = 180f;           // Max degrees per second rotation
    public float rotationDeadzone = 0.1f;
    public float rotationAccelerationRate = 5f;
    public float rotationDecelerationRate = 5f;
    private Quaternion initialLocalRotation;

    void Start()
    {
        initialLocalRotation = rotationJoystickSource.localRotation;
    }


    [Header("References")]
    public Rigidbody targetRigidbody;
    public Animator animator;

    // Internal state
    private Vector3 currentMoveVelocity = Vector3.zero;
    private float currentAngularVelocity = 0f; // degrees per second

    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
        UpdateAnimator();
    }

    void HandleMovement()
    {
        // Get movement input from joystick
        Vector3 moveDir = movementJoystickSource.forward;
        moveDir.y = 0f;

        float moveMagnitude = moveDir.magnitude;
        Vector3 targetVelocity = Vector3.zero;

        if (moveMagnitude >= movementDeadzone)
        {
            targetVelocity = moveDir * movementSpeed;
        }

        float moveRate = (moveMagnitude >= movementDeadzone) ? movementAccelerationRate : movementDecelerationRate;
        currentMoveVelocity = Vector3.Lerp(currentMoveVelocity, targetVelocity, Time.fixedDeltaTime * moveRate);

        Vector3 currentVel = targetRigidbody.linearVelocity;
        targetRigidbody.linearVelocity = new Vector3(
            currentMoveVelocity.x,
            currentVel.y,       // preserve vertical velocity (gravity, jump, etc.)
            currentMoveVelocity.z
        );
    }

    void HandleRotation()
    {
        // Get the joystick rotation relative to the character root
        Quaternion relativeRotation = Quaternion.Inverse(transform.rotation) * rotationJoystickSource.rotation;
        
        Vector3 relativeEuler = relativeRotation.eulerAngles;

        // Convert Euler angles from 0-360 to -180 to 180 for the X axis
        float tilt = relativeEuler.z;
        if (tilt > 180f)
            tilt -= 360f;

        // Clamp the tilt to your max expected joystick angle (±90 degrees)
        float maxTiltAngle = 90f;
        float clampedTilt = Mathf.Clamp(tilt, -maxTiltAngle, maxTiltAngle);

        // Normalize to -1 to 1
        float inputX = clampedTilt / maxTiltAngle;

        // Deadzone check
        if (Mathf.Abs(inputX) < rotationDeadzone)
            inputX = 0f;

        // Calculate target angular velocity (degrees per second)
        float targetAngularVelocity = inputX * rotationSpeed;

        // Smoothly move current angular velocity towards target
        float rotRate = (Mathf.Abs(inputX) >= rotationDeadzone) ? rotationAccelerationRate : rotationDecelerationRate;
        currentAngularVelocity = Mathf.MoveTowards(currentAngularVelocity, targetAngularVelocity, rotRate * rotationSpeed * Time.fixedDeltaTime);

        // Apply angular velocity on Y axis (converted to radians)
        targetRigidbody.angularVelocity = new Vector3(0f, currentAngularVelocity * Mathf.Deg2Rad, 0f);
    }


    void UpdateAnimator()
    {
        if (animator == null)
            return;

        // Calculate movement speed and direction for animator blend
        float moveSpeed = currentMoveVelocity.magnitude;
        float blendValue = 0f;

        if (moveSpeed > 0.01f)
        {
            Vector3 velocityDir = currentMoveVelocity.normalized;
            float forwardDot = Vector3.Dot(transform.forward, velocityDir);
            blendValue = moveSpeed * Mathf.Sign(forwardDot);
        }

        animator.SetFloat("Blend", blendValue);

        // Optional: You can add rotation speed parameter to animator here if desired
        // animator.SetFloat("RotationSpeed", Mathf.Abs(currentAngularVelocity) / rotationSpeed);
    }
}

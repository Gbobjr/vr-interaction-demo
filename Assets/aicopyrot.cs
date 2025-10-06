using UnityEngine;

public class ApplyRotationAsVelocity : MonoBehaviour
{
    public Transform sourceObject;              // Object that defines movement direction (e.g., player controller)
    public Rigidbody targetRigidbody;           // Object that moves
    public float movementSpeed = 10f;           // Movement speed
    [Tooltip("The maximum speed the character can rotate at")]
    public float maxAngularVelocity = 180f;
    [Tooltip("The force applied to accelerate the rotation")]
    public float torqueStrength = 50f;
    [Tooltip("The damping to reduce jitter when controller is still")]
    public float damping = 5f;
    public Rigidbody JoyHeadRotate;
    public Animator animator;                   // Optional: animation blend tree

    void FixedUpdate()
    {
        if (JoyHeadRotate != null && targetRigidbody != null)
        {
            // Get the current and target rotations
            Quaternion currentRotation = targetRigidbody.rotation;
            Quaternion targetRotation = JoyHeadRotate.transform.rotation;

            // Calculate the difference in rotation
            Quaternion rotationDelta = Quaternion.Inverse(currentRotation) * targetRotation;
            rotationDelta.ToAngleAxis(out float angleInDegrees, out Vector3 rotationAxis);

            // Ensure the rotation axis is normalized
            //if (rotationAxis == Vector3.zero) return;
            //rotationAxis.Normalize();

            // Apply torque to rotate the rigidbody
            targetRigidbody.AddTorque(rotationAxis * angleInDegrees * torqueStrength * Time.fixedDeltaTime);

            // Reduce jitter by applying damping when the controller is not moving
            targetRigidbody.angularVelocity *= Mathf.Clamp01(1 - damping * Time.fixedDeltaTime);

            // Limit the maximum angular velocity
            if (targetRigidbody.angularVelocity.magnitude > maxAngularVelocity)
            {
                targetRigidbody.angularVelocity = targetRigidbody.angularVelocity.normalized * maxAngularVelocity;
            }
            if (sourceObject == null || targetRigidbody == null)
            {
                Debug.LogWarning("Source Object or Target Rigidbody not assigned!");
                return;
            }

            // Use the source object's forward direction (rotates with player/controller)
            Vector3 direction = sourceObject.forward;
            direction.y = 0f; // Flatten to XZ plane
                              //direction.Normalize();

            // Preserve vertical movement (jumping, gravity, etc.)
            Vector3 currentVelocity = targetRigidbody.linearVelocity;

            // Apply new velocity in forward direction
            Vector3 newVelocity = new Vector3(
                direction.x * movementSpeed,
                currentVelocity.y,
                direction.z * movementSpeed
            );

            targetRigidbody.linearVelocity = newVelocity;

            // Debug: Draw the direction ray in Scene view
            //Debug.DrawRay(sourceObject.position, direction * 2f, Color.green);

            // Optional: Update animator blend parameter
            if (animator != null)
            {
                float speed = new Vector3(newVelocity.x, 0, newVelocity.z).magnitude;
                animator.SetFloat("Blend", speed);
            }
        }
    }
}

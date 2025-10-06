using UnityEngine;

public class ApplyRotationAsVelocity : MonoBehaviour
{
    public Transform sourceObject; // The object whose rotation will be used
    public Rigidbody targetRigidbody; // The object to which velocity will be applied
    public Rigidbody JoyHeadRotate; // A separate Rigidbody for rotation
    public float movementSpeed = 10f; // The speed at which the target object will move
    public Animator animator; // A separate Rigidbody for rotation

    void FixedUpdate()
    {
        // Apply the rotation from the joy head.
        if (JoyHeadRotate != null)
        {
            targetRigidbody.transform.rotation = JoyHeadRotate.transform.rotation;
        }

        if (sourceObject == null || targetRigidbody == null)
        {
            Debug.LogWarning("Source Object or Target Rigidbody not assigned!");
            return;
        }

        // Get the forward direction based on the source object's rotation
        Vector3 direction = sourceObject.forward;

        // Ensure the direction vector is flat on the XZ plane to prevent
        // the forward rotation from affecting vertical movement
        direction.y = 0;
        // Normalize the vector to ensure consistent speed

        // Get the current velocity of the target Rigidbody
        Vector3 currentVelocity = targetRigidbody.linearVelocity;

        // Create the new velocity vector, preserving the existing Y velocity
        // The movement is applied on the XZ plane.
        Vector3 newVelocity = new Vector3(
            -direction.x * movementSpeed,
            currentVelocity.y, // Preserve the existing Y velocity
            -direction.z * movementSpeed
        );

        // Apply the new velocity to the target object's Rigidbody
        targetRigidbody.linearVelocity = newVelocity;

        // Update animator parameter based on velocity
        Vector3 horizontalVelocity = new Vector3(newVelocity.x, 0, newVelocity.z);
        float speed = horizontalVelocity.magnitude;

        if (animator != null)
        {
            animator.SetFloat("Blend",speed);
        }

        
        print(speed);
    }
}

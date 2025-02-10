using UnityEngine;

// This script makes P3 rotate automatically between -90 and +90 degrees,
// returning to 0 degrees at the end of each cycle, with a 1-second pause between cycles.
public class Scenario1 : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 50f;

    // Delay duration at the end of each cycle (in seconds)
    public float delay = 1f;

    // Target angle for rotation (initially set to -90 degrees)
    private float targetAngle = -90f;

    // Boolean to determine if the system is in a paused state
    private bool isPaused = false;

    void Update()
    {
        // If the system is paused, do nothing
        if (isPaused) return;

        // Get the current local rotation angle on the x-axis
        float currentAngle = transform.localEulerAngles.x;

        // Normalize the angle to be between -180 and +180
        if (currentAngle > 180) currentAngle -= 360;

        // Check if the target angle is reached
        if (Mathf.Abs(currentAngle - targetAngle) < 0.1f)
        {
            // If the target angle is reached:
            // - Pause the rotation
            // - Change direction after the specified delay
            isPaused = true;
            Invoke(nameof(ChangeDirection), delay);
            return;
        }

        // Rotate towards the target angle
        float step = rotationSpeed * Time.deltaTime; // Calculate the rotation step
        float newAngle = Mathf.MoveTowards(currentAngle, targetAngle, step); // Gradually move towards the target
        transform.localEulerAngles = new Vector3(newAngle, 0f, 0f); // Apply the new angle
    }

    // Function to change the rotation direction
    private void ChangeDirection()
    {
        // Exit the paused state
        isPaused = false;

        // Update the target angle based on the current state
        if (targetAngle == -90f)
        {
            targetAngle = 90f; // If currently at -90 degrees, set the target to +90 degrees
        }
        else if (targetAngle == 90f)
        {
            targetAngle = 0f; // If currently at +90 degrees, set the target to 0 degrees
        }
        else
        {
            targetAngle = -90f; // If currently at 0 degrees, restart the cycle by setting the target to -90 degrees
        }
    }
}

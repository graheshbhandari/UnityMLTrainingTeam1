using UnityEngine;

// This script rotates Pivot-P3 by -90 degrees when the "Down" key is pressed.
public class Down : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 50f;

    // Target angle for rotation
    private float targetAngle = 0f;

    // Boolean to track if rotation is in progress
    private bool isRotating = false;

    void Update()
    {
        // Check if "down" key is pressed
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isRotating)
        {
            // Set the new target angle (-90 degrees relative to current angle)
            targetAngle = transform.localEulerAngles.x - 90f;

            // Normalize the angle to stay between 0 and 360
            if (targetAngle >= 360f)
                targetAngle -= 360f;

            // Start rotation
            isRotating = true;
        }

        // Perform rotation if rotation is in progress
        if (isRotating)
        {
            // Get the current angle
            float currentAngle = transform.localEulerAngles.x;

            // Calculate the step for rotation
            float step = rotationSpeed * Time.deltaTime;

            // Move towards the target angle
            float newAngle = Mathf.MoveTowards(currentAngle, targetAngle, step);
            transform.localEulerAngles = new Vector3(newAngle, 0f, 0f);

            // Stop rotation when target angle is reached
            if (Mathf.Abs(newAngle - targetAngle) < 0.1f)
            {
                isRotating = false;
            }
        }
    }
}

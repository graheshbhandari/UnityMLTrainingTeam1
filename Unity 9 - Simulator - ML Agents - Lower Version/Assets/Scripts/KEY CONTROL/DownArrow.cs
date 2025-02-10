using UnityEngine;
using System.Collections;

public class DownArrow : MonoBehaviour
{
    // Robots and Pivots
    public GameObject p1A, m1A, pivotP2A, p2A, m2A, pivotP3A, p3A; // Robot A parts
    public GameObject p1B, m1B, pivotP2B, p2B, m2B, pivotP3B, p3B; // Robot B parts

    // Rotation speed
    public float rotationSpeed = 50f; // 50 degrees per second

    // Processing control and counter
    public bool isProcessing = false;
    private int sequenceCounter = 0; // Sequence counter

    private void Update()
    {
        // Start the process when the DownArrow key is pressed
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isProcessing)
        {
            if (GameManager.Instance.simulationType == GameManager.SimulationType.Physical ||
                GameManager.Instance.simulationType == GameManager.SimulationType.Both)
            {
                this.SendCommand("south"); //Send the "south" command
            }
            StartCoroutine(ProcessSequence());
        }

        // Decrease counter when the LeftArrow key is pressed
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !isProcessing)
        {
            DecreaseSequenceCounter(); // Counter decrement function
        }

        // Decrease counter when the RightArrow key is pressed
        if (Input.GetKeyDown(KeyCode.RightArrow) && !isProcessing)
        {
            DecreaseSequenceCounter(); // Counter decrement function
        }

        // Decrease counter by 1 when the UpArrow key is pressed
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isProcessing)
        {
            DecreaseSequenceCounter(); // Calls the counter decrement function
        }
    }

    /// <summary>
    /// Decreases the counter value by one.
    /// </summary>
    public void SendCommand(string direction)
    {
        // Send your command to the Python server or whatever system you're using to receive commands.
        Debug.Log("Sending Command: " + direction);
        // You can replace the Debug.Log with your actual method for sending commands
        // Example: PythonCommandSender.SendCommand(direction);
    }

    /// <summary>
    /// Belirtilen işlemleri sırasıyla yapar.
    /// </summary>
    public IEnumerator ProcessSequence()
    {
        isProcessing = true;

        // Perform operations based on the counter sequence
        if (sequenceCounter % 2 == 0) // Even number: Parent-Child 1 → Parent-Child 2
        {
            Debug.Log("Pattern 1 is running.");

            // 1. Set up the first Parent-Child structure
            SetParentRelationships1();
            Debug.Log("First Parent-Child structure established.");

            // 2. Rotate pivots -90 degrees
            yield return StartCoroutine(RotatePivots(-90f));
            Debug.Log("Pivots rotated -90 degrees (First Structure).");

            // 3. Reset Parent-Child relationships
            ResetParentRelationships();
            Debug.Log("Parent-Child structure reset (Transition to Second Structure).");

            // 4. Set up the second Parent-Child structure
            SetParentRelationships2();
            Debug.Log("Second Parent-Child structure established.");

            // 5. Rotate pivots -90 degrees again
            yield return StartCoroutine(RotatePivots(-90f));
            Debug.Log("Pivots rotated -90 degrees (Second Structure).");
        }
        else // Odd number: Parent-Child 2 → Parent-Child 1
        {
            Debug.Log("Pattern 2 is running.");

            // 1. Set up the second Parent-Child structure
            SetParentRelationships2();
            Debug.Log("Second Parent-Child structure established.");

            // 2. Rotate pivots -90 degrees
            yield return StartCoroutine(RotatePivots(-90f));
            Debug.Log("Pivots rotated -90 degrees (Second Structure).");

            // 3. Reset Parent-Child relationships
            ResetParentRelationships();
            Debug.Log("Parent-Child structure reset (Transition to First Structure).");

            // 4. Set up the first Parent-Child structure
            SetParentRelationships1();
            Debug.Log("First Parent-Child structure established.");

            // 5. Rotate pivots -90 degrees again
            yield return StartCoroutine(RotatePivots(-90f));
            Debug.Log("Pivots rotated -90 degrees (First Structure).");
        }

        // Increment the counter, so the order changes the next time the DownArrow key is pressed
        sequenceCounter++;

        isProcessing = false;
    }

    /// <summary>
    /// Smoothly rotates the pivots.
    /// </summary>
    /// <param name="targetAngle">Total angle to be rotated.</param>

    private void DecreaseSequenceCounter()
    {
        sequenceCounter--; // Decrease the counter by one
        Debug.Log("Counter Value: " + sequenceCounter);
    }

    /// <summary>
    /// Performs the specified operations sequentially.
    /// </summary>
    private IEnumerator RotatePivots(float targetAngle)
    {
        float totalRotation = 0f;

        while (totalRotation < Mathf.Abs(targetAngle))
        {
            float step = rotationSpeed * Time.deltaTime;
            if (totalRotation + step > Mathf.Abs(targetAngle))
            {
                step = Mathf.Abs(targetAngle) - totalRotation; // Prevent exceeding the target
            }

            // Rotate pivots
            float rotationDirection = targetAngle > 0 ? 1f : -1f;
            pivotP3A.transform.Rotate(Vector3.right, step * rotationDirection);
            pivotP3B.transform.Rotate(-Vector3.right, step * rotationDirection);

            totalRotation += step;

            yield return null; // Wait one frame
        }
    }

    /// <summary>
    /// Resets Parent-Child relationships.
    /// </summary>
    private void ResetParentRelationships()
    {
        m1A.transform.SetParent(null);
        pivotP2A.transform.SetParent(null);
        p2A.transform.SetParent(null);
        m2A.transform.SetParent(null);
        pivotP3A.transform.SetParent(null);
        p3A.transform.SetParent(null);

        pivotP3B.transform.SetParent(null);
        p3B.transform.SetParent(null);
        m2B.transform.SetParent(null);
        pivotP2B.transform.SetParent(null);
        p2B.transform.SetParent(null);
        m1B.transform.SetParent(null);
        p1B.transform.SetParent(null);
    }

    /// <summary>
    /// Establishes the first Parent-Child structure.
    /// </summary>
    private void SetParentRelationships1()
    {
        m1A.transform.SetParent(p1A.transform);
        pivotP2A.transform.SetParent(m1A.transform);
        p2A.transform.SetParent(pivotP2A.transform);
        m2A.transform.SetParent(p2A.transform);
        pivotP3A.transform.SetParent(m2A.transform);
        p3A.transform.SetParent(pivotP3A.transform);

        p3B.transform.SetParent(p3A.transform);

        pivotP3B.transform.SetParent(p3B.transform);
        m2B.transform.SetParent(pivotP3B.transform);
        p2B.transform.SetParent(m2B.transform);
        pivotP2B.transform.SetParent(p2B.transform);
        m1B.transform.SetParent(pivotP2B.transform);
        p1B.transform.SetParent(m1B.transform);
    }

    /// <summary>
    /// Establishes the second Parent-Child structure.
    /// </summary>
    private void SetParentRelationships2()
    {
        m1B.transform.SetParent(p1B.transform);
        pivotP2B.transform.SetParent(m1B.transform);
        p2B.transform.SetParent(pivotP2B.transform);
        m2B.transform.SetParent(p2B.transform);
        pivotP3B.transform.SetParent(m2B.transform);
        p3B.transform.SetParent(pivotP3B.transform);

        p3A.transform.SetParent(p3B.transform);

        pivotP3A.transform.SetParent(p3A.transform);
        m2A.transform.SetParent(pivotP3A.transform);
        p2A.transform.SetParent(m2A.transform);
        pivotP2A.transform.SetParent(p2A.transform);
        m1A.transform.SetParent(pivotP2A.transform);
        p1A.transform.SetParent(m1A.transform);
    }

}

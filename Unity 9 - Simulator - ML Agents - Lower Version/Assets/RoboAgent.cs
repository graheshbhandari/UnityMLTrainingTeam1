using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RoboAgent : Agent
{
    public UpArrow upArrow;
    public DownArrow downArrow;
    public RightArrow rightArrow;
    public LeftArrow leftArrow;

    public Transform Target; // Reference to the target in the scene

    private float moveDelay = 1f; // Delay between actions
    private Coroutine currentCoroutine = null; // Track running coroutine


    void Start()
    {
        upArrow = GetComponent<UpArrow>();
        downArrow = GetComponent<DownArrow>();
        rightArrow = GetComponent<RightArrow>();
        leftArrow = GetComponent<LeftArrow>();
    }

    private void ResetTargetRandomPosition()
    {
        Target.localPosition = new Vector3(Random.Range(-500f, 500f), 0f, Random.Range(-500f, 500f));
    }

    public override void OnEpisodeBegin()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        ResetTargetRandomPosition();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(Target.position);
        sensor.AddObservation(Vector3.Distance(transform.position, Target.position));
    }


    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (currentCoroutine != null) return; // Block new actions until the current one finishes

        int action = actionBuffers.DiscreteActions[0];

        // Prevent array index errors by ensuring "action" is within range (0-4)
        if (action < 0 || action > 4) return;

        switch (action)
        {
            case 1:
                if (!upArrow.isProcessing)
                    currentCoroutine = StartCoroutine(ExecuteMovement(upArrow, "north"));
                break;

            case 2:
                if (!downArrow.isProcessing)
                    currentCoroutine = StartCoroutine(ExecuteMovement(downArrow, "south"));
                break;

            case 3:
                if (!rightArrow.isProcessing)
                    currentCoroutine = StartCoroutine(ExecuteMovement(rightArrow, "east"));
                break;

            case 4:
                if (!leftArrow.isProcessing)
                    currentCoroutine = StartCoroutine(ExecuteMovement(leftArrow, "west"));
                break;

            default:
                return; // No action
        }

        float distanceToTarget = Vector3.Distance(transform.position, Target.position);

        if (distanceToTarget < 1.5f)
        {
            AddReward(1.0f);
            EndEpisode();
        }
        else
        {
            AddReward(-0.01f);
        }

        if (distanceToTarget > 10f)
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }

    private IEnumerator ExecuteMovement(dynamic arrow, string command)
    {
        if (arrow is UpArrow up)
        {
            up.SendCommand(command);
            yield return StartCoroutine(up.ProcessSequence());
        }
        else if (arrow is DownArrow down)
        {
            down.SendCommand(command);
            yield return StartCoroutine(down.ProcessSequence());
        }
        else if (arrow is RightArrow right)
        {
            right.SendCommand(command);
            yield return StartCoroutine(right.ProcessSequence());
        }
        else if (arrow is LeftArrow left)
        {
            left.SendCommand(command);
            yield return StartCoroutine(left.ProcessSequence());
        }

        yield return new WaitForSeconds(moveDelay); // Delay before next action
        currentCoroutine = null; // Allow next movement
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = 0;

        if (currentCoroutine == null) // Only allow movement if no coroutine is running
        {
            if (Input.GetKey(KeyCode.UpArrow) && !upArrow.isProcessing) discreteActions[0] = 1;
            if (Input.GetKey(KeyCode.DownArrow) && !downArrow.isProcessing) discreteActions[0] = 2;
            if (Input.GetKey(KeyCode.RightArrow) && !rightArrow.isProcessing) discreteActions[0] = 3;
            if (Input.GetKey(KeyCode.LeftArrow) && !leftArrow.isProcessing) discreteActions[0] = 4;
        }
    }
}

/* public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int action = actionBuffers.DiscreteActions[0];

        switch (action)
        {
            case 1: // Move Forward (Up Arrow)
                if (!upArrow.isProcessing)
                {
                    upArrow.SendCommand("north");
                    StartCoroutine(upArrow.ProcessSequence());
                }
                break;

            case 2: // Move Backward (Down Arrow)
                if (!downArrow.isProcessing)
                {
                    downArrow.SendCommand("south");
                    StartCoroutine(downArrow.ProcessSequence());
                }
                break;

            case 3: // Move Right (Right Arrow)
                if (!rightArrow.isProcessing)
                {
                    rightArrow.SendCommand("east");
                    StartCoroutine(rightArrow.ProcessSequence());
                }
                break;

            case 4: // Move Left (Left Arrow)
                if (!leftArrow.isProcessing)
                {
                    leftArrow.SendCommand("west");
                    StartCoroutine(leftArrow.ProcessSequence());
                }
                break;

            default:
                break;
        }

        float distanceToTarget = Vector3.Distance(transform.position, Target.position);

        if (distanceToTarget < 1.5f)
        {
            AddReward(1.0f);
            EndEpisode();
        }
        else
        {
            AddReward(-0.01f);
        }

        if (distanceToTarget > 10f)
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = 0; // Default: No movement

        if (Input.GetKey(KeyCode.UpArrow) && !upArrow.isProcessing) discreteActions[0] = 1;
        if (Input.GetKey(KeyCode.DownArrow) && !downArrow.isProcessing) discreteActions[0] = 2;
        if (Input.GetKey(KeyCode.RightArrow) && !rightArrow.isProcessing) discreteActions[0] = 3;
        if (Input.GetKey(KeyCode.LeftArrow) && !leftArrow.isProcessing) discreteActions[0] = 4;
    }
} */
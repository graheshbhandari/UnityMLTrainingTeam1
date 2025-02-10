using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Collections;

public class RobotControl : MonoBehaviour
{
    private const string IP = "127.0.0.1"; // Python server IP
    private const int PORT = 65432; // Python server port

    // Robot parts (make sure these are assigned in the Inspector)
    public GameObject robotA, robotB;
    public GameObject p1A, m1A, pivotP2A, p2A, m2A, pivotP3A, p3A;
    public GameObject p1B, m1B, pivotP2B, p2B, m2B, pivotP3B, p3B;

    // Rotation speed for pivot
    public float rotationSpeed = 50f;

    // Movement logic
    public void MoveRobot(Vector3 direction)
    {
        robotA.transform.Translate(direction * Time.deltaTime, Space.World);
        robotB.transform.Translate(direction * Time.deltaTime, Space.World);
    }

    public void SendCommand(string command)
    {
        try
        {
            using (TcpClient client = new TcpClient(IP, PORT))
            {
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(command);
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                Debug.Log("Response: " + Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }
        }
        catch (SocketException e)
        {
            Debug.LogError("Socket error: " + e.Message);
        }
    }

    // Rotate robot pivots
    public IEnumerator RotatePivots(float targetAngle)
    {
        float totalRotation = 0f;

        while (totalRotation < targetAngle)
        {
            float step = rotationSpeed * Time.deltaTime;
            if (totalRotation + step > targetAngle)
            {
                step = targetAngle - totalRotation;
            }

            pivotP3A.transform.Rotate(Vector3.right, step);
            pivotP3B.transform.Rotate(Vector3.right, step);

            totalRotation += step;

            yield return null;
        }
    }
}
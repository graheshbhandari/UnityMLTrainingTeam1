using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI; // Required for working with UI elements like Buttons
using TMPro;          // TextMeshPro support

public class PythonCommandSender : MonoBehaviour
{
    private const string IP = "127.0.0.1"; // IP address of the Python server
    private const int PORT = 65432;       // Port of the Python server

    // Public buttons to assign in the Unity Inspector
    public Button position1Button; // Button for "upright" command
    public Button position4Button; // Button for "north" command
    public Button position3Button; // Button for "south" command
    public Button position5Button; // Button for "east" command
    public Button position6Button; // Button for "west" command
    public Button position2Button; // Button for "walk" command

    public void upright()
    {
        SendCommand("upright");
    }
    public void north()
    {
        SendCommand("north");
    }
    public void south()
    {
        SendCommand("south");
    }
    public void east()
    {
        SendCommand("east");
    }
    public void west()
    {
        SendCommand("west");
    }
    public void walk()
    {
        SendCommand("walk");
    }
    public void turn()
    {
        SendCommand("turn");
    }

    // Method to send a command to the Python server
    public void SendCommand(string command)
    {
        if(GameManager.Instance.simulationType == GameManager.SimulationType.Physical ||
            GameManager.Instance.simulationType == GameManager.SimulationType.Both)
        {
            try
            {
                using (TcpClient client = new TcpClient(IP, PORT)) // Connect to the server
                {
                    NetworkStream stream = client.GetStream();

                    // Send the command to the server
                    byte[] data = Encoding.UTF8.GetBytes(command);
                    stream.Write(data, 0, data.Length);

                    // Optionally, read the response from the server
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
    }
}

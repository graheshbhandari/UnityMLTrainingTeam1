using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    // public GameObject pythonCommandSender;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    private GameManager() { }

    public enum SimulationType
    {
        Virtual,
        Physical,
        Both,
    }

    [SerializeField] public SimulationType simulationType = SimulationType.Virtual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

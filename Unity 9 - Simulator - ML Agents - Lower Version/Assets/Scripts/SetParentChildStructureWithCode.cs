using UnityEngine;

public class RobotHierarchy : MonoBehaviour
{
    // Sahnede bulunan nesneleri public olarak atayabilirsiniz
    public GameObject robotA, robotB;
    public GameObject p1A, p2A, p3A;
    public GameObject m1A, m2A;
    public GameObject pivotP2A, pivotP3A;

    public GameObject p1B, p2B, p3B;
    public GameObject m1B, m2B;
    public GameObject pivotP2B, pivotP3B;

    void Start()
    {
        // ROBOT-A ve ROBOT-B'yi birbirine baðlamak. Yani Robot-B'yi P3-A'nýn Child'ý yapmak gerekiyor.
        p3A.transform.SetParent(robotB.transform);

        // ROBOT-A içindeki yapýyý kurmak
        p1A.transform.SetParent(robotA.transform);
        m1A.transform.SetParent(p1A.transform);
        pivotP2A.transform.SetParent(m1A.transform);
        p2A.transform.SetParent(pivotP2A.transform);
        m2A.transform.SetParent(p2A.transform);
        pivotP3A.transform.SetParent(m2A.transform);
        p3A.transform.SetParent(pivotP3A.transform);

        // ROBOT-B içindeki yapýyý kurmak
        p1B.transform.SetParent(robotB.transform);
        m1B.transform.SetParent(p1B.transform);
        pivotP2B.transform.SetParent(m1B.transform);
        p2B.transform.SetParent(pivotP2B.transform);
        m2B.transform.SetParent(p2B.transform);
        pivotP3B.transform.SetParent(m2B.transform);
        p3B.transform.SetParent(pivotP3B.transform);
    }
}

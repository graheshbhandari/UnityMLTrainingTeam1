using UnityEngine;

public class AssignChild : MonoBehaviour
{
    public GameObject P3;      // P3 nesnesi
    public GameObject GROUND;  // GROUND nesnesi

    void Update()
    {
        // "G" tu�una bas�ld���nda �al��t�r
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (P3 != null && GROUND != null)
            {
                P3.transform.SetParent(GROUND.transform); // P3'� GROUND'un child'� yap
                Debug.Log("P3 is now a child of GROUND");
            }
            else
            {
                Debug.LogWarning("P3 or GROUND is not assigned in the inspector.");
            }
        }
    }
}

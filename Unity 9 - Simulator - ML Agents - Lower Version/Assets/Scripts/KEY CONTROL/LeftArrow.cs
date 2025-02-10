using UnityEngine;
using System.Collections;

public class LeftArrow : MonoBehaviour
{
    // Robotlar ve Pivotlar
    public GameObject p1A, m1A, pivotP2A, p2A, m2A, pivotP3A, p3A; // Robot A parçaları
    public GameObject p1B, m1B, pivotP2B, p2B, m2B, pivotP3B, p3B; // Robot B parçaları

    // Dönüş hızı
    public float rotationSpeed = 50f; // Saniyede 50 derece

    // İşlem devam kontrolü ve sayaç
    public bool isProcessing = false;
    private int sequenceCounter = 0; // Sıra sayacı

    private void Update()
    {
        // SolOk tuşuna basıldığında işlemi başlat
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !isProcessing)
        {
            if (GameManager.Instance.simulationType == GameManager.SimulationType.Physical ||
                GameManager.Instance.simulationType == GameManager.SimulationType.Both)
            {
                this.SendCommand("west"); //Send the "west" command
            }
            StartCoroutine(ProcessSequence());
        }

        // SağOk tuşuna basıldığında sayaç azalt
        if (Input.GetKeyDown(KeyCode.RightArrow) && !isProcessing)
        {
            DecreaseSequenceCounter(); // Sayaç azaltma fonksiyonu
        }
        // AşağıOk tuşuna basıldığında sayaç azalt
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isProcessing)
        {
            DecreaseSequenceCounter(); // Sayaç azaltma fonksiyonu
        }

        // YukarıOk tuşuna basıldığında sayaç azalt
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isProcessing)
        {
            DecreaseSequenceCounter(); // Sayaç azaltma fonksiyonu
        }

    }

    /// <summary>
    /// Belirtilen işlemleri sırasıyla yapar.
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

        // Sayaç sırasına göre işlemleri yap
        if (sequenceCounter % 2 == 0) // Çift sayı: Parent-Child 1 → Parent-Child 2
        {
            Debug.Log("1. Örüntü çalışıyor.");

            // 1. Parent-Child yapısını kur
            SetParentRelationships1();
            Debug.Log("Birinci Parent-Child yapısı kuruldu.");

            // 2. PivotP2A'yı -90 derece döndür
            yield return StartCoroutine(RotatePivotP2A(90f));
            Debug.Log("PivotP2A -90 derece döndürüldü.");

            // 3. Pivotları 90 derece döndür
            yield return StartCoroutine(RotatePivots(90f));
            Debug.Log("Pivotlar 90 derece döndürüldü (Birinci Yapı).");

            // 4. Parent-Child yapısını boz
            ResetParentRelationships();
            Debug.Log("Parent-Child yapısı sıfırlandı (İkinci Yapıya geçiş için).");

            // 5. İkinci Parent-Child yapısını kur
            SetParentRelationships2();
            Debug.Log("İkinci Parent-Child yapısı kuruldu.");

            // 6. Pivotları tekrar 90 derece döndür
            yield return StartCoroutine(RotatePivots(90f));
            Debug.Log("Pivotlar 90 derece döndürüldü (İkinci Yapı).");

            // 7. PivotP2A'yı -90 derece döndür
            yield return StartCoroutine(RotatePivotP2A(90f));
            Debug.Log("PivotP2A -90 derece döndürüldü.");
        }
        else // Tek sayı: Parent-Child 2 → Parent-Child 1
        {
            Debug.Log("2. Örüntü çalışıyor.");

            // 1. Parent-Child 2 yapısını kur
            SetParentRelationships2();
            Debug.Log("İkinci Parent-Child yapısı kuruldu.");

            // 2. PivotP2B'yi -90 derece döndür
            yield return StartCoroutine(RotatePivotP2B(90f));
            Debug.Log("PivotP2B 90 derece döndürüldü.");

            // 3. Pivotları 90 derece döndür
            yield return StartCoroutine(RotatePivots(90f));
            Debug.Log("Pivotlar 90 derece döndürüldü (İkinci Yapı).");

            // 4. Parent-Child yapısını boz
            ResetParentRelationships();
            Debug.Log("Parent-Child yapısı sıfırlandı (Birinci Yapıya geçiş için).");

            // 5. Parent-Child 1 yapısını kur
            SetParentRelationships1();
            Debug.Log("Birinci Parent-Child yapısı kuruldu.");

            // 6. Pivotları tekrar 90 derece döndür
            yield return StartCoroutine(RotatePivots(90f));
            Debug.Log("Pivotlar 90 derece döndürüldü (Birinci Yapı).");

            // 7. PivotP2B'yi -90 derece döndür
            yield return StartCoroutine(RotatePivotP2B(90f));
            Debug.Log("PivotP2B -90 derece döndürüldü.");

        }

        // Sayaç artırılır, bir sonraki "SağOk" tuşuna basıldığında sıralama değişir
        sequenceCounter++;

        isProcessing = false;
    }

    /// <summary>
    /// Sayaç değerini bir azaltır.
    /// </summary>
    private void DecreaseSequenceCounter()
    {
        sequenceCounter--; // Sayaç bir azaltılır
        Debug.Log("Sayaç Değeri: " + sequenceCounter);
    }

    /// <summary>
    /// Pivotları yumuşak bir şekilde döndürür.
    /// </summary>
    /// <param name="targetAngle">Döndürülmesi gereken toplam açı.</param>
    private IEnumerator RotatePivots(float targetAngle)
    {
        float totalRotation = 0f;

        while (totalRotation < targetAngle)
        {
            float step = rotationSpeed * Time.deltaTime;
            if (totalRotation + step > targetAngle)
            {
                step = targetAngle - totalRotation; // Hedefi aşmasın
            }

            // Pivotları döndür
            pivotP3A.transform.Rotate(Vector3.right, step);
            pivotP3B.transform.Rotate(-Vector3.right, step);

            totalRotation += step;

            yield return null; // Bir frame bekle
        }
    }

    /// <summary>
    /// PivotP2A'yı yumuşak bir şekilde döndürür.
    /// </summary>
    /// <param name="targetAngle">Döndürülmesi gereken toplam açı.</param>
    private IEnumerator RotatePivotP2A(float targetAngle)
    {
        float totalRotation = 0f;

        while (totalRotation < targetAngle)
        {
            float step = rotationSpeed * Time.deltaTime;
            if (totalRotation + step > targetAngle)
            {
                step = targetAngle - totalRotation; // Hedefi aşmasın
            }

            // PivotP2A'yı Y ekseninde döndür
            pivotP2A.transform.Rotate(Vector3.down, step);

            totalRotation += step;

            yield return null; // Bir frame bekle
        }
    }

    /// <summary>
    /// PivotP2B'yi yumuşak bir şekilde döndürür.
    /// </summary>
    /// <param name="targetAngle">Döndürülmesi gereken toplam açı.</param>
    private IEnumerator RotatePivotP2B(float targetAngle)
    {
        float totalRotation = 0f;

        while (totalRotation < targetAngle)
        {
            float step = rotationSpeed * Time.deltaTime;
            if (totalRotation + step > targetAngle)
            {
                step = targetAngle - totalRotation; // Hedefi aşmasın
            }

            // PivotP2B'yi Y ekseninde döndür
            pivotP2B.transform.Rotate(Vector3.down, step);

            totalRotation += step;

            yield return null; // Bir frame bekle
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

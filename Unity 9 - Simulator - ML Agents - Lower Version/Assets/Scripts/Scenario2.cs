using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    // D�n�� h�z�, derece/saniye cinsinden
    public float rotationSpeed = 50f;

    // Hedef a��lar�, s�ras�yla 0, 90, 180 dereceler
    private float[] targetAngles = { 0f, 90f, 180f };

    // Ge�erli hedef a��y� takip eden index
    private int currentTargetIndex = 0;

    // Duraklama durumu
    private bool isPaused = false;

    // Bekleme s�resi (saniye)
    public float waitTime = 1f;

    void Update()
    {
        // E�er duraklama durumundaysa, hareket etme
        if (isPaused) return;

        // �u anki a�� (y ekseninde)
        float currentAngle = transform.localEulerAngles.y;

        // 0 - 360 dereceden b�y�k a��lar varsa, 360'tan ��kararak -180 ile 180 aras�na normalize et
        if (currentAngle > 180) currentAngle -= 360;

        // Hedef a��ya ula��l�nca
        if (Mathf.Abs(currentAngle - targetAngles[currentTargetIndex]) < 0.1f)
        {
            // Hedef a��ya ula��ld���nda:
            // - Rotay� duraklat
            // - Bekleme s�resi sonras� y�n de�i�tir
            isPaused = true;
            Invoke(nameof(ChangeDirection), waitTime);
            return;
        }

        // Hedef a��ya do�ru d�n
        float step = rotationSpeed * Time.deltaTime; // D�n�� ad�m�n� hesapla
        float newAngle = Mathf.MoveTowards(currentAngle, targetAngles[currentTargetIndex], step); // Hedef a��ya do�ru ilerle
        transform.localEulerAngles = new Vector3(0f, newAngle, 0f); // Yeni a��y� uygula (y ekseni etraf�nda d�nd�r)
    }

    // D�n�� y�n�n� de�i�tirme fonksiyonu
    private void ChangeDirection()
    {
        // Duraklama durumundan ��k
        isPaused = false;

        // Bir sonraki hedef a��y� se�
        currentTargetIndex++;

        // E�er hedef a�� dizisinin sonuna geldiysek, ba�a d�n ve d�ng�y� yeniden ba�lat
        if (currentTargetIndex >= targetAngles.Length)
        {
            currentTargetIndex = 0;
        }
    }
}

using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    // Dönüþ hýzý, derece/saniye cinsinden
    public float rotationSpeed = 50f;

    // Hedef açýlarý, sýrasýyla 0, 90, 180 dereceler
    private float[] targetAngles = { 0f, 90f, 180f };

    // Geçerli hedef açýyý takip eden index
    private int currentTargetIndex = 0;

    // Duraklama durumu
    private bool isPaused = false;

    // Bekleme süresi (saniye)
    public float waitTime = 1f;

    void Update()
    {
        // Eðer duraklama durumundaysa, hareket etme
        if (isPaused) return;

        // Þu anki açý (y ekseninde)
        float currentAngle = transform.localEulerAngles.y;

        // 0 - 360 dereceden büyük açýlar varsa, 360'tan çýkararak -180 ile 180 arasýna normalize et
        if (currentAngle > 180) currentAngle -= 360;

        // Hedef açýya ulaþýlýnca
        if (Mathf.Abs(currentAngle - targetAngles[currentTargetIndex]) < 0.1f)
        {
            // Hedef açýya ulaþýldýðýnda:
            // - Rotayý duraklat
            // - Bekleme süresi sonrasý yön deðiþtir
            isPaused = true;
            Invoke(nameof(ChangeDirection), waitTime);
            return;
        }

        // Hedef açýya doðru dön
        float step = rotationSpeed * Time.deltaTime; // Dönüþ adýmýný hesapla
        float newAngle = Mathf.MoveTowards(currentAngle, targetAngles[currentTargetIndex], step); // Hedef açýya doðru ilerle
        transform.localEulerAngles = new Vector3(0f, newAngle, 0f); // Yeni açýyý uygula (y ekseni etrafýnda döndür)
    }

    // Dönüþ yönünü deðiþtirme fonksiyonu
    private void ChangeDirection()
    {
        // Duraklama durumundan çýk
        isPaused = false;

        // Bir sonraki hedef açýyý seç
        currentTargetIndex++;

        // Eðer hedef açý dizisinin sonuna geldiysek, baþa dön ve döngüyü yeniden baþlat
        if (currentTargetIndex >= targetAngles.Length)
        {
            currentTargetIndex = 0;
        }
    }
}

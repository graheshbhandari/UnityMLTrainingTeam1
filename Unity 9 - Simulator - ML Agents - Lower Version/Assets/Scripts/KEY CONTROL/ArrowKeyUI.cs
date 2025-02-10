using UnityEngine;
using UnityEngine.UI;

public class ArrowKeyUI : MonoBehaviour
{
    private GameObject[] arrowKeys;
    private Color defaultBorderColor = Color.white; // Beyaz kenarlık
    private Color activeFillColor = Color.white;    // Beyaz dolgu
    private Color transparentFillColor = new Color(1, 1, 1, 0); // Şeffaf dolgu (RGBA)

    private Button northButton;
    private Button southButton;
    private Button eastButton;
    private Button westButton;

    void Start()
    {
        // Find UI Buttons by name in the scene
        northButton = GameObject.Find("NorthButton")?.GetComponent<Button>();
        southButton = GameObject.Find("SouthButton")?.GetComponent<Button>();
        eastButton = GameObject.Find("EastButton")?.GetComponent<Button>();
        westButton = GameObject.Find("WestButton")?.GetComponent<Button>();
        // Canvas oluştur
        GameObject canvasObject = new GameObject("ArrowKeyCanvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        // Panel oluştur
        GameObject panelObject = new GameObject("ArrowKeyPanel");
        panelObject.transform.SetParent(canvasObject.transform);
        RectTransform panelRect = panelObject.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0, 0); // Sol alt köşe
        panelRect.anchorMax = new Vector2(0, 0); // Sol alt köşe
        panelRect.pivot = new Vector2(0, 0);     // Sol alt köşe
        panelRect.anchoredPosition = new Vector2(50, 50); // Ekranın iç kısmında biraz boşluk
        panelRect.sizeDelta = new Vector2(400, 400); // Panel boyutu büyütüldü

        // Ok kutuları için diziyi başlat
        arrowKeys = new GameObject[4];
        string[] keyNames = { "Up", "Down", "Left", "Right" };
        string[] symbols = { "↑", "↓", "←", "→" };

        // Her bir ok tuşu için kutu oluştur
        for (int i = 0; i < 4; i++)
        {
            // Kutu GameObject
            GameObject keyObject = new GameObject(keyNames[i]);
            keyObject.transform.SetParent(panelObject.transform);

            RectTransform rect = keyObject.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(100, 100); // Boyut iki katına çıkarıldı

            // Konumlandırma
            Vector2 position = Vector2.zero;
            if (i == 0) position = new Vector2(100, 200); // Up
            if (i == 1) position = new Vector2(100, 0);   // Down
            if (i == 2) position = new Vector2(0, 100);   // Left
            if (i == 3) position = new Vector2(200, 100); // Right
            rect.anchoredPosition = position;

            // Arkaplan için Image bileşeni
            Image bgImage = keyObject.AddComponent<Image>();
            bgImage.color = transparentFillColor; // Şeffaf başlangıç

            // Beyaz kenarlık ekle
            Outline outline = keyObject.AddComponent<Outline>();
            outline.effectColor = new Color(1f, 1f, 1f, 1f); // Beyaz kenarlık rengi
            outline.effectDistance = new Vector2(5, 5); // Kenarlık kalınlığı artırıldı

            // Kenarlığın her zaman görünür olması için Z sıralamasını ayarlayalım
            outline.GetComponent<RectTransform>().SetSiblingIndex(0); // Outline en üstte

            // İkon için alt GameObject
            GameObject textObject = new GameObject("Icon");
            textObject.transform.SetParent(keyObject.transform);

            RectTransform textRect = textObject.AddComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(100, 100); // İkon boyutu büyütüldü
            textRect.anchoredPosition = Vector2.zero;

            Text iconText = textObject.AddComponent<Text>();
            iconText.text = symbols[i]; // Unicode sembol atanıyor
            iconText.alignment = TextAnchor.MiddleCenter;
            iconText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); // Legacy font kullanımı
            iconText.color = Color.white; // Beyaz yazı rengi
            iconText.fontSize = 48; // Yazı boyutu büyütüldü

            arrowKeys[i] = keyObject;
        }
    }

    void Update()
    {
        UpdateKey(KeyCode.UpArrow, 0, northButton);
        UpdateKey(KeyCode.DownArrow, 1, southButton);
        UpdateKey(KeyCode.LeftArrow, 2, westButton);
        UpdateKey(KeyCode.RightArrow, 3, eastButton);
    }

    private void UpdateKey(KeyCode key, int index, Button uiButton)
    {
        Image image = arrowKeys[index].GetComponent<Image>();

        if (Input.GetKeyDown(key))
        {
            image.color = activeFillColor;
            uiButton?.onClick.Invoke(); // Simulate button click
        }
        else if (Input.GetKeyUp(key))
        {
            image.color = transparentFillColor;
        }
    }
}

using TMPro; 
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorController : MonoBehaviour
{
    public TMP_InputField widthInput; //InputField para el ancho
    public TMP_InputField heightInput; //InputField para el alto
    public Button generateMapButton;
    public GameObject tilePrefab; //Prefab de los tiles 
    public RectTransform gridParent; 

    private int mapWidth;
    private int mapHeight;

    void Start()
    {
        generateMapButton.onClick.AddListener(GenerateMap);
    }

    void GenerateMap()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        if (int.TryParse(widthInput.text, out mapWidth) && int.TryParse(heightInput.text, out mapHeight))
        {
            RectTransform tileRect = tilePrefab.GetComponent<RectTransform>();
            float tileWidth = tileRect.sizeDelta.x;
            float tileHeight = tileRect.sizeDelta.y;

            gridParent.sizeDelta = new Vector2(tileWidth * mapWidth, tileHeight * mapHeight);

            // Generar tiles
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    GameObject tile = Instantiate(tilePrefab, gridParent); 
                    RectTransform rect = tile.GetComponent<RectTransform>();

                    // Ajusta la posición en la grilla
                    rect.anchoredPosition = new Vector2(x * tileWidth, -y * tileHeight); 
                }
            }
        }
        else
        {
            Debug.LogError("Por favor, ingresa valores válidos para el ancho y alto.");
        }
    }
}

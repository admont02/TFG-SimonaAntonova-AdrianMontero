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
        // Limpia el grid existente
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        // Comprueba si las filas y columnas están configuradas
        if (int.TryParse(widthInput.text, out mapHeight) && int.TryParse(heightInput.text, out mapWidth))
        {
            if (mapWidth >= 3 && mapWidth <= 10 && mapHeight >= 3 && mapHeight <= 10)
            {
                int totalTiles = mapWidth * mapHeight; // Total de tiles

                for (int i = 0; i < totalTiles; i++)
                {
                    // Crear un nuevo tile
                    GameObject tile = Instantiate(tilePrefab, gridParent);

                    // Calcular fila y columna
                    int fila = i / mapHeight;   // Índice entero para la fila
                    int columna = i % mapHeight; // Resto para la columna

                    // Invertir las filas para que el grid empiece desde la esquina inferior izquierda
                    fila = mapWidth - 1 - fila;

                    // Configurar la posición del tile
                    RectTransform rect = tile.GetComponent<RectTransform>();
                    float tileWidth = rect.sizeDelta.x;
                    float tileHeight = rect.sizeDelta.y;

                    rect.anchoredPosition = new Vector2(columna * tileWidth, fila * tileHeight);

                    // Opcional: Renombrar el tile para facilitar la depuración
                    tile.name = $"Tile ({fila},{columna})";
                }
            }
            else
            {
                Debug.LogError("Por favor, ingresa valores válidos para las filas y columnas.");

            }
        }
        else
        {
            Debug.LogError("Por favor, ingresa valores válidos para las filas y columnas.");
        }
    }


    public int GetWidth()
    {
        return mapWidth;
    }
    public int GetHeight()
    {
        return mapHeight;
    }
}

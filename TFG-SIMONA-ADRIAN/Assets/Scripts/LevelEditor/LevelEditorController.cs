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

    public GridLayoutGroup gridLayoutGroup;

    private int mapWidth;
    private int mapHeight;

    void Start()
    {
        if (generateMapButton != null)
            generateMapButton.onClick.AddListener(GenerateMap);
        else
        {
            mapWidth = 9;
            mapHeight = 9;
        }
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
                gridLayoutGroup.cellSize = new Vector2(
                    gridParent.transform.parent.GetComponent<RectTransform>().sizeDelta.x / mapWidth,
                    gridParent.transform.parent.GetComponent<RectTransform>().sizeDelta.y / mapHeight);

                //gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                gridLayoutGroup.constraintCount = mapWidth;

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
                    rect.sizeDelta = gridLayoutGroup.cellSize;
                    rect.GetComponent<BoxCollider2D>().size = gridLayoutGroup.cellSize;
                    ////tile.
                    //float tileWidth = rect.sizeDelta.x * (gridParent.sizeDelta.x/ mapWidth);
                    //float tileHeight = rect.sizeDelta.y * (gridParent.sizeDelta.y/ mapHeight);

                    //rect.anchoredPosition = new Vector2(columna * tileWidth, fila * tileHeight);

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

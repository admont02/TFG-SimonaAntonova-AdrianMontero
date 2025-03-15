using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MapSaver : MonoBehaviour
{
    public Transform gridParent; // Contenedor del grid
    public int filas; // Número de filas en el grid
    public int columnas; // Número de columnas en el grid

    public void SaveMap(string fileName)
    {
        MapaData mapa = new MapaData
        {
            numPiezas = 0, // Se actualizará conforme se agreguen piezas
            filas = gridParent.GetComponent<LevelEditorController>().GetWidth(),
            columnas = gridParent.GetComponent<LevelEditorController>().GetHeight()
        };

        // Recorre todos los tiles del grid
        foreach (Transform tile in gridParent)
        {
            // Obtener la posición del tile
            int fila = Mathf.RoundToInt(tile.GetComponent<RectTransform>().anchoredPosition.y / -50f); // Asegúrate de ajustar "-50" según el tamaño del tile
            int columna = Mathf.RoundToInt(tile.GetComponent<RectTransform>().anchoredPosition.x / 50f);

            // Obtener la imagen actual del tile
            Image tileImage = tile.GetComponent<Image>();
            if (tileImage != null && tileImage.sprite != null)
            {
                // Determina el tipo de pieza en función del sprite (ajusta según tu lógica)
                if (tileImage.sprite.name == "Crossroad")
                {
                    mapa.Crossroad.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "Vertical")
                {
                    mapa.Vertical.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "Horizontal")
                {
                    mapa.Horizontal.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "Roundabout")
                {
                    mapa.Roundabout.Add(new TipoDePieza { fil = fila, col = columna });
                }

                // Incrementa el contador de piezas
                mapa.numPiezas++;
            }
        }

        // Convierte el mapa a JSON
        string json = JsonUtility.ToJson(mapa, true);

        // Guarda el JSON en un archivo
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, json);

        Debug.Log($"Mapa guardado en {path}");
    }
}

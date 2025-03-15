using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MapSaver : MonoBehaviour
{
    public Transform gridParent; // Contenedor del grid
    public int filas; // N�mero de filas en el grid
    public int columnas; // N�mero de columnas en el grid

    public void SaveMap(string fileName)
    {
        MapaData mapa = new MapaData
        {
            numPiezas = 0, // Se actualizar� conforme se agreguen piezas
            filas = gridParent.GetComponent<LevelEditorController>().GetWidth(),
            columnas = gridParent.GetComponent<LevelEditorController>().GetHeight()
        };

        // Recorre todos los tiles del grid
        foreach (Transform tile in gridParent)
        {
            // Obtener la posici�n del tile
            int fila = Mathf.RoundToInt(tile.GetComponent<RectTransform>().anchoredPosition.y / -50f); // Aseg�rate de ajustar "-50" seg�n el tama�o del tile
            int columna = Mathf.RoundToInt(tile.GetComponent<RectTransform>().anchoredPosition.x / 50f);

            // Obtener la imagen actual del tile
            Image tileImage = tile.GetComponent<Image>();
            if (tileImage != null && tileImage.sprite != null)
            {
                // Determina el tipo de pieza en funci�n del sprite (ajusta seg�n tu l�gica)
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

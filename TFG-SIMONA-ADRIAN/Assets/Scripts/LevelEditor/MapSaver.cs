using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MapSaver : MonoBehaviour
{
    public Transform gridParent; // Contenedor del grid
    public int filas; // Número de filas en el grid
    public int columnas; // Número de columnas en el grid

    public void SaveMap()
    {
        MapaData mapa = new MapaData
        {
            numPiezas = gridParent.GetComponent<LevelEditorController>().GetWidth() * gridParent.GetComponent<LevelEditorController>().GetHeight(),
            filas = gridParent.GetComponent<LevelEditorController>().GetWidth(),
            columnas = gridParent.GetComponent<LevelEditorController>().GetHeight(),
        };

        foreach (Transform tile in gridParent)
        {
            // Obtener fila y columna del nombre del tile
            string[] parts = tile.name.Replace("Tile (", "").Replace(")", "").Split(',');
            int fila = int.Parse(parts[0]);
            int columna = int.Parse(parts[1]);

            // Comprobar si el tile tiene una imagen asignada
            Image tileImage = tile.GetComponent<Image>();
            if (tileImage != null && tileImage.sprite != null)
            {
                // Agregar a la lista correspondiente según el sprite del tile
                if (tileImage.sprite.name == "Roundabout")
                {
                    mapa.Roundabout.Add(new TipoDePieza { fil = fila, col = columna });
                }
                // Otros casos (Vertical, Horizontal, Roundabout, etc.)
            }
        }

        // Convierte a JSON y guarda
        string json = JsonUtility.ToJson(mapa, true);
        string path = Path.Combine(Application.streamingAssetsPath, "prueba.json");
        File.WriteAllText(path, json);

        Debug.Log($"Mapa guardado en {path}");
    }

}

using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MapSaver : MonoBehaviour
{
    public Transform gridParent; 
    public int filas; 
    public int columnas; 

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
            //Obtener fila y columna del nombre del tile
            string[] parts = tile.name.Replace("Tile (", "").Replace(")", "").Split(',');
            int fila = int.Parse(parts[0]);
            int columna = int.Parse(parts[1]);

            //Comprobar si el tile tiene una imagen asignada
            Image tileImage = tile.GetComponent<Image>();
            if (tileImage != null && tileImage.sprite != null)
            {
               
                if (tileImage.sprite.name == "Roundabout")
                {
                    mapa.Roundabout.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "Crossroad")
                {
                    mapa.Crossroad.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if(tileImage.sprite.name == "Vertical")
                {
                    mapa.Vertical.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if(tileImage.sprite.name == "Horizontal")
                {
                    mapa.Horizontal.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if(tileImage.sprite.name == "Pavement")
                {
                    mapa.Pavement.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if(tileImage.sprite.name == "TurnRight")
                {
                    mapa.TurnRight.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if(tileImage.sprite.name == "TurnLeft")
                {
                    mapa.TurnLeft.Add(new TipoDePieza { fil = fila, col = columna });
                }

            }
        }

        //JSON 
        string json = JsonUtility.ToJson(mapa, true);
        string path = Path.Combine(Application.streamingAssetsPath, "prueba.json");
        File.WriteAllText(path, json);

        Debug.Log($"Mapa guardado en {path}");
    }

}

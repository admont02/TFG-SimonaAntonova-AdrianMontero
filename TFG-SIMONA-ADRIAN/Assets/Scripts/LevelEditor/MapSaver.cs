using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LevelLoaderXML;

public class MapSaver : MonoBehaviour
{
    public Transform gridParent;
    public Dropdown levelTypeDropdown;
    public int filas;
    public int columnas;

    public TMP_InputField textMeshProUGUI;
    public void SaveMap()
    {
        string levelType = levelTypeDropdown.options[levelTypeDropdown.value].text;
        MapaCompleto mapaCompleto = new MapaCompleto
        {
            type = levelType,
            mapa = new MapaData
            {
                numPiezas = gridParent.GetComponent<LevelEditorController>().GetWidth() * gridParent.GetComponent<LevelEditorController>().GetHeight(),
                filas = gridParent.GetComponent<LevelEditorController>().GetWidth(),
                columnas = gridParent.GetComponent<LevelEditorController>().GetHeight(),
                Vertical = new List<TipoDePieza>(),
                Horizontal = new List<TipoDePieza>(),
                Grass_2 = new List<TipoDePieza>(),
                TurnLeft = new List<TipoDePieza>(),
                TurnRight = new List<TipoDePieza>(),
                TurnLeft2 = new List<TipoDePieza>(),
                TurnRight2 = new List<TipoDePieza>()
            },
            maxVelocidad = new List<MaxVelocidad>(),
            IACars = new List<IA_Car>(),
            targetJugador = new TargetForPlayer(),
            jugadorNuevo = new Jugador()
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
                    mapaCompleto.mapa.Roundabout.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "Crossroad")
                {
                    mapaCompleto.mapa.Crossroad.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "Vertical")
                {
                    mapaCompleto.mapa.Vertical.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "VerticalContinua")
                {
                    mapaCompleto.mapa.VerticalContinua.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "Horizontal")
                {
                    mapaCompleto.mapa.Horizontal.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "HorizontalContinua")
                {
                    mapaCompleto.mapa.HorizontalContinua.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "Pavement")
                {
                    mapaCompleto.mapa.Pavement.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "Pavement_1")
                {
                    mapaCompleto.mapa.Pavement_1.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "Pavement_2")
                {
                    mapaCompleto.mapa.Pavement_2.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "Pavement_3")
                {
                    mapaCompleto.mapa.Pavement_3.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "Pavement_4")
                {
                    mapaCompleto.mapa.Pavement_4.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "TurnRight")
                {
                    mapaCompleto.mapa.TurnRight.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "TurnRightContinua")
                {
                    mapaCompleto.mapa.TurnRightContinua.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "TurnLeft")
                {
                    mapaCompleto.mapa.TurnLeft.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "TurnLeftContinua")
                {
                    mapaCompleto.mapa.TurnLeftContinua.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "TurnLeft2Continua")
                {
                    mapaCompleto.mapa.TurnLeft2Continua.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "TurnRight2")
                {
                    mapaCompleto.mapa.TurnRight2.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "TurnRight2Continua")
                {
                    mapaCompleto.mapa.TurnRight2Continua.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "TurnLeft2")
                {
                    mapaCompleto.mapa.TurnLeft2.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "TunnelVertical")
                {
                    mapaCompleto.mapa.TunnelVertical.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "TunnelHorizontal")
                {
                    mapaCompleto.mapa.TunnelHorizontal.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "Grass")
                {
                    mapaCompleto.mapa.Grass.Add(new TipoDePieza { fil = fila, col = columna });
                }
                else if (tileImage.sprite.name == "Grass_2")
                {
                    mapaCompleto.mapa.Grass_2.Add(new TipoDePieza { fil = fila, col = columna });
                }
                // Guardar stops (InteractivePoints)
                foreach (InteractivePoint point in tile.GetComponentsInChildren<InteractivePoint>())
                {
                    if (point.gameObject.transform.childCount > 0)
                    {
                        string nombreHijo = point.gameObject.transform.GetChild(0).name;
                        if (nombreHijo == "stop")
                        {
                            mapaCompleto.stops.Add(new Stop
                            {
                                pieza = new Pieza { index = tile.GetSiblingIndex() },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion
                            });
                        }
                        else if (nombreHijo == "prohibido")
                        {
                            mapaCompleto.prohibidos.Add(new Prohibido
                            {
                                pieza = new Pieza { index = tile.GetSiblingIndex() },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion
                            });
                        }
                        else if (nombreHijo == "ceda")
                        {
                            mapaCompleto.cedas.Add(new Ceda
                            {
                                pieza = new Pieza { index = tile.GetSiblingIndex() },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion
                            });
                        }
                        else if (nombreHijo == "IA_Car")
                        {
                            mapaCompleto.IACars.Add(new IA_Car
                            {
                                pieza = new Pieza { index = tile.GetSiblingIndex() },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion
                            });
                        }
                        else if (nombreHijo == "targetJugador")
                        {
                            mapaCompleto.targetJugador = (new TargetForPlayer
                            {
                                pieza = new Pieza { index = tile.GetSiblingIndex() },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col }
                            });
                        }
                        else if (nombreHijo == "Car_Player")
                        {
                            mapaCompleto.jugadorNuevo = (new Jugador
                            {
                                pieza = new Pieza { index = tile.GetSiblingIndex() },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion
                            });
                        }
                    }

                }
            }
        }

        //JSON 
        string json = JsonUtility.ToJson(mapaCompleto, true);
        string path = Path.Combine(Application.streamingAssetsPath, "Editor/" + textMeshProUGUI.text + ".json");
        int counter = 1;
        while (File.Exists(path))
        {
            path = Path.Combine(Application.streamingAssetsPath, $"Editor/{textMeshProUGUI.text}_{counter}.json");
            counter++;
        }
        File.WriteAllText(path, json);

        Debug.Log($"Mapa guardado en {path}");
    }

}

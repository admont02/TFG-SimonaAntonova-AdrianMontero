using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static LevelLoaderXML;

public class MapSaver : MonoBehaviour
{
    public Transform gridParent;
    public Transform gridParentPrioridad;
    public Dropdown levelTypeDropdown;
    public int filas;
    public int columnas;

    public TMP_InputField textMeshProUGUI;

    public Toggle rainToggle;
    public Toggle fogToggle;
    public Toggle nightToggle;
    public Toggle deslumbramientoToggle;

    public Toggle cortasToggle;
    public Toggle largasToggle;
    public Toggle posToggle;
    public Toggle antidelToggle;
    public Toggle antitraToggle;

    string[] empty = new string[1];
    public void SaveMap()
    {
        string levelType = levelTypeDropdown.options[levelTypeDropdown.value].text;
        Transform mapParent = gridParent;
        if (gridParentPrioridad.gameObject.activeSelf)
            mapParent = gridParentPrioridad;

        MapaCompleto mapaCompleto = new MapaCompleto
        {
            type = levelType,
            mapa = new MapaData
            {
                numPiezas = mapParent.GetComponent<LevelEditorController>().GetWidth() * mapParent.GetComponent<LevelEditorController>().GetHeight(),
                filas = mapParent.GetComponent<LevelEditorController>().GetWidth(),
                columnas = mapParent.GetComponent<LevelEditorController>().GetHeight(),
                Vertical = new List<TipoDePieza>(),
                Horizontal = new List<TipoDePieza>(),
                Grass_2 = new List<TipoDePieza>(),
                TurnLeft = new List<TipoDePieza>(),
                TurnRight = new List<TipoDePieza>(),
                TurnLeft2 = new List<TipoDePieza>(),
                TurnRight2 = new List<TipoDePieza>()
            },
            maxVelocidad = new List<MaxVelocidad>(),
            iniLuces = new List<IniLuz>(),
            IACars = new List<IA_Car>(),
            objetivo = new List<string>(),
            targetJugador = new TargetForPlayer(),
            jugador = new Jugador(),
            levelDialogs = empty,
            completedDialogs = empty,
            wrongDialogs = empty,
            rain = rainToggle.isOn,
            fog = fogToggle.isOn,
            night = nightToggle.isOn,
            deslumbramiento = deslumbramientoToggle.isOn

        };

        int numJugador = 0;
        int numTarget = 0;
        List<(int indexEditor, int posicionIACars)> cochesOrdenados = new List<(int, int)>();

        foreach (Transform tile in mapParent)
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
                    int ind = fila * mapaCompleto.mapa.columnas + columna;
                    mapaCompleto.mapa.Roundabout.Add(new RoundaboutPieza { fil = fila, col = columna, conexiones = new List<int> { ind - 1, ind + 1, ind - mapaCompleto.mapa.columnas, ind + mapaCompleto.mapa.columnas } });
                }
                else if (tileImage.sprite.name == "Crossroad")
                {
                    int ind = fila * mapaCompleto.mapa.columnas + columna;
                    mapaCompleto.mapa.Crossroad.Add(new CrossroadPieza { fil = fila, col = columna, conexiones = new List<int> { ind - 1, ind + 1, ind - mapaCompleto.mapa.columnas, ind + mapaCompleto.mapa.columnas } });
                }
                else if (tileImage.sprite.name == "Crossroad1")
                {
                    int ind = fila * mapaCompleto.mapa.columnas + columna;
                    mapaCompleto.mapa.Crossroad.Add(new CrossroadPieza { fil = fila, col = columna, conexiones = new List<int> { ind - 1, ind + 1, ind - mapaCompleto.mapa.columnas, ind + mapaCompleto.mapa.columnas } });
                    mapaCompleto.cuadriculas.Add(new Cuadricula
                    {
                        pieza = new Pieza { index = fila * mapaCompleto.mapa.columnas + columna },
                        subPosicion = new SubPosicion { fil = 9, col = 9 }
                    });
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
                // default
                else
                {
                    mapaCompleto.mapa.Pavement.Add(new TipoDePieza { fil = fila, col = columna });
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
                                pieza = new Pieza { index = fila * mapaCompleto.mapa.columnas + columna },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion
                            });
                        }
                        else if (nombreHijo == "semaforo")
                        {
                            mapaCompleto.semaforos.Add(new Semaforo
                            {
                                pieza = new Pieza { index = fila * mapaCompleto.mapa.columnas + columna },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion,
                                greenSeconds = 20.0f,
                                amberSeconds = 2.0f,
                                redSeconds = 15.0f,
                                initialLight = "red"
                            });
                        }
                        else if (nombreHijo == "prohibido")
                        {
                            mapaCompleto.prohibidos.Add(new Prohibido
                            {
                                pieza = new Pieza { index = fila * mapaCompleto.mapa.columnas + columna },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion
                            });
                            foreach (CrossroadPieza crossroad in mapaCompleto.mapa.Crossroad)
                            {
                                if (crossroad.conexiones.Contains(fila * mapaCompleto.mapa.columnas + columna))
                                {
                                    crossroad.conexiones.Remove(fila * mapaCompleto.mapa.columnas + columna);
                                    Debug.Log($"Conexión eliminada en Crossroad {crossroad.fil}, {crossroad.col} debido a señal de prohibido en {fila}, {columna}");
                                }
                            }
                            foreach (RoundaboutPieza roundabout in mapaCompleto.mapa.Roundabout)
                            {
                                if (roundabout.conexiones.Contains(fila * mapaCompleto.mapa.columnas + columna))
                                {
                                    roundabout.conexiones.Remove(fila * mapaCompleto.mapa.columnas + columna);
                                    Debug.Log($"Conexión eliminada en roundabout {roundabout.fil}, {roundabout.col} debido a señal de prohibido en {fila}, {columna}");
                                }
                            }
                        }
                        else if (nombreHijo == "ceda")
                        {
                            mapaCompleto.cedas.Add(new Ceda
                            {
                                pieza = new Pieza { index = fila * mapaCompleto.mapa.columnas + columna },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion
                            });
                        }
                        else if (nombreHijo == "iniLuz")
                        {
                            mapaCompleto.iniLuces.Add(new IniLuz
                            {
                                pieza = new Pieza { index = fila * mapaCompleto.mapa.columnas + columna },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion
                            });
                        }
                        else if (nombreHijo == "FrenteIzq")
                        {
                            mapaCompleto.frenteIzq.Add(new FrenteIzq
                            {
                                pieza = new Pieza { index = fila * mapaCompleto.mapa.columnas + columna },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion
                            });
                        }
                        else if (nombreHijo == "FrenteDcha")
                        {
                            mapaCompleto.frenteDcha.Add(new FrenteDcha
                            {
                                pieza = new Pieza { index = fila * mapaCompleto.mapa.columnas + columna },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion
                            });
                        }
                        else if (nombreHijo == "Frente")
                        {
                            mapaCompleto.frente.Add(new Frente
                            {
                                pieza = new Pieza { index = fila * mapaCompleto.mapa.columnas + columna },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion
                            });
                        }
                        else if (nombreHijo == "maxVel")
                        {
                            mapaCompleto.maxVelocidad.Add(new MaxVelocidad
                            {
                                pieza = new Pieza { index = fila * mapaCompleto.mapa.columnas + columna },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion,
                                velocidad = point.gameObject.transform.GetChild(0).GetComponent<VelocitySetter>().velocidad
                            });
                        }
                        else if (nombreHijo == "IA_Car")
                        {
                            BotonCoche botonCoche = point.gameObject.transform.GetChild(0).GetComponent<BotonCoche>();

                            int _branchTo = 0; // Valor por defecto

                            if (botonCoche != null)
                            {
                                if (botonCoche.cocheData.recto) _branchTo = 1;
                                else if (botonCoche.cocheData.izqda) _branchTo = 2;
                                else if (botonCoche.cocheData.dcha) _branchTo = 0;
                            }
                            mapaCompleto.IACars.Add(new IA_Car
                            {
                                pieza = new Pieza { index = fila * mapaCompleto.mapa.columnas + columna },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion,
                                branchTo = _branchTo
                            });
                            if (levelType == "Prioridad")
                                cochesOrdenados.Add((botonCoche.cocheData.index, mapaCompleto.IACars.Count - 1));
                        }
                        else if (nombreHijo == "Ambulance")
                        {
                            BotonCoche botonCoche = point.gameObject.transform.GetChild(0).GetComponent<BotonCoche>();

                            int _branchTo = 0; // Valor por defecto

                            if (botonCoche != null)
                            {
                                if (botonCoche.cocheData.recto) _branchTo = 1;
                                else if (botonCoche.cocheData.izqda) _branchTo = 2;
                                else if (botonCoche.cocheData.dcha) _branchTo = 0;
                            }
                            mapaCompleto.IACars.Add(new IA_Car
                            {
                                pieza = new Pieza { index = fila * mapaCompleto.mapa.columnas + columna },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion,
                                branchTo = _branchTo,
                                vehicle = "ambulance"
                            });
                            if (levelType == "Prioridad")
                                cochesOrdenados.Add((botonCoche.cocheData.index, mapaCompleto.IACars.Count - 1));
                        }
                        else if (nombreHijo == "AmbulanceEmergency")
                        {
                            BotonCoche botonCoche = point.gameObject.transform.GetChild(0).GetComponent<BotonCoche>();

                            int _branchTo = -1; // Valor por defecto

                            if (botonCoche != null)
                            {
                                if (botonCoche.cocheData.recto) _branchTo = 1;
                                else if (botonCoche.cocheData.izqda) _branchTo = 2;
                                else if (botonCoche.cocheData.dcha) _branchTo = 0;
                            }
                            mapaCompleto.IACars.Add(new IA_Car
                            {
                                pieza = new Pieza { index = fila * mapaCompleto.mapa.columnas + columna },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion,
                                branchTo = _branchTo,
                                vehicle = "ambulance",
                                emergency = true
                            });
                            if (levelType == "Prioridad")
                                cochesOrdenados.Add((botonCoche.cocheData.index, mapaCompleto.IACars.Count - 1));
                        }
                        else if (nombreHijo == "Bus")
                        {
                            BotonCoche botonCoche = point.gameObject.transform.GetChild(0).GetComponent<BotonCoche>();

                            int _branchTo = 0; // Valor por defecto

                            if (botonCoche != null)
                            {
                                if (botonCoche.cocheData.recto) _branchTo = 1;
                                else if (botonCoche.cocheData.izqda) _branchTo = 2;
                                else if (botonCoche.cocheData.dcha) _branchTo = 0;
                            }
                            mapaCompleto.IACars.Add(new IA_Car
                            {
                                pieza = new Pieza { index = fila * mapaCompleto.mapa.columnas + columna },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion,
                                branchTo = _branchTo,
                                vehicle = "bus"
                            });
                            if (levelType == "Prioridad")
                                cochesOrdenados.Add((botonCoche.cocheData.index, mapaCompleto.IACars.Count - 1));
                        }
                        else if (nombreHijo == "TargetForPlayer")
                        {


                            mapaCompleto.targetJugador = new TargetForPlayer();
                            mapaCompleto.targetJugador = (new TargetForPlayer
                            {
                                pieza = new Pieza { index = fila * mapaCompleto.mapa.columnas + columna },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col }
                            });
                            numTarget++;
                        }
                        else if (nombreHijo == "Car_Player")
                        {
                            mapaCompleto.jugador = new Jugador();

                            mapaCompleto.jugador = (new Jugador
                            {
                                pieza = new Pieza { index = fila * mapaCompleto.mapa.columnas + columna },
                                subPosicion = new SubPosicion { fil = point.fil, col = point.col },
                                orientacion = point.orientacion
                            });
                            numJugador++;
                        }
                    }
                }
            }
        }
        if (levelType == "Manejo")
        {
            // Comprobar que hay jugador y target
            if (numJugador != 1 || numTarget < 1)
            {
                Debug.LogError("Los niveles de manejo deben contener 1 jugador y al menos 1 destino (estrella), ahora hay: " + numJugador + ", " + numTarget);
                return;
            }
            empty[0] = " Nivel de Manejo creado desde el editor";
            mapaCompleto.levelDialogs = empty;

            mapaCompleto.rain = false;
            mapaCompleto.fog = false;
            mapaCompleto.night = false;
            mapaCompleto.deslumbramiento = false;
        }
        else if (levelType == "Luces")
        {
            // Comprobar que hay jugador y target
            if (numJugador < 1 || numTarget < 1)
            {
                Debug.LogError("Los niveles de luces deben contener 1 jugador y al menos 1 destino (estrella), ahora hay: " + numJugador + ", " + numTarget);
                return;
            }

            //if (rainToggle.isOn || fogToggle.isOn || nightToggle.isOn || deslumbramientoToggle.isOn)
            //{
            mapaCompleto.rain = rainToggle.isOn;
            mapaCompleto.fog = fogToggle.isOn;
            mapaCompleto.night = nightToggle.isOn;
            mapaCompleto.deslumbramiento = deslumbramientoToggle.isOn;


            if (cortasToggle.isOn)
                mapaCompleto.objetivo.Add(cortasToggle.gameObject.name);
            if (largasToggle.isOn)
                mapaCompleto.objetivo.Add(largasToggle.gameObject.name);
            if (posToggle.isOn)
                mapaCompleto.objetivo.Add(posToggle.gameObject.name);
            if (antidelToggle.isOn)
                mapaCompleto.objetivo.Add(antidelToggle.gameObject.name);
            if (antitraToggle.isOn)
                mapaCompleto.objetivo.Add(antitraToggle.gameObject.name);
            //}
            //else
            //{
            //    Debug.LogError("Los niveles de luces deben tener algun elemento de la lista de opciones activo");
            //    return;
            //}

            empty[0] = " Nivel de Luces creado desde el editor";
            mapaCompleto.levelDialogs = empty;
        }
        else if (levelType == "Prioridad")
        {
            if (cochesOrdenados.Count < 2)
            {
                Debug.LogError("Los niveles de prioridad deben contener al menos 2 cochesIA, ahora hay: " + cochesOrdenados.Count);
                return;
            }
            for (int i = 0; i < mapaCompleto.IACars.Count; i++)
            {
                if (mapaCompleto.IACars[i].branchTo < 0)
                {
                    Debug.LogError("Los coches deben tener un destino en los niveles de prioridad ");
                    return;
                }
            }
            // Comprobar 
            empty[0] = " Nivel de Prioridad creado desde el editor";
            mapaCompleto.levelDialogs = empty;

            mapaCompleto.rain = false;
            mapaCompleto.fog = false;
            mapaCompleto.night = false;
            mapaCompleto.deslumbramiento = false;
            mapaCompleto.correctOrder = cochesOrdenados.OrderBy(c => c.indexEditor).Select(c => c.posicionIACars).ToList();

            Debug.Log("Orden correcto de coches guardado: " + string.Join(", ", mapaCompleto.correctOrder));

        }

        string[] auxi = new string[1];
        auxi[0] = " Nivel completado correctamente!";
        mapaCompleto.completedDialogs = auxi;
        string[] auxi2 = new string[1];
        auxi2[0] = " Nivel incorrecto!";
        mapaCompleto.wrongDialogs = auxi2;


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

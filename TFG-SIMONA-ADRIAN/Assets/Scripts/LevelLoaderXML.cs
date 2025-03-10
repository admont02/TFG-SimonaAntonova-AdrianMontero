using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LevelLoaderXML : MonoBehaviour
{
    // Clases mapeadas para cada sección del XML
    [Serializable]
    public class Conexion2
    {
        public int ConexionId;
    }

    [Serializable]
    public class Pieza2
    {
        public int Id;
        public int Fil;
        public int Col;
        public List<int> Conexiones = new List<int>();
    }

    [Serializable]
    public class Prefab2
    {
        public string Name;
        public List<Pieza2> Piezas = new List<Pieza2>();
    }

    [Serializable]
    public class JugadorNuevo2
    {
        public int PiezaIndex;
        public int SubFil;
        public int SubCol;
        public string Orientacion;
    }

    [Serializable]
    public class Semaforo2
    {
        public bool Doble;
        public int PiezaIndex;
        public int SubFil;
        public int SubCol;
        public string Orientacion;
        public float GreenSeconds;
        public float AmberSeconds;
        public float RedSeconds;
        public string InitialLight;
    }

    [Serializable]
    public class Cuadricula2
    {
        public int PiezaIndex;
        public int SubFil;
        public int SubCol;
    }

    [Serializable]
    public class TargetJugador2
    {
        public int PiezaIndex;
        public int SubFil;
        public int SubCol;
    }

    [Serializable]
    public class IACar2
    {
        public int PiezaIndex;
        public int SubFil;
        public int SubCol;
        public string Orientacion;
        public List<Vector3> Posiciones = new List<Vector3>();
    }

    [Serializable]
    public class Nivel2
    {
        public int Id;
        public List<Prefab2> Prefabs = new List<Prefab2>();
        public JugadorNuevo2 Jugador;
        public List<Semaforo2> Semaforos = new List<Semaforo2>();
        public List<Cuadricula2> Cuadriculas = new List<Cuadricula2>();
        public TargetJugador2 Target;
        public List<IACar2> Coches = new List<IACar2>();
        public List<string> Dialogos = new List<string>();
        public List<string> DialogosCompletados = new List<string>();
    }

    public XmlDocument xmlFile;
    private Nivel2 nivel = new Nivel2();

    private void Start()
    {
        if (xmlFile != null)
        {
            CargarNivelDesdeXML(xmlFile.Value);
        }
        else
        {
            Debug.LogError("Archivo XML no asignado.");
        }
    }

    private void CargarNivelDesdeXML(string xmlContent)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);

            XmlNode nivelNode = xmlDoc.SelectSingleNode("nivel");
            nivel.Id = int.Parse(nivelNode.Attributes["id"].Value);

            // Carga Prefabs y Piezas
            XmlNodeList prefabs = nivelNode.SelectNodes("mapaNuevo/Piezas/Prefab");
            foreach (XmlNode prefabNode in prefabs)
            {
                Prefab2 prefab = new Prefab2();
                prefab.Name = prefabNode.Attributes["name"].Value;

                XmlNodeList piezas = prefabNode.SelectNodes("pieza");
                foreach (XmlNode piezaNode in piezas)
                {
                    Pieza2 pieza = new Pieza2();
                    pieza.Id = int.Parse(piezaNode.Attributes["id"].Value);
                    pieza.Fil = int.Parse(piezaNode.Attributes["fil"].Value);
                    pieza.Col = int.Parse(piezaNode.Attributes["col"].Value);

                    XmlNodeList conexiones = piezaNode.SelectNodes("conexiones/conexion");
                    foreach (XmlNode conexionNode in conexiones)
                    {
                        pieza.Conexiones.Add(int.Parse(conexionNode.InnerText));
                    }

                    prefab.Piezas.Add(pieza);
                }

                nivel.Prefabs.Add(prefab);
            }

            // Carga JugadorNuevo
            XmlNode jugadorNode = nivelNode.SelectSingleNode("jugadorNuevo");
            nivel.Jugador = new JugadorNuevo2
            {
                PiezaIndex = int.Parse(jugadorNode.SelectSingleNode("pieza").Attributes["index"].Value),
                SubFil = int.Parse(jugadorNode.SelectSingleNode("subPosicion").Attributes["fil"].Value),
                SubCol = int.Parse(jugadorNode.SelectSingleNode("subPosicion").Attributes["col"].Value),
                Orientacion = jugadorNode.SelectSingleNode("orientacion").InnerText
            };

            // Carga SemaforosNuevos
            XmlNodeList semaforos = nivelNode.SelectNodes("semaforosNuevos/semaforo");
            foreach (XmlNode semaforoNode in semaforos)
            {
                Semaforo2 semaforo = new Semaforo2
                {
                    Doble = bool.Parse(semaforoNode.SelectSingleNode("doble").InnerText),
                    PiezaIndex = int.Parse(semaforoNode.SelectSingleNode("pieza").Attributes["index"].Value),
                    SubFil = int.Parse(semaforoNode.SelectSingleNode("subPosicion").Attributes["fil"].Value),
                    SubCol = int.Parse(semaforoNode.SelectSingleNode("subPosicion").Attributes["col"].Value),
                    Orientacion = semaforoNode.SelectSingleNode("orientacion").InnerText,
                    GreenSeconds = float.Parse(semaforoNode.SelectSingleNode("greenSeconds").InnerText),
                    AmberSeconds = float.Parse(semaforoNode.SelectSingleNode("amberSeconds").InnerText),
                    RedSeconds = float.Parse(semaforoNode.SelectSingleNode("redSeconds").InnerText),
                    InitialLight = semaforoNode.SelectSingleNode("initialLight").InnerText
                };

                nivel.Semaforos.Add(semaforo);
            }

            // Carga Cuadriculas
            XmlNodeList cuadriculas = nivelNode.SelectNodes("cuadriculas/cuadricula");
            foreach (XmlNode cuadriculaNode in cuadriculas)
            {
                Cuadricula2 cuadricula = new Cuadricula2
                {
                    PiezaIndex = int.Parse(cuadriculaNode.SelectSingleNode("pieza").Attributes["index"].Value),
                    SubFil = int.Parse(cuadriculaNode.SelectSingleNode("subPosicion").Attributes["fil"].Value),
                    SubCol = int.Parse(cuadriculaNode.SelectSingleNode("subPosicion").Attributes["col"].Value)
                };

                nivel.Cuadriculas.Add(cuadricula);
            }

            // Carga TargetJugador
            XmlNode targetNode = nivelNode.SelectSingleNode("targetJugador");
            nivel.Target = new TargetJugador2
            {
                PiezaIndex = int.Parse(targetNode.SelectSingleNode("pieza").Attributes["index"].Value),
                SubFil = int.Parse(targetNode.SelectSingleNode("subPosicion").Attributes["fil"].Value),
                SubCol = int.Parse(targetNode.SelectSingleNode("subPosicion").Attributes["col"].Value)
            };

            // Carga IACars
            XmlNodeList coches = nivelNode.SelectNodes("IACars/coche");
            foreach (XmlNode cocheNode in coches)
            {
                IACar2 coche = new IACar2
                {
                    PiezaIndex = int.Parse(cocheNode.SelectSingleNode("pieza").Attributes["index"].Value),
                    SubFil = int.Parse(cocheNode.SelectSingleNode("subPosicion").Attributes["fil"].Value),
                    SubCol = int.Parse(cocheNode.SelectSingleNode("subPosicion").Attributes["col"].Value),
                    Orientacion = cocheNode.SelectSingleNode("orientacion").InnerText
                };

                XmlNodeList posiciones = cocheNode.SelectNodes("posiciones/posicion");
                foreach (XmlNode posicionNode in posiciones)
                {
                    coche.Posiciones.Add(new Vector3(
                        float.Parse(posicionNode.Attributes["x"].Value),
                        float.Parse(posicionNode.Attributes["y"].Value),
                        float.Parse(posicionNode.Attributes["z"].Value)
                    ));
                }

                nivel.Coches.Add(coche);
            }

            // Carga LevelDialogs
            XmlNodeList dialogos = nivelNode.SelectNodes("levelDialogs/dialogo");
            foreach (XmlNode dialogoNode in dialogos)
            {
                nivel.Dialogos.Add(dialogoNode.InnerText);
            }

            // Carga CompletedDialogs
            XmlNodeList completedDialogs = nivelNode.SelectNodes("completedDialogs/dialogo");
            foreach (XmlNode dialogoNode in completedDialogs)
            {
                nivel.DialogosCompletados.Add(dialogoNode.InnerText);
            }

            Debug.Log("Nivel cargado correctamente. ID: " + nivel.Id);
        }
        catch (Exception e)
        {
            Debug.LogError("Error al cargar el nivel desde el XML: " + e.Message);
        }
    }
}

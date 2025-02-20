using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Unity.AI.Navigation;
using UnityEngine.UIElements;

public class LevelLoader : MonoBehaviour
{
    public string jsonFileName = "nivel2.json";
    private string mapsFolderPath = "Assets/TemplatesMapas/";
    public GameObject TargetPrefab;
    [SerializeField]
    GameObject cocheIAPrefab;
    [SerializeField]
    GameObject ClicLevelManagerPref;
    [SerializeField]
    GameObject cuadriculaPrefab;
    [SerializeField]
    GameObject semaforoPrefab;
    [SerializeField]
    GameObject semaforoDoblePrefab;
    [SerializeField]
    GameObject playerPrefab;
    public void CargarNivel()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);
        filePath = filePath.Replace("\\", "/"); // Reemplaza las barras invertidas por barras normales
        Debug.Log("Ruta del archivo JSON: " + filePath);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Nivel nivelData = JsonUtility.FromJson<Nivel>(json);
            //if (Enum.TryParse(nivelData.type.ToString(), true, out LevelType tipo)) 
            //{ 
            //    nivelData.type = tipo; 
            //} 
            //else 
            //{ 
            //    nivelData.type = LevelType.Desconocido; 
            //}
            CrearNivel(nivelData);
            Debug.Log(" se encontró el archivo JSON.");
        }
        else
        {
            Debug.Log("No se encontró el archivo JSON.");
        }
    }
    Vector3 ConvertToPosition(int fil, int col, float scale)
    {
        float z = scale / 2 + scale * fil;
        float x = scale / 2 + scale * col;
        return new Vector3(x, 0, z);
    }
    Vector3 ConvertToSubPosition(Vector3 basePosition, int subFil, int subCol, float subScale)
    {
        float offsetZ = -100 / 2 + subScale / 2 + subScale * subFil;
        float offsetX = -100 / 2 + subScale / 2 + subScale * subCol;
        return basePosition + new Vector3(offsetX, 2.81f, offsetZ);
    }


    void CrearNivel(Nivel nivel)
    {
        // Crear el punto objetivo
        if (!nivel.isMenu)
        {
            // FUNCIONAMIENTO DE TEMPLATE MAPA HASTA 19/02

            if (nivel.mapa.nombre != null)
            {
                string mapPath = Path.Combine(mapsFolderPath, nivel.mapa.nombre);
                mapPath = mapPath.Replace("\\", "/");
                GameObject mapPrefab = Resources.Load<GameObject>(nivel.mapa.nombre);
                if (mapPrefab != null)
                {
                    GameObject instantiatedMap = Instantiate(mapPrefab, new Vector3(nivel.mapa.posicion.x, nivel.mapa.posicion.y, nivel.mapa.posicion.z), Quaternion.identity);
                    NavMeshSurface navM = instantiatedMap.GetComponentInChildren<NavMeshSurface>();
                    // navM.BuildNavMesh();
                }
                else
                {
                    Debug.LogError("No se encontró el prefab del mapa: " + mapPath);
                }

            }


            // CÓDIGO ACTUAL MAPAS (DESDE 19/02)

            //float scale = 100f; //Escala utilizada para convertir las filas y columnas a posiciones en Unity
            //float subScale = scale / 3;
            //List<Vector3> posicionesPiezas = new List<Vector3>();
            //foreach (var recta in nivel.mapaNuevo.rectas)
            //{
            //    Vector3 posicion = ConvertToPosition(recta.fil, recta.col, scale);
            //    posicionesPiezas.Add(posicion);
            //    GameObject rectaPrefab = Resources.Load<GameObject>("PiezasPrefabs/City_Crossroad");
            //    if (rectaPrefab != null)
            //    {
            //        Instantiate(rectaPrefab, posicion, Quaternion.identity);
            //    }
            //    else
            //    {
            //        Debug.LogError("No se encontró el prefab de la recta.");
            //    }
            //}



            //JUGADOR PREVIAMENTE
            if (nivel.jugador.posicionInicial != null)
            {
                Quaternion rotPlayer = Quaternion.Euler(nivel.jugador.rotacionInicial.x, nivel.jugador.rotacionInicial.y, nivel.jugador.rotacionInicial.z);
                GameObject playerObj = Instantiate(playerPrefab, new Vector3(nivel.jugador.posicionInicial.x, nivel.jugador.posicionInicial.y, nivel.jugador.posicionInicial.z), rotPlayer);
                playerObj.SetActive(true);
                GameManager.Instance.carController = playerObj.GetComponent<CarController>();
                GameManager.Instance.SetPlayer(playerObj.transform);
            }




            //JUGADOR NUEVO
            //if (nivel.jugadorNuevo != null)
            //{
            //    int indexPieza = nivel.jugadorNuevo.pieza.index;
            //    if (indexPieza >= 0 && indexPieza < posicionesPiezas.Count)
            //    {
            //        Vector3 posicionPieza = posicionesPiezas[indexPieza];
            //        Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
            //        Quaternion rotPlayer = Quaternion.Euler(nivel.jugadorNuevo.rotacionInicial.x, nivel.jugadorNuevo.rotacionInicial.y, nivel.jugadorNuevo.rotacionInicial.z);
            //        GameObject playerObj = Instantiate(playerPrefab, posicionJugador, rotPlayer);
            //        playerObj.SetActive(true);
            //        GameManager.Instance.carController = playerObj.GetComponent<CarController>();
            //        GameManager.Instance.SetPlayer(playerObj.transform);
            //    }
            //    else
            //    {
            //        Debug.LogError("No se encontró la pieza especificada para el jugador.");
            //    }
            //}

            GameObject targetPoint = Instantiate(TargetPrefab, new Vector3(nivel.targetJugador.x, nivel.targetJugador.y, nivel.targetJugador.z), Quaternion.identity);
            targetPoint.SetActive(true);
            GameManager.Instance.SetPlayerTarget(targetPoint);
            //targetPoint.transform.position = new Vector3(nivel.targetJugador.x, nivel.targetJugador.y, nivel.targetJugador.z);
            LineRenderer lineRenderer = targetPoint.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, targetPoint.transform.position);
            lineRenderer.SetPosition(1, targetPoint.transform.position + new Vector3(0, 100.0f, 0));
        }

        GameManager.Instance.dialogueSystem.SetLevelDialog(nivel.levelDialogs, nivel.completedDialogs);

        // Crear coches IA
        int id = 0;
        foreach (var cocheIA in nivel.cochesIA)
        {
            Quaternion rotation = Quaternion.Euler(cocheIA.rotacionInicial.x, cocheIA.rotacionInicial.y, cocheIA.rotacionInicial.z);
            GameObject cocheIAObj = Instantiate(cocheIAPrefab, new Vector3(cocheIA.posicionInicial.x, cocheIA.posicionInicial.y, cocheIA.posicionInicial.z), rotation);
            cocheIAObj.name = "CocheIA" + id;
            id++;

            GameObject priorityTextObj = new GameObject("PriorityText");
            priorityTextObj.transform.SetParent(cocheIAObj.transform);
            priorityTextObj.transform.localPosition = new Vector3(0, 2, 0);
            TextMesh priorityText = priorityTextObj.AddComponent<TextMesh>();
            priorityText.color = Color.red;


            GameManager.Instance.AddCocheIA(cocheIAObj);
            OtherCar otherCar = cocheIAObj.GetComponent<OtherCar>();
            if (otherCar != null && cocheIA.posiciones.Count > 0)
            {
                List<Vector3> destinations = new List<Vector3>();
                foreach (var pos in cocheIA.posiciones)
                {
                    destinations.Add(new Vector3(pos.x, pos.y, pos.z));
                }
                otherCar.SetDestinations(destinations);
            }
        }
        //CUADRICULAS
        foreach (var cuadricula in nivel.cuadriculas)
        {
            Quaternion prefabRotation = cuadriculaPrefab.transform.rotation;
            GameObject cuadriculaObj = Instantiate(cuadriculaPrefab, new Vector3(cuadricula.posicion.x, cuadricula.posicion.y, cuadricula.posicion.z), prefabRotation);
            cuadriculaObj.SetActive(true);
        }
        //SEMAFOROS
        foreach (var semaforo in nivel.semaforos)
        {
            Quaternion rotation = Quaternion.Euler(semaforo.rotacion.x, semaforo.rotacion.y, semaforo.rotacion.z);
            if (semaforo.doble)
            {
                GameObject semaforoObj = Instantiate(semaforoDoblePrefab, new Vector3(semaforo.posicion.x, semaforo.posicion.y, semaforo.posicion.z), rotation);
                foreach (Transform child in semaforoObj.transform)
                {
                    SimpleTrafficLight semaforoScript = child.GetComponent<SimpleTrafficLight>();
                    if (semaforoScript != null)
                    {
                        semaforoScript.greenSeconds = semaforo.greenSeconds;
                        semaforoScript.amberSeconds = semaforo.amberSeconds;
                        semaforoScript.redSeconds = semaforo.redSeconds;
                        semaforoScript.red.SetActive(semaforo.initialLight == "red");
                        semaforoScript.amber.SetActive(semaforo.initialLight == "amber");
                        semaforoScript.green.SetActive(semaforo.initialLight == "green");
                    }
                }
            }
            else
            {
                GameObject semaforoObj = Instantiate(semaforoPrefab, new Vector3(semaforo.posicion.x, semaforo.posicion.y, semaforo.posicion.z), rotation);

                SimpleTrafficLight semaforoScript = semaforoObj.GetComponent<SimpleTrafficLight>();
                semaforoScript.greenSeconds = semaforo.greenSeconds;
                semaforoScript.amberSeconds = semaforo.amberSeconds;
                semaforoScript.redSeconds = semaforo.redSeconds; // Configurar la luz inicial
                semaforoScript.red.SetActive(semaforo.initialLight == "red");
                semaforoScript.amber.SetActive(semaforo.initialLight == "amber");
                semaforoScript.green.SetActive(semaforo.initialLight == "green");
            }

        }
        if (nivel.fog)
        {
            GameManager.Instance.EnableFog();
        }
        switch (nivel.type)
        {
            //case LevelType.Desconocido:
            //    break;
            //case LevelType.Conduccion:
            //    break;
            case "Prioridad":


                GameObject managerInstance = Instantiate(ClicLevelManagerPref);

                break;
            case "Luces":
                Debug.Log("nivel luces");
                //GameObject cocheJugador = Instantiate(Resources.Load("CochePrefab"), new Vector3(nivel.posicionCoche.x, nivel.posicionCoche.y, nivel.posicionCoche.z), Quaternion.identity) as GameObject; 
                CarLights carLights = GameManager.Instance.carController.gameObject.GetComponent<CarLights>();
                if (carLights != null)
                {
                    carLights.objetivoLuces = nivel.objetivo;
                }
                break;
        }
    }

}


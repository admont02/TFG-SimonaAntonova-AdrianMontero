using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Unity.AI.Navigation;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

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
    [SerializeField]
    GameObject stopPrefab;
    [SerializeField]
    GameObject forbiddenPrefab;

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

    Vector3 ConvertToSubPosition(Vector3 basePosition, int subFil, int subCol, float subScale, int subdivisions)
    {
        //el numero magico 5 es un offset para que 0,0 sea en la pos 5,5
        float subdivisionSize = subScale / subdivisions; // Tamaño de cada subdivisión
        float offsetX = -subScale / 2 + 5 + subdivisionSize * subFil;
        float offsetZ = -subScale / 2 + 5 + subdivisionSize * subCol;
        return basePosition + new Vector3(offsetX, 2.81f, offsetZ);
    }


    Quaternion ConvertirOrientacionARotacion(string orientacion)
    {
        switch (orientacion.ToLower())
        {
            case "izquierda":
                return Quaternion.Euler(0, 270, 0);
            case "derecha":
                return Quaternion.Euler(0, 90, 0);
            case "arriba":
                return Quaternion.Euler(0, 0, 0);
            case "abajo":
                return Quaternion.Euler(0, 180, 0);
            default:
                Debug.LogWarning("Orientación desconocida, usando rotación por defecto.");
                return Quaternion.identity;
        }
    }

    void CrearNivel(Nivel nivel)
    {
        float scale = 100f; //Escala utilizada para convertir las filas y columnas a posiciones en Unity
        int subdivisions = 20;
        float subScale = scale;
        //List<Vector3> posicionesPiezas = new List<Vector3>();
        Dictionary<int, GameObject> posicionesPiezas = new Dictionary<int, GameObject>();
        int nodos = nivel.mapaNuevo.numPiezas;
        GameManager.Instance.filas = nivel.mapaNuevo.filas;
        GameManager.Instance.columnas = nivel.mapaNuevo.columnas;
        Digrafo digrafo = new Digrafo(nodos);
        // Crear el punto objetivo
        if (!nivel.isMenu)
        {

            foreach (var recta in nivel.mapaNuevo.Crossroad)
            {

                Vector3 posicion = ConvertToPosition(recta.fil, recta.col, scale);
                GameObject rectaPrefab = Resources.Load<GameObject>("PiezasPrefabs/Crossroad");
                if (rectaPrefab != null)
                {
                    recta.id = recta.fil * nivel.mapaNuevo.columnas + recta.col;
                    posicionesPiezas[recta.id] = Instantiate(rectaPrefab, posicion, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("No se encontró el prefab de la recta.");
                }
                foreach (var conn in recta.conexiones)
                {
                    digrafo.ponArista(recta.id, conn);
                }

            }
            foreach (var recta in nivel.mapaNuevo.Vertical)
            {

                Vector3 posicion = ConvertToPosition(recta.fil, recta.col, scale);
                GameObject rectaPrefab = Resources.Load<GameObject>("PiezasPrefabs/Vertical");
                if (rectaPrefab != null)
                {
                    recta.id = recta.fil * nivel.mapaNuevo.columnas + recta.col;
                    posicionesPiezas[recta.id] = Instantiate(rectaPrefab, posicion, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("No se encontró el prefab de la recta.");
                }
                foreach (var conn in recta.conexiones)
                {
                    digrafo.ponArista(recta.id, conn);
                }
            }
            foreach (var recta in nivel.mapaNuevo.Horizontal)
            {

                Vector3 posicion = ConvertToPosition(recta.fil, recta.col, scale);
                GameObject rectaPrefab = Resources.Load<GameObject>("PiezasPrefabs/Horizontal");
                if (rectaPrefab != null)
                {

                    recta.id = recta.fil * nivel.mapaNuevo.columnas + recta.col;
                    posicionesPiezas[recta.id] = Instantiate(rectaPrefab, posicion, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("No se encontró el prefab de la recta.");
                }
                foreach (var conn in recta.conexiones)
                {
                    digrafo.ponArista(recta.id, conn);
                }
            }
            foreach (var recta in nivel.mapaNuevo.Intersection)
            {

                Vector3 posicion = ConvertToPosition(recta.fil, recta.col, scale);
                GameObject rectaPrefab = Resources.Load<GameObject>("PiezasPrefabs/Intersection");
                if (rectaPrefab != null)
                {

                    recta.id = recta.fil * nivel.mapaNuevo.columnas + recta.col;
                    posicionesPiezas[recta.id] = Instantiate(rectaPrefab, posicion, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("No se encontró el prefab de la recta.");
                }
                foreach (var conn in recta.conexiones)
                {
                    digrafo.ponArista(recta.id, conn);
                }
            }
            foreach (var recta in nivel.mapaNuevo.Intersection3)
            {

                Vector3 posicion = ConvertToPosition(recta.fil, recta.col, scale);
                GameObject rectaPrefab = Resources.Load<GameObject>("PiezasPrefabs/Intersection3");
                if (rectaPrefab != null)
                {

                    recta.id = recta.fil * nivel.mapaNuevo.columnas + recta.col;
                    posicionesPiezas[recta.id] = Instantiate(rectaPrefab, posicion, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("No se encontró el prefab de la recta.");
                }
                foreach (var conn in recta.conexiones)
                {
                    digrafo.ponArista(recta.id, conn);
                }
            }
            foreach (var recta in nivel.mapaNuevo.Intersection4)
            {

                Vector3 posicion = ConvertToPosition(recta.fil, recta.col, scale);
                GameObject rectaPrefab = Resources.Load<GameObject>("PiezasPrefabs/Intersection4");
                if (rectaPrefab != null)
                {

                    recta.id = recta.fil * nivel.mapaNuevo.columnas + recta.col;
                    posicionesPiezas[recta.id] = Instantiate(rectaPrefab, posicion, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("No se encontró el prefab de la recta.");
                }
                foreach (var conn in recta.conexiones)
                {
                    digrafo.ponArista(recta.id, conn);
                }
            }
            foreach (var recta in nivel.mapaNuevo.Intersection2)
            {

                Vector3 posicion = ConvertToPosition(recta.fil, recta.col, scale);
                GameObject rectaPrefab = Resources.Load<GameObject>("PiezasPrefabs/Intersection2");
                if (rectaPrefab != null)
                {

                    recta.id = recta.fil * nivel.mapaNuevo.columnas + recta.col;
                    posicionesPiezas[recta.id] = Instantiate(rectaPrefab, posicion, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("No se encontró el prefab de la recta.");
                }
                foreach (var conn in recta.conexiones)
                {
                    digrafo.ponArista(recta.id, conn);
                }
            }
            foreach (var recta in nivel.mapaNuevo.Roundabout)
            {

                Vector3 posicion = ConvertToPosition(recta.fil, recta.col, scale);
                GameObject rectaPrefab = Resources.Load<GameObject>("PiezasPrefabs/Roundabout");
                if (rectaPrefab != null)
                {
                    recta.id = recta.fil * nivel.mapaNuevo.columnas + recta.col;
                    posicionesPiezas[recta.id] = Instantiate(rectaPrefab, posicion, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("No se encontró el prefab de la recta.");
                }
                foreach (var conn in recta.conexiones)
                {
                    digrafo.ponArista(recta.id, conn);
                }
            }
            foreach (var recta in nivel.mapaNuevo.TurnRight)
            {
                Vector3 posicion = ConvertToPosition(recta.fil, recta.col, scale);
                GameObject rectaPrefab = Resources.Load<GameObject>("PiezasPrefabs/TurnRight");
                if (rectaPrefab != null)
                {
                    recta.id = recta.fil * nivel.mapaNuevo.columnas + recta.col;
                    posicionesPiezas[recta.id] = Instantiate(rectaPrefab, posicion, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("No se encontró el prefab de la recta.");
                }
                foreach (var conn in recta.conexiones)
                {
                    digrafo.ponArista(recta.id, conn);
                }
            }
            foreach (var recta in nivel.mapaNuevo.TurnLeft)
            {

                Vector3 posicion = ConvertToPosition(recta.fil, recta.col, scale);
                GameObject rectaPrefab = Resources.Load<GameObject>("PiezasPrefabs/TurnLeft");
                if (rectaPrefab != null)
                {
                    recta.id = recta.fil * nivel.mapaNuevo.columnas + recta.col;
                    posicionesPiezas[recta.id] = Instantiate(rectaPrefab, posicion, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("No se encontró el prefab de la recta.");
                }
                foreach (var conn in recta.conexiones)
                {
                    digrafo.ponArista(recta.id, conn);
                }
            }
            foreach (var recta in nivel.mapaNuevo.TurnLeft2)
            {

                Vector3 posicion = ConvertToPosition(recta.fil, recta.col, scale);
                GameObject rectaPrefab = Resources.Load<GameObject>("PiezasPrefabs/TurnLeft2");
                if (rectaPrefab != null)
                {
                    recta.id = recta.fil * nivel.mapaNuevo.columnas + recta.col;
                    posicionesPiezas[recta.id] = Instantiate(rectaPrefab, posicion, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("No se encontró el prefab de la recta.");
                }
                foreach (var conn in recta.conexiones)
                {
                    digrafo.ponArista(recta.id, conn);
                }
            }
            foreach (var recta in nivel.mapaNuevo.TurnRight2)
            {

                Vector3 posicion = ConvertToPosition(recta.fil, recta.col, scale);
                GameObject rectaPrefab = Resources.Load<GameObject>("PiezasPrefabs/TurnRight2");
                if (rectaPrefab != null)
                {
                    recta.id = recta.fil * nivel.mapaNuevo.columnas + recta.col;
                    posicionesPiezas[recta.id] = Instantiate(rectaPrefab, posicion, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("No se encontró el prefab de la recta.");
                }
                foreach (var conn in recta.conexiones)
                {
                    digrafo.ponArista(recta.id, conn);
                }
            }
            foreach (var r_f_l_t in nivel.mapaNuevo.Pavement)
            {
                Vector3 posicion = ConvertToPosition(r_f_l_t.fil, r_f_l_t.col, scale);
                GameObject rectaPrefab = Resources.Load<GameObject>("PiezasPrefabs/Pavement");
                if (rectaPrefab != null)
                {
                    r_f_l_t.id = r_f_l_t.fil * nivel.mapaNuevo.columnas + r_f_l_t.col;
                    posicionesPiezas[r_f_l_t.id] = Instantiate(rectaPrefab, posicion, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("No se encontró el prefab de la recta.");
                }
                foreach (var conn in r_f_l_t.conexiones)
                {
                    digrafo.ponArista(r_f_l_t.id, conn);
                }
            }


            //JUGADOR NUEVO
            if (nivel.jugadorNuevo != null)
            {
                int indexPieza = nivel.jugadorNuevo.pieza.index;
                if (indexPieza >= 0 && indexPieza < posicionesPiezas.Count)
                {
                    Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
                    //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
                    Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale, subdivisions);

                    // Quaternion rotPlayer = Quaternion.Euler(nivel.jugadorNuevo.rotacionInicial.x, nivel.jugadorNuevo.rotacionInicial.y, nivel.jugadorNuevo.rotacionInicial.z);
                    Quaternion rotPlayer = ConvertirOrientacionARotacion(nivel.jugadorNuevo.orientacion);
                    GameObject playerObj = Instantiate(playerPrefab, posicionJugador, rotPlayer);
                    playerObj.SetActive(true);
                    GameManager.Instance.carController = playerObj.GetComponent<PrometeoCarController>();
                    GameManager.Instance.SetPlayer(playerObj.transform);
                }
                else
                {
                    Debug.LogError("No se encontró la pieza especificada para el jugador.");
                }
            }
            Vector3 posicionPiezaTarget = posicionesPiezas[nivel.targetJugador.pieza.index].transform.position;
            //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
            Vector3 posicionTarget = ConvertToSubPosition(posicionPiezaTarget, nivel.targetJugador.subPosicion.fil, nivel.targetJugador.subPosicion.col, subScale, subdivisions);
            GameObject targetPoint = Instantiate(TargetPrefab, posicionTarget, Quaternion.identity);
            targetPoint.SetActive(true);
            GameManager.Instance.SetPlayerTarget(targetPoint);

            //targetPoint.transform.position = new Vector3(nivel.targetJugador.x, nivel.targetJugador.y, nivel.targetJugador.z);
            LineRenderer lineRenderer = targetPoint.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, targetPoint.transform.position);
            lineRenderer.SetPosition(1, targetPoint.transform.position + new Vector3(0, 100.0f, 0));


            GameObject playerObjAux = GameManager.Instance.carController.gameObject;
            //GPSController gpsController = playerObjAux.GetComponent<GPSController>();
            //gpsController.Initialize(digrafo, posicionesPiezas, nivel.jugadorNuevo.pieza.index, nivel.targetJugador.pieza.index);
            GameManager.Instance.graph = digrafo;
        }

        GameManager.Instance.dialogueSystem.SetLevelDialog(nivel.levelDialogs, nivel.completedDialogs);


        //IAs nuevo
        int id = 0;
        foreach (var IAcar in nivel.IACars)
        {
            Quaternion rotation = ConvertirOrientacionARotacion(IAcar.orientacion);
            int indexPieza = IAcar.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
            //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
            Vector3 posicionCoche = ConvertToSubPosition(posicionPieza, IAcar.subPosicion.fil, IAcar.subPosicion.col, subScale, subdivisions);
            GameObject cocheIAObj = Instantiate(cocheIAPrefab, posicionCoche, rotation);
            cocheIAObj.name = "CocheIA" + id;
            id++;

            GameObject priorityTextObj = new GameObject("PriorityText");
            priorityTextObj.transform.SetParent(cocheIAObj.transform);
            priorityTextObj.transform.localPosition = new Vector3(0, 2, 0);
            TextMesh priorityText = priorityTextObj.AddComponent<TextMesh>();
            priorityText.color = Color.red;


            GameManager.Instance.AddCocheIA(cocheIAObj);
            OtherCar otherCar = cocheIAObj.GetComponent<OtherCar>();
            WaypointNavigator wN = cocheIAObj.GetComponent<WaypointNavigator>();
            wN.SetInitialWaypoint(posicionesPiezas, indexPieza, IAcar.orientacion);
            //if (otherCar != null && IAcar.posiciones.Count > 0)
            //{
            //    List<Vector3> destinations = new List<Vector3>();
            //    foreach (var pos in IAcar.posiciones)
            //    {
            //        destinations.Add(new Vector3(pos.x, pos.y, pos.z));
            //    }
            //    otherCar.SetDestinations(destinations);
            //}
        }

        //CUADRICULAS ACTUAL
        foreach (var cuadricula in nivel.cuadriculas)
        {
            int indexPieza = cuadricula.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;

            //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
            Vector3 posicionCuadricula = ConvertToSubPosition(posicionPieza, cuadricula.subPosicion.fil, cuadricula.subPosicion.col, subScale, subdivisions);
            Quaternion prefabRotation = cuadriculaPrefab.transform.rotation;
            GameObject cuadriculaObj = Instantiate(cuadriculaPrefab, posicionCuadricula, prefabRotation);
            cuadriculaObj.SetActive(true);
        }
        //stops
        foreach (var sign in nivel.stops)
        {
            int indexPieza = sign.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;

            //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
            Vector3 posicionCuadricula = ConvertToSubPosition(posicionPieza, sign.subPosicion.fil, sign.subPosicion.col, subScale, subdivisions);
            Quaternion prefabRotation = ConvertirOrientacionARotacion(sign.orientacion);
            GameObject stopObj = Instantiate(stopPrefab, posicionCuadricula, prefabRotation);
            stopObj.SetActive(true);
        }
        //prohibidos
        foreach (var sign in nivel.prohibidos)
        {
            int indexPieza = sign.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;

            //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
            Vector3 posicionCuadricula = ConvertToSubPosition(posicionPieza, sign.subPosicion.fil, sign.subPosicion.col, subScale, subdivisions);
            Quaternion prefabRotation = ConvertirOrientacionARotacion(sign.orientacion);
            GameObject stopObj = Instantiate(forbiddenPrefab, posicionCuadricula, prefabRotation);
            stopObj.SetActive(true);
        }
        //SEMAFOROS NUEVOS
        foreach (var semaforo in nivel.semaforosNuevos)
        {
            // Quaternion rotation = Quaternion.Euler(semaforo.rotacion.x, semaforo.rotacion.y, semaforo.rotacion.z);
            Quaternion rotation = ConvertirOrientacionARotacion(semaforo.orientacion);
            int indexPieza = semaforo.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
            //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
            Vector3 posicionSemaforo = ConvertToSubPosition(posicionPieza, semaforo.subPosicion.fil, semaforo.subPosicion.col, subScale, subdivisions);
            if (semaforo.doble)
            {
                GameObject semaforoObj = Instantiate(semaforoDoblePrefab, posicionSemaforo, rotation);
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
                GameObject semaforoObj = Instantiate(semaforoPrefab, posicionSemaforo, rotation);

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


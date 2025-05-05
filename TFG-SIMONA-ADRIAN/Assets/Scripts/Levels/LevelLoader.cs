using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Unity.AI.Navigation;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using Xasu.HighLevel;

public class LevelLoader : MonoBehaviour
{
    public string jsonFileName = "nivel2.json";
    private string mapsFolderPath = "Assets/TemplatesMapas/";
    public GameObject TargetPrefab;
    [SerializeField]
    GameObject cocheIAPrefab;
    [SerializeField]
    GameObject ambulancePrefab;
    [SerializeField]
    GameObject busPrefab;
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
    GameObject cedaPrefab;
    [SerializeField]
    GameObject forbiddenPrefab;
    [SerializeField]
    GameObject MaxSpeedPrefab;
    [SerializeField]
    GameObject FrenteIzqPrefab;
    [SerializeField]
    GameObject iniLuzPrefab;
    [SerializeField]
    GameObject destroyer;
    public GameObject TargetIconPrefab;
    public float scale = 50f; //Escala utilizada para convertir las filas y columnas a posiciones en Unity
    // Padre de las piezas
    [SerializeField]
    Transform conjuntoPiezas;

    public void CargarNivel()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, SceneData.JsonFileName);
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
        float offsetX = -subScale / 2 + 2.5f + subdivisionSize * subFil;
        float offsetZ = -subScale / 2 + 2.5f + subdivisionSize * subCol;
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

        int subdivisions = 20;
        float subScale = scale;
        //List<Vector3> posicionesPiezas = new List<Vector3>();
        Dictionary<int, GameObject> posicionesPiezas = new Dictionary<int, GameObject>();
        int nodos = nivel.mapa.numPiezas;
        GameManager.Instance.filas = nivel.mapa.filas;
        GameManager.Instance.columnas = nivel.mapa.columnas;
        GameManager.Instance.scale = scale;
        Digrafo digrafo = new Digrafo(nodos);
        // Crear el punto objetivo
        if (!nivel.isMenu)
        {

            CrearTipoPiezas(nivel.mapa.Crossroad, "Crossroad", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Vertical, "Vertical", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.VerticalContinua, "VerticalContinua", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Horizontal, "Horizontal", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.HorizontalContinua, "HorizontalContinua", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Roundabout, "Roundabout", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.TurnRight, "TurnRight", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.TurnRightContinua, "TurnRightContinua", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.TurnRight2, "TurnRight2", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.TurnRight2Continua, "TurnRight2Continua", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.TurnLeft, "TurnLeft", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.TurnLeftContinua, "TurnLeftContinua", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.TurnLeft2, "TurnLeft2", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.TurnLeft2Continua, "TurnLeft2Continua", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Intersection, "Intersection", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Intersection2, "Intersection2", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Intersection3, "Intersection3", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Intersection4, "Intersection4", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Pavement, "Pavement", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Pavement_2, "Pavement_2", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Pavement_3, "Pavement_3", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Pavement_4, "Pavement_4", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.TunnelVertical, "TunnelVertical", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Grass, "Grass", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Grass_2, "Grass_2", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);

            //JUGADOR NUEVO
            if (nivel.jugador.pieza != null)
            {
                int indexPieza = nivel.jugador.pieza.index;
                if (indexPieza >= 0 && indexPieza < posicionesPiezas.Count)
                {
                    Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
                    //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
                    Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugador.subPosicion.fil, nivel.jugador.subPosicion.col, subScale, subdivisions);

                    // Quaternion rotPlayer = Quaternion.Euler(nivel.jugadorNuevo.rotacionInicial.x, nivel.jugadorNuevo.rotacionInicial.y, nivel.jugadorNuevo.rotacionInicial.z);
                    Quaternion rotPlayer = ConvertirOrientacionARotacion(nivel.jugador.orientacion);
                    GameObject playerObj = Instantiate(playerPrefab, posicionJugador, rotPlayer);
                    playerObj.transform.localScale = new Vector3(playerObj.transform.localScale.x * scale / 100, playerObj.transform.localScale.y * scale / 100, playerObj.transform.localScale.z * scale / 100);
                    playerObj.SetActive(true);
                    GameManager.Instance.carController = playerObj.GetComponent<PrometeoCarController>();
                    GameManager.Instance.SetPlayer(playerObj.transform);
                }
                else
                {
                    Debug.LogError("No se encontró la pieza especificada para el jugador.");
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
                lineRenderer.SetPosition(1, targetPoint.transform.position + new Vector3(0, 50.0f, 0));
                //icono minimapa
                Vector3 iconPosition = posicionTarget;
                iconPosition.y += 30.0f; // Aquí defines cuánto quieres aumentar la altura
                Quaternion rotacionTarget = Quaternion.Euler(100, 0, 0);
                GameObject minimapIcon = Instantiate(TargetIconPrefab, iconPosition, rotacionTarget);
                minimapIcon.SetActive(true);


                GameObject playerObjAux = GameManager.Instance.carController.gameObject;
                //GPSController gpsController = playerObjAux.GetComponent<GPSController>();
                //gpsController.Initialize(digrafo, posicionesPiezas, nivel.jugadorNuevo.pieza.index, nivel.targetJugador.pieza.index);

            }
        }
        GameManager.Instance.graph = digrafo;
        GameManager.Instance.dialogueSystem.SetLevelDialog(nivel.levelDialogs, nivel.completedDialogs,nivel.wrongDialogs);


        //IAs nuevo
        int id = 0;
        foreach (var IAcar in nivel.IACars)
        {
            Quaternion rotation = ConvertirOrientacionARotacion(IAcar.orientacion);
            int indexPieza = IAcar.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
            //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
            Vector3 posicionCoche = ConvertToSubPosition(posicionPieza, IAcar.subPosicion.fil, IAcar.subPosicion.col, subScale, subdivisions);
            GameObject cocheIAObj;
            if (IAcar.vehicle == "ambulance")
            {
                cocheIAObj = Instantiate(ambulancePrefab, posicionCoche, rotation);
                if (IAcar.emergency)
                {
                    cocheIAObj.GetComponent<Ambulance>().redLight.enabled = true;
                    cocheIAObj.GetComponent<Ambulance>().lightsOn=true;
                    cocheIAObj.GetComponent<OtherCar>().movementSpeed += 10;

                }
                else
                {
                    cocheIAObj.GetComponent<Ambulance>().redLight.enabled = false;
                    cocheIAObj.GetComponent<Ambulance>().blueLight.enabled = false;
                }

            }
            else if (IAcar.vehicle == "bus")
            {
                cocheIAObj = Instantiate(busPrefab, posicionCoche, rotation);

            }
            else
            {
                cocheIAObj = Instantiate(cocheIAPrefab, posicionCoche, rotation);

            }
            cocheIAObj.transform.localScale = new Vector3(cocheIAObj.transform.localScale.x * scale / 100, cocheIAObj.transform.localScale.y * scale / 100, cocheIAObj.transform.localScale.z * scale / 100);
            cocheIAObj.name = "CocheIA" + id;


            GameObject priorityTextObj = new GameObject("PriorityText");
            priorityTextObj.transform.SetParent(cocheIAObj.transform);
            if (IAcar.vehicle == "bus")
            {
                priorityTextObj.transform.localPosition = new Vector3(0, 4.5f, 0);
                priorityTextObj.transform.localScale = new Vector3(1, 1, 1);
            }
            else if(IAcar.vehicle == "ambulance")
            {
                priorityTextObj.transform.localPosition = new Vector3(0, 15.5f, 0);
                priorityTextObj.transform.localScale = new Vector3(4, 4, 4);
            }
            else
            {
                priorityTextObj.transform.localPosition = new Vector3(0, 12.5f, 0);
                priorityTextObj.transform.localScale = new Vector3(4, 4, 4);
            }
            
            TextMesh priorityText = priorityTextObj.AddComponent<TextMesh>();
            priorityText.color = Color.red;
           // priorityText.font = fuenteCoches;
            priorityText.fontStyle = FontStyle.Bold;

            GameManager.Instance.AddCocheIA(cocheIAObj);
            OtherCar otherCar = cocheIAObj.GetComponent<OtherCar>();
            WaypointNavigator wN = cocheIAObj.GetComponent<WaypointNavigator>();
            wN.SetInitialWaypoint(posicionesPiezas, indexPieza, IAcar.orientacion);


            otherCar.branchTo = IAcar.branchTo;
            otherCar.orientacion = IAcar.orientacion;
            otherCar.carID = id;
            id++;


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
            cuadriculaObj.transform.localScale = new Vector3(cuadriculaObj.transform.localScale.x * scale / 100, cuadriculaObj.transform.localScale.y * scale / 100, cuadriculaObj.transform.localScale.z * scale / 100);

            cuadriculaObj.SetActive(true);
        }
        //stops
        foreach (var sign in nivel.stops)
        {
            int indexPieza = sign.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
            posicionPieza.y = 1;

            //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
            Vector3 posicionCuadricula = ConvertToSubPosition(posicionPieza, sign.subPosicion.fil, sign.subPosicion.col, subScale, subdivisions);
            Quaternion prefabRotation = ConvertirOrientacionARotacion(sign.orientacion);
            posicionCuadricula.y = 1;
            GameObject stopObj = Instantiate(stopPrefab, posicionCuadricula, prefabRotation);
            stopObj.transform.localScale = new Vector3(stopObj.transform.localScale.x * scale / 100, stopObj.transform.localScale.y * scale / 100, stopObj.transform.localScale.z * scale / 100);

            stopObj.SetActive(true);
        }
        foreach (var sign in nivel.IADestroyer)
        {
            int indexPieza = sign.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
            posicionPieza.y = 1;

            //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
            Vector3 posicionCuadricula = ConvertToSubPosition(posicionPieza, sign.subPosicion.fil, sign.subPosicion.col, subScale, subdivisions);
            posicionCuadricula.y = 1;
            Quaternion prefabRotation = ConvertirOrientacionARotacion(sign.orientacion);
            GameObject stopObj = Instantiate(destroyer, posicionCuadricula, prefabRotation);
            stopObj.transform.localScale = new Vector3(stopObj.transform.localScale.x * scale / 100, stopObj.transform.localScale.y * scale / 100, stopObj.transform.localScale.z * scale / 100);

            stopObj.SetActive(true);
        }
        foreach (var sign in nivel.cedas)
        {
            int indexPieza = sign.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
            posicionPieza.y = 1;

            //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
            Vector3 posicionCuadricula = ConvertToSubPosition(posicionPieza, sign.subPosicion.fil, sign.subPosicion.col, subScale, subdivisions);
            posicionCuadricula.y = 1;
            Quaternion prefabRotation = ConvertirOrientacionARotacion(sign.orientacion);
            GameObject stopObj = Instantiate(cedaPrefab, posicionCuadricula, prefabRotation);
            stopObj.transform.localScale = new Vector3(stopObj.transform.localScale.x * scale / 100, stopObj.transform.localScale.y * scale / 100, stopObj.transform.localScale.z * scale / 100);

            stopObj.SetActive(true);
            if (ClicLevelManager.Instance != null)
            {
                stopObj.GetComponent<YieldSign>().enabled = false;
            }
        }
        foreach (var sign in nivel.iniLuces)
        {
            int indexPieza = sign.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
            posicionPieza.y = 1;

            //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
            Vector3 posicionCuadricula = ConvertToSubPosition(posicionPieza, sign.subPosicion.fil, sign.subPosicion.col, subScale, subdivisions);
            posicionCuadricula.y = 1;
            Quaternion prefabRotation = ConvertirOrientacionARotacion(sign.orientacion);
            GameObject stopObj = Instantiate(iniLuzPrefab, posicionCuadricula, prefabRotation);
            stopObj.transform.localScale = new Vector3(stopObj.transform.localScale.x * scale / 100, stopObj.transform.localScale.y * scale / 100, stopObj.transform.localScale.z * scale / 100);

            stopObj.SetActive(true);
            
        }
        //prohibidos
        foreach (var sign in nivel.maxVelocidad)
        {
            int indexPieza = sign.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
            posicionPieza.y = 1;

            //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
            Vector3 posicionCuadricula = ConvertToSubPosition(posicionPieza, sign.subPosicion.fil, sign.subPosicion.col, subScale, subdivisions);
            posicionCuadricula.y = 1;
            Quaternion prefabRotation = ConvertirOrientacionARotacion(sign.orientacion);
            GameObject stopObj = Instantiate(MaxSpeedPrefab, posicionCuadricula, prefabRotation);
            stopObj.GetComponent<SenyalVelocidad>().setMaxVelocity(sign.velocidad);
            stopObj.transform.localScale = new Vector3(stopObj.transform.localScale.x * scale / 100, stopObj.transform.localScale.y * scale / 100, stopObj.transform.localScale.z * scale / 100);

            stopObj.SetActive(true);
        }
        foreach (var sign in nivel.frenteIzq)
        {
            int indexPieza = sign.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
            posicionPieza.y = 1;

            //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
            Vector3 posicionCuadricula = ConvertToSubPosition(posicionPieza, sign.subPosicion.fil, sign.subPosicion.col, subScale, subdivisions);
            posicionCuadricula.y = 1;
            Quaternion prefabRotation = ConvertirOrientacionARotacion(sign.orientacion);
            GameObject stopObj = Instantiate(FrenteIzqPrefab, posicionCuadricula, prefabRotation);
            stopObj.transform.localScale = new Vector3(stopObj.transform.localScale.x * scale / 100, stopObj.transform.localScale.y * scale / 100, stopObj.transform.localScale.z * scale / 100);

            stopObj.SetActive(true);
        }
        //prohibidos
        foreach (var sign in nivel.prohibidos)
        {
            int indexPieza = sign.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
            posicionPieza.y = 1;
            //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
            Vector3 posicionCuadricula = ConvertToSubPosition(posicionPieza, sign.subPosicion.fil, sign.subPosicion.col, subScale, subdivisions);
            Quaternion prefabRotation = ConvertirOrientacionARotacion(sign.orientacion);
            posicionCuadricula.y = 1;
            GameObject stopObj = Instantiate(forbiddenPrefab, posicionCuadricula, prefabRotation);
            stopObj.transform.localScale = new Vector3(stopObj.transform.localScale.x * scale / 100, stopObj.transform.localScale.y * scale / 100, stopObj.transform.localScale.z * scale / 100);

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
                semaforoObj.transform.localScale = new Vector3(semaforoObj.transform.localScale.x * scale / 100, semaforoObj.transform.localScale.y * scale / 100, semaforoObj.transform.localScale.z * scale / 100);

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
                semaforoObj.transform.localScale = new Vector3(semaforoObj.transform.localScale.x * scale / 100, semaforoObj.transform.localScale.y * scale / 100, semaforoObj.transform.localScale.z * scale / 100);

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
        else if (nivel.rain)
        {
            GameManager.Instance.EnableRain();

        }
        else if (nivel.night)
        {
            GameManager.Instance.SetNight();

        }
        GameManager.Instance.CurrentLevel = nivel.type;
        switch (nivel.type)
        {
            //case LevelType.Desconocido:
            //    break;
            //case LevelType.Conduccion:
            //    break;
            case "Prioridad":

                Debug.Log("Prioridad");
                GameObject managerInstance = Instantiate(ClicLevelManagerPref);
                managerInstance.GetComponent<ClicLevelManager>().correctOrderList = nivel.correctOrder;

                break;
            case "Luces":
                Debug.Log("nivel luces");
                //GameManager.Instance.antinieblaDelanteras.SetActive(true);
                //GameManager.Instance.antinieblaTraseras.SetActive(true);
                //GameManager.Instance.posicion.SetActive(true);
                //GameManager.Instance.largas.SetActive(true);
                //GameManager.Instance.cortas.SetActive(true);
                //GameObject cocheJugador = Instantiate(Resources.Load("CochePrefab"), new Vector3(nivel.posicionCoche.x, nivel.posicionCoche.y, nivel.posicionCoche.z), Quaternion.identity) as GameObject; 
                CarLights carLights = GameManager.Instance.carController.gameObject.GetComponent<CarLights>();
                if (carLights != null)
                {
                    carLights.objetivoLuces = nivel.objetivo;
                }
                // Activar modo deslumbramiento
                if (nivel.deslumbramiento)
                    GameManager.Instance.SetDeslumbramiento();


                break;
        }
        GameManager.Instance.SetCurrentLevel(nivel.nivel);
    }
    private void CrearTipoPiezas(List<PosicionMapa> piezas, string prefabName, MapaNuevo nivel, Transform conjuntoPiezas, float scale, Dictionary<int, GameObject> posicionesPiezas, Digrafo digrafo)
    {
        foreach (var recta in piezas)
        {
            //Convertir la posición
            Vector3 posicion = ConvertToPosition(recta.fil, recta.col, scale);

            //Cargar el prefab correspondiente
            GameObject rectaPrefab = Resources.Load<GameObject>($"PiezasPrefabs/{prefabName}");
            if (rectaPrefab != null)
            {
                //Asignar ID y crear instancia
                recta.id = recta.fil * nivel.columnas + recta.col;
                posicionesPiezas[recta.id] = Instantiate(rectaPrefab, posicion, Quaternion.identity, conjuntoPiezas);
                posicionesPiezas[recta.id].transform.localScale = new Vector3(scale / 100, scale / 100, scale / 100);
            }
            else
            {
                Debug.LogError($"No se encontró el prefab: {prefabName}");
            }

            //Añadir conexiones al grafo
            foreach (var conn in recta.conexiones)
            {
                digrafo.ponArista(recta.id, conn);
            }
        }
    }

}


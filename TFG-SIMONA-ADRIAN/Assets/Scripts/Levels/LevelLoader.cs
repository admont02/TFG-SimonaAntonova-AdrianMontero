using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Unity.AI.Navigation;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
//using Xasu.HighLevel;
/// <summary>
/// Clase encargada de la creaci�n de los elementos de la escena en Unity a partir del contenido de un archivo JSON.
/// </summary>
public class LevelLoader : MonoBehaviour
{
    //prefabs de las piezas
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
    private float cuadriculaSize = 2.5f; //Tama�o de cada cuadr�cula dentro de una pieza
    //Padre de las piezas
    [SerializeField]
    public Transform conjuntoPiezas;
    [SerializeField]
    GameObject FrentePrefab;
    [SerializeField]
    GameObject FrenteDchaPrefab;

    public void CargarNivel()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, SceneData.JsonFileName);
        filePath = filePath.Replace("\\", "/"); //Reemplaza las barras invertidas por barras normales
        //creaci�n del nivel
        Debug.Log("Ruta del archivo JSON: " + filePath);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Nivel nivelData = JsonUtility.FromJson<Nivel>(json);
            
            CrearNivel(nivelData);
            Debug.Log(" se encontr� el archivo JSON.");
        }
        else
        {
            filePath = Path.Combine(Application.streamingAssetsPath, "Editor/" + SceneData.JsonFileName); 
            filePath = filePath.Replace("\\", "/"); // Reemplaza las barras invertidas por barras normales
            Debug.Log("Ruta del archivo JSON: " + filePath);
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                Nivel nivelData = JsonUtility.FromJson<Nivel>(json);
               
                CrearNivel(nivelData);
                Debug.Log(" se encontr� el archivo JSON.");
            }
            else
                Debug.Log("No se encontr� el archivo JSON.");
        }
    }
    /// <summary>
    /// M�todo encargado de convertir el esquema {fil,col} a una posici�n de Unity
    /// </summary>
    /// <param name="fil"></param>
    /// <param name="col"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    Vector3 ConvertToPosition(int fil, int col, float scale)
    {
        float z = scale / 2 + scale * fil;
        float x = scale / 2 + scale * col;
        return new Vector3(x, 0, z);
    }
    /// <summary>
    /// M�todo encargado de convertir el esquema {fil,col} dentro de una pieza a una posicion de Unity
    /// </summary>
    /// <param name="basePosition"></param>
    /// <param name="subFil"></param>
    /// <param name="subCol"></param>
    /// <param name="subScale"></param>
    /// <param name="subdivisions"></param>
    /// <returns></returns>
    Vector3 ConvertToSubPosition(Vector3 basePosition, int subFil, int subCol, float subScale, int subdivisions)
    {
        float subdivisionSize = subScale / subdivisions; //Tama�o de cada subdivisi�n
        float offsetX = -subScale / 2 + cuadriculaSize + subdivisionSize * subFil;
        float offsetZ = -subScale / 2 + cuadriculaSize + subdivisionSize * subCol;
        return basePosition + new Vector3(offsetX, 2.81f, offsetZ);
    }

    /// <summary>
    /// Transformaci�n de la orientaci�n de un elemento a una rotaci�n en Unity
    /// </summary>
    /// <param name="orientacion"></param>
    /// <returns></returns>
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
                Debug.LogWarning("Orientaci�n desconocida, usando rotaci�n por defecto.");
                return Quaternion.identity;
        }
    }
    /// <summary>
    /// M�todo encargado de leer el JSON y crear los elementos
    /// </summary>
    /// <param name="nivel"></param>
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
        //grafo de conexiones
        Digrafo digrafo = new Digrafo(nodos);
        // Crear el punto objetivo
        if (!nivel.isMenu)
        {
            //creacion de las piezas pertinentes que conforman el nivel
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
            CrearTipoPiezas(nivel.mapa.Pavement_1, "Pavement_1", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Pavement_2, "Pavement_2", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Pavement_3, "Pavement_3", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Pavement_4, "Pavement_4", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.TunnelVertical, "TunnelVertical", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.TunnelHorizontal, "TunnelHorizontal", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Grass, "Grass", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);
            CrearTipoPiezas(nivel.mapa.Grass_2, "Grass_2", nivel.mapa, conjuntoPiezas, scale, posicionesPiezas, digrafo);

            //creacion del vehiculo del jugador
            if (nivel.jugador.pieza != null && nivel.type!="Prioridad")
            {
                //pieza en la que se encuentra
                int indexPieza = nivel.jugador.pieza.index;
                if (indexPieza >= 0 && indexPieza < posicionesPiezas.Count)
                {
                    Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
                    //pos del jugador dentro de la pieza
                    Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugador.subPosicion.fil, nivel.jugador.subPosicion.col, subScale, subdivisions);
                    //rotacion del jugador
                    Quaternion rotPlayer = ConvertirOrientacionARotacion(nivel.jugador.orientacion);
                    GameObject playerObj = Instantiate(playerPrefab, posicionJugador, rotPlayer);
                    playerObj.transform.localScale = new Vector3(playerObj.transform.localScale.x * scale / 100, playerObj.transform.localScale.y * scale / 100, playerObj.transform.localScale.z * scale / 100);
                    playerObj.SetActive(true);

                    GameManager.Instance.carController = playerObj.GetComponent<PrometeoCarController>();
                    GameManager.Instance.SetPlayer(playerObj.transform);
                }
                else
                {
                    Debug.LogError("No se encontr� la pieza especificada para el jugador.");
                }

                //creacion del punto objetivo del nivel
                Vector3 posicionPiezaTarget = posicionesPiezas[nivel.targetJugador.pieza.index].transform.position;
                Vector3 posicionTarget = ConvertToSubPosition(posicionPiezaTarget, nivel.targetJugador.subPosicion.fil, nivel.targetJugador.subPosicion.col, subScale, subdivisions);
                GameObject targetPoint = Instantiate(TargetPrefab, posicionTarget, Quaternion.identity);
                targetPoint.SetActive(true);
                GameManager.Instance.SetPlayerTarget(targetPoint);

                LineRenderer lineRenderer = targetPoint.GetComponent<LineRenderer>();
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, targetPoint.transform.position);
                lineRenderer.SetPosition(1, targetPoint.transform.position + new Vector3(0, 50.0f, 0));
                //icono minimapa
                Vector3 iconPosition = posicionTarget;
                iconPosition.y += 30.0f; 
                Quaternion rotacionTarget = Quaternion.Euler(100, 0, 0);
                GameObject minimapIcon = Instantiate(TargetIconPrefab, iconPosition, rotacionTarget);
                minimapIcon.SetActive(true);


                //gps, ya no se usa
                //GPSController gpsController = playerObjAux.GetComponent<GPSController>();
                //gpsController.Initialize(digrafo, posicionesPiezas, nivel.jugadorNuevo.pieza.index, nivel.targetJugador.pieza.index);

            }
        }
        //asignacion del digrafo de las conexiones y los dialogos
        GameManager.Instance.graph = digrafo;
        GameManager.Instance.dialogueSystem.SetLevelDialog(nivel.levelDialogs, nivel.completedDialogs, nivel.wrongDialogs);


        //vehiculos no controlables
        int id = 0;
        foreach (var IAcar in nivel.IACars)
        {
            Quaternion rotation = ConvertirOrientacionARotacion(IAcar.orientacion);
            int indexPieza = IAcar.pieza.index;

            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
            //posicion coche dentro de la pieza
            Vector3 posicionCoche = ConvertToSubPosition(posicionPieza, IAcar.subPosicion.fil, IAcar.subPosicion.col, subScale, subdivisions);
            GameObject cocheIAObj;
            //distincion del tipo de vehiculo (turismo,ambulancia,bus)
            if (IAcar.vehicle == "ambulance")
            {
                cocheIAObj = Instantiate(ambulancePrefab, posicionCoche, rotation);
                //situacion de emergencia
                if (IAcar.emergency)
                {
                    cocheIAObj.GetComponent<Ambulance>().redLight.enabled = true;
                    cocheIAObj.GetComponent<Ambulance>().lightsOn = true;
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

            //texto de prioridad
            GameObject priorityTextObj = new GameObject("PriorityText");
            priorityTextObj.transform.SetParent(cocheIAObj.transform);
            if (IAcar.vehicle == "bus")
            {
                priorityTextObj.transform.localPosition = new Vector3(0, 4.5f, 0);
                priorityTextObj.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (IAcar.vehicle == "ambulance")
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
            priorityText.fontStyle = FontStyle.Bold;

            GameManager.Instance.AddCocheIA(cocheIAObj);
            OtherCar otherCar = cocheIAObj.GetComponent<OtherCar>();
            //set del waypoint inicial
            WaypointNavigator wN = cocheIAObj.GetComponent<WaypointNavigator>();
            wN.SetInitialWaypoint(posicionesPiezas, indexPieza, IAcar.orientacion);

            //rama que debe tomar (solo niveles de prioridad)
            otherCar.branchTo = IAcar.branchTo;
            otherCar.orientacion = IAcar.orientacion;
            otherCar.carID = id;
            id++;


        }
        //creacion de se�ales y marcas viales
        //cuadriculas amarillas
        foreach (var cuadricula in nivel.cuadriculas)
        {
            int indexPieza = cuadricula.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
            //pos
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
            //pos
            Vector3 posicionStop = ConvertToSubPosition(posicionPieza, sign.subPosicion.fil, sign.subPosicion.col, subScale, subdivisions);
            Quaternion prefabRotation = ConvertirOrientacionARotacion(sign.orientacion);
            posicionStop.y = 1;
            GameObject stopObj = Instantiate(stopPrefab, posicionStop, prefabRotation);
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
        //cedas
        foreach (var sign in nivel.cedas)
        {
            int indexPieza = sign.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
            posicionPieza.y = 1;
            //pos
            Vector3 posicionCeda = ConvertToSubPosition(posicionPieza, sign.subPosicion.fil, sign.subPosicion.col, subScale, subdivisions);
            posicionCeda.y = 1;
            Quaternion prefabRotation = ConvertirOrientacionARotacion(sign.orientacion);
            GameObject stopObj = Instantiate(cedaPrefab, posicionCeda, prefabRotation);
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
        foreach (var sign in nivel.frente)
        {
            int indexPieza = sign.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
            posicionPieza.y = 1;

            //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
            Vector3 posicionCuadricula = ConvertToSubPosition(posicionPieza, sign.subPosicion.fil, sign.subPosicion.col, subScale, subdivisions);
            posicionCuadricula.y = 1;
            Quaternion prefabRotation = ConvertirOrientacionARotacion(sign.orientacion);
            GameObject stopObj = Instantiate(FrentePrefab, posicionCuadricula, prefabRotation);
            stopObj.transform.localScale = new Vector3(stopObj.transform.localScale.x * scale / 100, stopObj.transform.localScale.y * scale / 100, stopObj.transform.localScale.z * scale / 100);

            stopObj.SetActive(true);
        }
        foreach (var sign in nivel.frenteDcha)
        {
            int indexPieza = sign.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
            posicionPieza.y = 1;

            //Vector3 posicionJugador = ConvertToSubPosition(posicionPieza, nivel.jugadorNuevo.subPosicion.fil, nivel.jugadorNuevo.subPosicion.col, subScale);
            Vector3 posicionCuadricula = ConvertToSubPosition(posicionPieza, sign.subPosicion.fil, sign.subPosicion.col, subScale, subdivisions);
            posicionCuadricula.y = 1;
            Quaternion prefabRotation = ConvertirOrientacionARotacion(sign.orientacion);
            GameObject stopObj = Instantiate(FrenteDchaPrefab, posicionCuadricula, prefabRotation);
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
        //SEMAFOROS 
        foreach (var semaforo in nivel.semaforos)
        {
            Quaternion rotation = ConvertirOrientacionARotacion(semaforo.orientacion);
            int indexPieza = semaforo.pieza.index;


            Vector3 posicionPieza = posicionesPiezas[indexPieza].transform.position;
            //pos
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
                semaforoScript.redSeconds = semaforo.redSeconds; //configurar la luz inicial
                semaforoScript.red.SetActive(semaforo.initialLight == "red");
                semaforoScript.amber.SetActive(semaforo.initialLight == "amber");
                semaforoScript.green.SetActive(semaforo.initialLight == "green");
            }

        }

        //configuraciones climaticas y ambientales
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
        //tipo de nivel
        switch (nivel.type)
        {
            
            case "Prioridad":

                Debug.Log("Prioridad");
                GameObject managerInstance = Instantiate(ClicLevelManagerPref);
                managerInstance.GetComponent<ClicLevelManager>().correctOrderList = nivel.correctOrder;

                break;
            case "Luces":
                Debug.Log("nivel luces");
                CarLights carLights = GameManager.Instance.carController.gameObject.GetComponent<CarLights>();
                if (carLights != null)
                {
                    carLights.objetivoLuces = nivel.objetivo;
                }
                //activar modo deslumbramiento
                if (nivel.deslumbramiento)
                    GameManager.Instance.SetDeslumbramiento();


                break;
        }
        GameManager.Instance.SetCurrentLevel(nivel.nivel);
    }
    /// <summary>
    /// M�todo encargado de crear el tipo de pieza correspondiente en la posicion indicada de la escena
    /// </summary>
    /// <param name="piezas"></param>
    /// <param name="prefabName"></param>
    /// <param name="nivel"></param>
    /// <param name="conjuntoPiezas"></param>
    /// <param name="scale"></param>
    /// <param name="posicionesPiezas"></param>
    /// <param name="digrafo"></param>
    private void CrearTipoPiezas(List<PosicionMapa> piezas, string prefabName, MapaNuevo nivel, Transform conjuntoPiezas, float scale, Dictionary<int, GameObject> posicionesPiezas, Digrafo digrafo)
    {
        foreach (var recta in piezas)
        {
            //Convertir la posici�n
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
                Debug.LogError($"No se encontr� el prefab: {prefabName}");
            }

            //A�adir conexiones al grafo
            foreach (var conn in recta.conexiones)
            {
                digrafo.ponArista(recta.id, conn);
            }
        }
    }

}


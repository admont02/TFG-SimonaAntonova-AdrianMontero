using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LevelLoader : MonoBehaviour
{
    public string jsonFileName = "nivel2.json";

    public GameObject TargetPrefab;
    [SerializeField]
    GameObject cocheIAPrefab;
    [SerializeField]
    GameObject ClicLevelManagerPref;
    [SerializeField]
    GameObject cuadriculaPrefab;
    [SerializeField]
    GameObject semaforoPrefab;
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

    void CrearNivel(Nivel nivel)
    {
        
        // Crear el punto objetivo
        if (!nivel.isMenu)
        {
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
            GameObject semaforoObj = Instantiate(semaforoPrefab, new Vector3(semaforo.posicion.x, semaforo.posicion.y, semaforo.posicion.z), rotation);
            
            SimpleTrafficLight semaforoScript = semaforoObj.GetComponent<SimpleTrafficLight>();
            semaforoScript.greenSeconds = semaforo.greenSeconds;
            semaforoScript.amberSeconds = semaforo.amberSeconds;
            semaforoScript.redSeconds = semaforo.redSeconds; // Configurar la luz inicial
            semaforoScript.red.SetActive(semaforo.initialLight == "red");
            semaforoScript.amber.SetActive(semaforo.initialLight == "amber");
            semaforoScript.green.SetActive(semaforo.initialLight == "green");
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


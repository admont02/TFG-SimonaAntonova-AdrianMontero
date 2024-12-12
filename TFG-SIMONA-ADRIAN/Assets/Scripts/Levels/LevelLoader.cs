using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelLoader : MonoBehaviour
{
    public string jsonFileName = "nivel1.json";

    public void CargarNivel()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);
        filePath = filePath.Replace("\\", "/"); // Reemplaza las barras invertidas por barras normales
        Debug.Log("Ruta del archivo JSON: " + filePath);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Nivel nivelData = JsonUtility.FromJson<Nivel>(json);
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
        // Crear el coche del jugador
        //GameObject cocheJugador = Instantiate(Resources.Load("CochePrefab"), new Vector3(nivel.posicionCoche.x, nivel.posicionCoche.y, nivel.posicionCoche.z), Quaternion.identity) as GameObject;

        //// Crear coches IA
        //foreach (var cocheIA in nivel.cochesIA)
        //{
        //    foreach (var pos in cocheIA.posiciones)
        //    {
        //        GameObject cocheIAObj = Instantiate(Resources.Load("CocheIAPrefab"), new Vector3(pos.x, pos.y, pos.z), Quaternion.identity) as GameObject;

        //    }
        //}

        //// Crear elementos del mapa
        //foreach (var elemento in nivel.elementosMapa)
        //{
        //    GameObject elementoObj = Instantiate(Resources.Load(elemento.tipo + "Prefab"), new Vector3(elemento.posicion.x, elemento.posicion.y, elemento.posicion.z), Quaternion.identity) as GameObject;
        //}
        // Crear el punto objetivo
        GameObject targetPoint = new GameObject("TargetPoint"); 
        targetPoint.transform.position = new Vector3(nivel.targetJugador.x, nivel.targetJugador.y, nivel.targetJugador.z); 
        LineRenderer lineRenderer = targetPoint.AddComponent<LineRenderer>(); 
        lineRenderer.positionCount = 2; 
        lineRenderer.SetPosition(0, targetPoint.transform.position); 
        lineRenderer.SetPosition(1, targetPoint.transform.position + new Vector3(0, 100.0f, 0));
        GameManager.Instance.dialogueSystem.SetLevelDialog(nivel.levelDialogs);
        OtherCar otherCar = FindObjectOfType<OtherCar>(); 
        if (otherCar != null && nivel.cochesIA.Count > 0 && nivel.cochesIA[0].posiciones.Count > 0) 
        { 
            List<Vector3> destinations = new List<Vector3>(); 
            foreach (var pos in nivel.cochesIA[0].posiciones) 
            { 
                destinations.Add(new Vector3(pos.x, pos.y, pos.z)); 
            } 
            otherCar.SetDestinations(destinations); 
        }
    }

}


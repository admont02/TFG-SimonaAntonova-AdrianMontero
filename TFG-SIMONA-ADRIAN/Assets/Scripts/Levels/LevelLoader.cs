using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelLoader : MonoBehaviour
{
    public string jsonFileName = "nivel2.json";

    public GameObject TargetPrefab;
    [SerializeField]
    GameObject cocheIAPrefab;

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
        GameObject targetPoint = Instantiate(TargetPrefab, new Vector3(nivel.targetJugador.x, nivel.targetJugador.y, nivel.targetJugador.z), Quaternion.identity);
        targetPoint.SetActive(true);
        GameManager.Instance.SetPlayerTarget(targetPoint);
        //targetPoint.transform.position = new Vector3(nivel.targetJugador.x, nivel.targetJugador.y, nivel.targetJugador.z);
        LineRenderer lineRenderer = targetPoint.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, targetPoint.transform.position);
        lineRenderer.SetPosition(1, targetPoint.transform.position + new Vector3(0, 100.0f, 0));
        GameManager.Instance.dialogueSystem.SetLevelDialog(nivel.levelDialogs, nivel.completedDialogs);
        //OtherCar otherCar = FindObjectOfType<OtherCar>();
        //if (otherCar != null && nivel.cochesIA.Count > 0 && nivel.cochesIA[0].posiciones.Count > 0)
        //{
        //    List<Vector3> destinations = new List<Vector3>();
        //    foreach (var pos in nivel.cochesIA[0].posiciones)
        //    {
        //        destinations.Add(new Vector3(pos.x, pos.y, pos.z));
        //    }
        //    otherCar.SetDestinations(destinations);
        //}
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
        if (nivel.fog)
        {
            GameManager.Instance.EnableFog();
        }
    }

}


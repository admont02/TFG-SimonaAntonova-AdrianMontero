//using UnityEngine;
//using System.Collections.Generic;

//public class GPSController : MonoBehaviour
//{
//    public Transform player; 
//    public Transform target; 
//    public GameObject arrowPrefab;
//    private GameObject arrowInstance;
//    private List<Node> path;
//    private int currentPathIndex;
//    private Digrafo graph;
//    private Dictionary<int, Vector3> posicionesPiezas; 
//    private int initialNodeId;
//    private int targetNodeId;

//    public void Initialize(Digrafo graph, Dictionary<int, Vector3> posicionesPiezas, int initialNodeId, int targetNodeId)
//    {
//        this.graph = graph;
//        this.posicionesPiezas = posicionesPiezas;
//        this.initialNodeId = initialNodeId;
//        this.targetNodeId = targetNodeId;
//        this.player = GameManager.Instance.carController.gameObject.transform;
//        SetupPath();
//    }

//    private void SetupPath()
//    {
//        if (arrowPrefab != null && player != null)
//        {
//            arrowInstance = Instantiate(arrowPrefab, player.transform);
//            arrowInstance.transform.localPosition = new Vector3(0, 2, 2); 
//        }

//        Node startNode = new Node(initialNodeId);
//        Node goalNode = new Node(targetNodeId);

//        path = NavigationUtils.AStar(graph, startNode, goalNode);
//        currentPathIndex = 0;
//    }

//    void Update()
//    {
//        if (arrowInstance != null && path != null && path.Count > 0)
//        {
//            if (Vector3.Distance(player.position, posicionesPiezas[path[currentPathIndex].Id]) < 1.0f && currentPathIndex < path.Count - 1)
//            {
//                currentPathIndex++;
//            }

//            Vector3 direction = posicionesPiezas[path[currentPathIndex].Id] - player.position;
//            direction.y = 0;
//            Quaternion rotation = Quaternion.LookRotation(direction);
//            arrowInstance.transform.rotation = Quaternion.Slerp(arrowInstance.transform.rotation, rotation, Time.deltaTime * 5f);
//        }
//    }
//}
using UnityEngine;
using System.Collections.Generic;

public class GPSController : MonoBehaviour
{
    public Transform player; // Referencia al coche del jugador
    public Transform target; // Referencia al objetivo del jugador
    public GameObject pathRendererPrefab; // Prefab del LineRenderer
    public int initialNodeId; // ID inicial en el grafo
    public int targetNodeId; // ID del nodo objetivo
    public Digrafo graph; // Referencia al grafo
    private List<Node> path;
    private int currentPathIndex;
    private Dictionary<int, Vector3> posicionesPiezas; // Mapa de posiciones de las piezas
    private GameObject pathRendererObject; // Instancia del LineRenderer
    private LineRenderer lineRenderer;
    private float recalculationInterval = 1.0f; // Intervalo de tiempo para recalcular la ruta en segundos
    private float timeSinceLastRecalculation = 0.0f;

    public void Initialize(Digrafo graph, Dictionary<int, Vector3> posicionesPiezas, int initialNodeId, int targetNodeId)
    {
        this.graph = graph;
        this.posicionesPiezas = posicionesPiezas;
        this.initialNodeId = initialNodeId;
        this.targetNodeId = targetNodeId;
        this.player = GameManager.Instance.carController.gameObject.transform;
        SetupPath();
    }

    private void SetupPath()
    {
        Node startNode = new Node(initialNodeId);
        Node goalNode = new Node(targetNodeId);
        path = NavigationUtils.AStar(graph, startNode, goalNode);
        currentPathIndex = 0;

        if (path != null && path.Count > 0)
        {
            CreatePathRenderer();
        }
    }

    private void CreatePathRenderer()
    {
        if (pathRendererObject != null)
        {
            Destroy(pathRendererObject);
        }

        pathRendererObject = Instantiate(pathRendererPrefab);
        lineRenderer = pathRendererObject.GetComponent<LineRenderer>();

        lineRenderer.positionCount = path.Count;

        for (int i = 0; i < path.Count; i++)
        {
            lineRenderer.SetPosition(i, posicionesPiezas[path[i].Id]);
        }
    }

    private void RecalculatePath()
    {
        // Recalcula el nodo inicial basado en la posición actual del jugador
        Node startNode = GetClosestNode(player.position);
        Node goalNode = new Node(targetNodeId);
        path = NavigationUtils.AStar(graph, startNode, goalNode);

        if (path != null && path.Count > 0)
        {
            CreatePathRenderer();
        }
    }

    private Node GetClosestNode(Vector3 position)
    {
        float closestDistance = float.MaxValue;
        int closestNodeId = initialNodeId;

        foreach (var kvp in posicionesPiezas)
        {
            float distance = Vector3.Distance(position, kvp.Value);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNodeId = kvp.Key;
            }
        }

        return new Node(closestNodeId);
    }

    void Update()
    {
        timeSinceLastRecalculation += Time.deltaTime;

        if (timeSinceLastRecalculation >= recalculationInterval)
        {
            RecalculatePath();
            timeSinceLastRecalculation = 0.0f;
        }

        if (lineRenderer != null && path != null && path.Count > 0)
        {
            if (Vector3.Distance(player.position, posicionesPiezas[path[currentPathIndex].Id]) < 1.0f && currentPathIndex < path.Count - 1)
            {
                currentPathIndex++;
            }

            Vector3 direction = posicionesPiezas[path[currentPathIndex].Id] - player.position;
            direction.y = 0; // Mantener la flecha en el plano horizontal
            Quaternion rotation = Quaternion.LookRotation(direction);
            lineRenderer.transform.rotation = Quaternion.Slerp(lineRenderer.transform.rotation, rotation, Time.deltaTime * 5f);
        }
    }
}


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
    private Dictionary<int, GameObject> posicionesPiezas; // Mapa de posiciones de las piezas
    private GameObject pathRendererObject; // Instancia del LineRenderer
    private LineRenderer lineRenderer;
    private float recalculationInterval = 1.0f; // Intervalo de tiempo para recalcular la ruta en segundos
    private float timeSinceLastRecalculation = 0.0f;

    public void Initialize(Digrafo graph_, Dictionary<int, GameObject> posicionesPiezas_, int initialNodeId_, int targetNodeId_)
    {
        graph = graph_;
        posicionesPiezas = posicionesPiezas_;
        initialNodeId = initialNodeId_;
        targetNodeId = targetNodeId_;
        player = GameManager.Instance.carController.gameObject.transform;
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

    //private void CreatePathRenderer()
    //{
    //    if (pathRendererObject != null)
    //    {
    //        Destroy(pathRendererObject);
    //    }

    //    pathRendererObject = Instantiate(pathRendererPrefab);
    //    lineRenderer = pathRendererObject.GetComponent<LineRenderer>();


    //    int i = 0;
    //    for (int j = 0; j < path.Count; j++)
    //    {
    //        //path[j]
    //        if (posicionesPiezas[path[j].Id].GetComponent<WaypointContainer>().GetWaypoints() != null)
    //            posicionesPiezas[path[j].Id].GetComponent<WaypointContainer>().Calculate();


    //        var currentWaypointContainer = posicionesPiezas[path[j].Id].GetComponent<WaypointContainer>();
    //        var waypoints = currentWaypointContainer.GetWaypoints();
    //        Waypoint aux = waypoints.GetComponent<Waypoint>();

    //        while (aux.next != null)
    //        {
    //            if (aux.branches.Count > 0)
    //            {
    //                if (j + 1 <= path.Count)
    //                    if (CheckWaypointDistance(aux.branches[0], aux.next, posicionesPiezas[path[j + 1].Id].transform.position))
    //                    {

    //                        Waypoint correctBranch = GetCorrectBranch(aux);

    //                        if (correctBranch != null)
    //                        {

    //                            lineRenderer.SetPosition(i, correctBranch.transform.position);
    //                            i++;


    //                            var nextWaypoint = correctBranch.next;
    //                            while (nextWaypoint != null)
    //                            {
    //                                lineRenderer.SetPosition(i, nextWaypoint.transform.position);
    //                                i++;
    //                                nextWaypoint = nextWaypoint.next;
    //                            }
    //                            break;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        lineRenderer.SetPosition(i, aux.GetPosition());
    //                        i++;
    //                    }
    //            }
    //            else
    //            {
    //                lineRenderer.SetPosition(i, aux.GetPosition());
    //                i++;
    //            }
    //            if (aux.next != null)
    //            {
    //                aux = aux.next;
    //                aux.previous.next = null;
    //            }
    //        }
    //    }
    //}
    private bool CheckWaypointDistance(Waypoint a, Waypoint b, Vector3 pos)
    {
        return (Vector3.Distance(a.GetPosition(), pos) < Vector3.Distance(a.GetPosition(), pos));


    }

    //Método para obtener la rama correcta hacia el destino
    //private Waypoint GetCorrectBranch(Waypoint waypoint)
    //{

    //    Node goalNode = new Node(targetNodeId);
    //    Vector3 destinationPosition = posicionesPiezas[goalNode.Id].transform.position;


    //    Vector3 directionToDestination = destinationPosition - waypoint.transform.position;


    //    Waypoint correctBranch = null;
    //    float maxDotProduct = float.MinValue;


    //    foreach (var branch in waypoint.branches)
    //    {

    //        Vector3 directionToBranch = branch.transform.position - waypoint.transform.position;


    //        float dotProduct = Vector3.Dot(directionToDestination.normalized, directionToBranch.normalized);


    //        if (dotProduct > maxDotProduct)
    //        {
    //            maxDotProduct = dotProduct;
    //            correctBranch = branch;
    //        }
    //    }

    //    return correctBranch;
    //}
    private void CreatePathRenderer()
    {
        if (pathRendererObject != null)
        {
            Destroy(pathRendererObject);
        }

        pathRendererObject = Instantiate(pathRendererPrefab);
        lineRenderer = pathRendererObject.GetComponent<LineRenderer>();

        List<Vector3> linePositions = new List<Vector3>();

        for (int i = 0; i < path.Count - 1; i++)
        {
            WaypointContainer currentWaypointContainer = posicionesPiezas[path[i].Id].GetComponent<WaypointContainer>();
            if (currentWaypointContainer.GetWaypoints() == null)
            {
                currentWaypointContainer.Calculate();
            }

            GameObject waypointObject = currentWaypointContainer.GetWaypoints();
            Waypoint waypointComponent = waypointObject.GetComponent<Waypoint>();

            if (waypointComponent == null)
            {
                continue; // Saltar si no hay waypoints
            }

            linePositions.Add(waypointComponent.transform.position);

            while (waypointComponent != null)
            {
                if (waypointComponent.branches.Count > 0)
                {
                    Waypoint correctBranch = GetCorrectBranch(waypointComponent, posicionesPiezas[path[i + 1].Id].transform.position);
                    if (correctBranch != null)
                    {
                        waypointComponent = correctBranch;
                    }
                    else
                    {
                        waypointComponent = waypointComponent.next;
                    }
                }
                else
                {
                    waypointComponent = waypointComponent.next;
                }

                if (waypointComponent != null)
                {
                    linePositions.Add(waypointComponent.transform.position);
                }
            }
        }

        // Adding the final target position
        linePositions.Add(posicionesPiezas[targetNodeId].transform.position);

        lineRenderer.positionCount = linePositions.Count;
        lineRenderer.SetPositions(linePositions.ToArray());
    }

    private Waypoint GetCorrectBranch(Waypoint waypoint, Vector3 destinationPosition)
    {
        Waypoint correctBranch = null;
        float minDistance = float.MaxValue;
        if (waypoint.next != null)
            minDistance = Vector3.Distance(waypoint.next.transform.position, destinationPosition);

        foreach (var branch in waypoint.branches)
        {
            float distance = Vector3.Distance(branch.transform.position, destinationPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                correctBranch = branch;
            }
        }

        return correctBranch;
    }

    private void RecalculatePath()
    {

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
            float distance = Vector3.Distance(position, kvp.Value.transform.position);
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
            if (Vector3.Distance(player.position, posicionesPiezas[path[currentPathIndex].Id].transform.position) < 1.0f && currentPathIndex < path.Count - 1)
            {
                currentPathIndex++;
            }

            Vector3 direction = posicionesPiezas[path[currentPathIndex].Id].transform.position - player.position;
            direction.y = 0; // Mantener la flecha en el plano horizontal
            Quaternion rotation = Quaternion.LookRotation(direction);
            lineRenderer.transform.rotation = Quaternion.Slerp(lineRenderer.transform.rotation, rotation, Time.deltaTime * 5f);
        }
    }
}


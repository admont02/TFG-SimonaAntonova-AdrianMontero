using UnityEngine;
using System.Collections.Generic;

public class GPSController : MonoBehaviour
{
    public Transform player; 
    public Transform target; 
    public GameObject arrowPrefab;
    private GameObject arrowInstance;
    private List<Node> path;
    private int currentPathIndex;
    private Digrafo graph;
    private Dictionary<int, Vector3> posicionesPiezas; 
    private int initialNodeId;
    private int targetNodeId;

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
        if (arrowPrefab != null && player != null)
        {
            arrowInstance = Instantiate(arrowPrefab, player.transform);
            arrowInstance.transform.localPosition = new Vector3(0, 2, 2); 
        }

        Node startNode = new Node(initialNodeId);
        Node goalNode = new Node(targetNodeId);
      
        path = NavigationUtils.AStar(graph, startNode, goalNode);
        currentPathIndex = 0;
    }

    void Update()
    {
        if (arrowInstance != null && path != null && path.Count > 0)
        {
            if (Vector3.Distance(player.position, posicionesPiezas[path[currentPathIndex].Id]) < 1.0f && currentPathIndex < path.Count - 1)
            {
                currentPathIndex++;
            }

            Vector3 direction = posicionesPiezas[path[currentPathIndex].Id] - player.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            arrowInstance.transform.rotation = Quaternion.Slerp(arrowInstance.transform.rotation, rotation, Time.deltaTime * 5f);
        }
    }
}

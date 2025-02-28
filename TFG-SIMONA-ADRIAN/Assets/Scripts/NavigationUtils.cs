using System.Collections.Generic;
using UnityEngine;

public static class NavigationUtils
{
    public static List<Node> AStar(Digrafo graph, Node start, Node goal)
    {
        var openSet = new PriorityQueue<Node>();
        var cameFrom = new Dictionary<Node, Node>();
        var gScore = new Dictionary<Node, float> { [start] = 0 };
        var fScore = new Dictionary<Node, float> { [start] = HeuristicCostEstimate(start, goal) };

        openSet.Enqueue(start, fScore[start]);

        HashSet<Node> closedSet = new HashSet<Node>();

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();

            if (current.Id == goal.Id)  
            {
                return ReconstructPath(cameFrom, current);
            }

            closedSet.Add(current);

            foreach (var neighborId in graph.getAdy(current.Id))
            {
                Node neighbor = new Node(neighborId);

                if (closedSet.Contains(neighbor))
                {
                    continue; 
                }

                float tentativeGScore = gScore[current] + Distance(current, neighbor);

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                    }
                }
            }
        }

        Debug.LogError("No se encontró un camino.");
        return null; 
    }

    private static float HeuristicCostEstimate(Node start, Node goal)
    {
        return Vector2Int.Distance(new Vector2Int(start.Id, 0), new Vector2Int(goal.Id, 0)); 
    }

    private static float Distance(Node a, Node b)
    {
        return Vector2Int.Distance(new Vector2Int(a.Id, 0), new Vector2Int(b.Id, 0)); 
    }

    private static List<Node> ReconstructPath(Dictionary<Node, Node> cameFrom, Node current)
    {
        var totalPath = new List<Node> { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }

        return totalPath;
    }
}

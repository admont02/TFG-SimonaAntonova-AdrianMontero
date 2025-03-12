using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
    // Start is called before the first frame update
    OtherCar controller;
    public Waypoint currentWaypoint;
    public bool randomDirection;
    int direction;
    int currentIndex;
    //esto deberia estar accesible en GameManager (tb se usa GPS)
    Dictionary<int, GameObject> posicionesPiezas;
    string initialOrientation;
    private void Awake()
    {
        controller = GetComponent<OtherCar>();
    }
    public void SetInitialWaypoint(Dictionary<int, GameObject> posicionesPiezas_, int index,string orientacion)
    {
        currentIndex = index;
        posicionesPiezas=posicionesPiezas_;
        currentWaypoint = posicionesPiezas[index].GetComponent<WaypointContainer>().GetWaypoint().GetComponent<Waypoint>();
        Direction dir = ParseOrientationToDirection(orientacion);
        currentWaypoint = GetClosestWaypointInDirection(posicionesPiezas[index], controller.transform.position,dir);
    }
    void Start()
    {
        if (randomDirection)
            direction = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
        controller.MoveToDestinations(currentWaypoint.GetPosition());
    }
    private int GetAdjacentPieceIndex(Direction direction)
    {
        //3 es numero magico mapa 3x3
        int newIndex = currentIndex;

        switch (direction)
        {
            case Direction.North:
                if (currentIndex + GameManager.Instance.filas < posicionesPiezas.Count) //Si no te sales del límite hacia el norte
                    newIndex = currentIndex + GameManager.Instance.filas;
                break;
            case Direction.South:
                if (currentIndex - GameManager.Instance.filas >= 0) //Si no te sales del límite hacia el sur
                    newIndex = currentIndex - GameManager.Instance.filas;
                break;
            case Direction.East:
                if ((currentIndex % GameManager.Instance.columnas) < 2) //Si no te sales del límite hacia el este (no estar en la última columna)
                    newIndex = currentIndex + 1;
                break;
            case Direction.West:
                if ((currentIndex % GameManager.Instance.columnas) > 0) //Si no te sales del límite hacia el oeste (no estar en la primera columna)
                    newIndex = currentIndex - 1;
                break;
        }
        //currentIndex = newIndex;
        return newIndex;
    }
    private Waypoint FindNextWaypointInAdjacentPiece(Direction oppositeDirection)
    {
        GameObject adjacentPiece= posicionesPiezas[currentIndex].GetComponent<WaypointContainer>().GetRoot();
        Waypoint[] waypoints = adjacentPiece.GetComponentsInChildren<Waypoint>();

        foreach (Waypoint waypoint in waypoints)
        {
            if (waypoint.direction == oppositeDirection)
            {
                return waypoint;  // Retorna el waypoint de entrada que corresponde con la dirección opuesta
            }
        }

        return null;  // Si no se encuentra, podría ser un error o un caso no esperado
    }
    // Update is called once per frame
    void Update()
    {
        if (controller.reachedDestination)
        {
            bool shouldBranch = false;
            if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
            {
                shouldBranch = UnityEngine.Random.Range(0f, 1f) <= currentWaypoint.branchRatio ? true : false;
            }
            if (shouldBranch)
            {
                currentWaypoint = currentWaypoint.branches[UnityEngine.Random.Range(0, currentWaypoint.branches.Count - 1)];
            }
            else
            {
                //if (direction == 0)
                //{
                if (currentWaypoint.next != null)
                {
                    currentWaypoint = currentWaypoint.next;

                }
                else
                {
                    
                 
                    Direction oppositeDirection = GetOppositeDirection(currentWaypoint.direction);
                    //int firstIndex = currentIndex;
                    currentIndex = GetAdjacentPieceIndex(currentWaypoint.direction);
                    //controller.IsConnected(firstIndex, currentIndex);
                    //Buscamos el siguiente waypoint en la pieza contigua
                    currentWaypoint = FindNextWaypointInAdjacentPiece(oppositeDirection);
                }

               
            }

            controller.MoveToDestinations(currentWaypoint.GetPosition());

        }

    }
    private Direction GetOppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.North: return Direction.South;
            case Direction.South: return Direction.North;
            case Direction.East: return Direction.West;
            case Direction.West: return Direction.East;
            default: return Direction.North;  // Valor predeterminado
        }
    }
    private Waypoint GetClosestWaypointInDirection(GameObject piece, Vector3 position, Direction direction)
    {
        Waypoint[] waypoints = piece.GetComponentsInChildren<Waypoint>();
        Waypoint closestWaypoint = null;
        float closestDistance = float.MaxValue;

        foreach (Waypoint waypoint in waypoints)
        {
            if (IsInDirection(position, waypoint.transform.position, direction))
            {
                float distance = Vector3.Distance(position, waypoint.transform.position);
                if (distance < closestDistance)
                {
                    closestWaypoint = waypoint;
                    closestDistance = distance;
                }
            }
        }

        return closestWaypoint;
    }

    private bool IsInDirection(Vector3 from, Vector3 to, Direction direction)
    {
        Vector3 directionVector = to - from;
        switch (direction)
        {
            case Direction.North: return directionVector.z > 0;
            case Direction.South: return directionVector.z < 0;
            case Direction.East: return directionVector.x > 0;
            case Direction.West: return directionVector.x < 0;
            default: return false;
        }
    }

    private Direction ParseOrientationToDirection(string orientation)
    {
        switch (orientation.ToLower())
        {
            case "arriba": return Direction.North;
            case "abajo": return Direction.South;
            case "derecha": return Direction.East;
            case "izquierda": return Direction.West;
            default: return Direction.North; 
        }
    }
}

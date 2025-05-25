using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Clase que utilizan los coches IA para la gestión de la ruta de Waypoints
/// </summary>
public class WaypointNavigator : MonoBehaviour
{
    
    OtherCar controller;
    public Waypoint currentWaypoint;
    public bool randomDirection;
    int direction;
    int currentIndex;
    
    Dictionary<int, GameObject> posicionesPiezas;
    string initialOrientation;
    private void Awake()
    {
        controller = GetComponent<OtherCar>();
    }
    /// <summary>
    /// Establece el Waypoint inicial del vehículo
    /// </summary>
    /// <param name="posicionesPiezas_"></param>
    /// <param name="index"></param>
    /// <param name="orientacion"></param>
    public void SetInitialWaypoint(Dictionary<int, GameObject> posicionesPiezas_, int index, string orientacion)
    {
        currentIndex = index;
        posicionesPiezas = posicionesPiezas_;
        currentWaypoint = posicionesPiezas[index].GetComponent<WaypointContainer>()?.GetWaypoint().GetComponent<Waypoint>();
        Direction dir = ParseOrientationToDirection(orientacion);
        if (currentWaypoint)
            currentWaypoint = GetClosestWaypointInDirection(posicionesPiezas[index], controller.transform.position, dir);
    }
    void Start()
    {
        if (randomDirection)
            direction = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
        controller.MoveToDestinations(currentWaypoint.GetPosition());
    }
    /// <summary>
    /// Devuelve la siguiente pieza a la que va a pasar el vehiculo
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
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
                //if ((currentIndex % GameManager.Instance.columnas) < 2) //Si no te sales del límite hacia el este (no estar en la última columna)
                newIndex = currentIndex + 1;
                break;
            case Direction.West:
                //if ((currentIndex % GameManager.Instance.columnas) > 0) //Si no te sales del límite hacia el oeste (no estar en la primera columna)
                newIndex = currentIndex - 1;
                break;
        }
        //currentIndex = newIndex;
        return newIndex;
    }
    /// <summary>
    /// Devuelve el primer Waypoint al que debe aproximarse el vehiculo en la nueva pieza
    /// </summary>
    /// <param name="exitDirection"></param>
    /// <param name="currentPosition"></param>
    /// <returns></returns>
    private Waypoint FindNearestWaypointInAdjacentPiece(Direction exitDirection, Vector3 currentPosition)
    {
        GameObject adjacentPiece = posicionesPiezas[currentIndex].GetComponent<WaypointContainer>().GetRoot();
        Waypoint[] waypoints = adjacentPiece.GetComponentsInChildren<Waypoint>();

        Waypoint nearestWaypoint = null;
        float shortestDistance = float.MaxValue;

        Direction entryDirection = GetOppositeDirection(exitDirection); //Direccion opuesta para conectar

        foreach (Waypoint waypoint in waypoints)
        {
            //Solo considera waypoints cuya entrada coincida con la dirección de salida del anterior
            if (waypoint.entryDirection == entryDirection)
            {
                float distance = Vector3.Distance(currentPosition, waypoint.GetPosition());
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestWaypoint = waypoint;
                }
            }
        }

        return nearestWaypoint;
    }


    // Update is called once per frame
    void Update()
    {
        if (controller.reachedDestination)
        {
            bool shouldBranch = false;
            int firstIndex = currentIndex;
            if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
            {
                for (int i = 0; i < currentWaypoint.branches.Count; i++)
                {
                    if (controller.IsConnected(firstIndex, GetAdjacentPieceIndex(currentWaypoint.branches[i].exitDirection)))
                    {
                        if (!ClicLevelManager.Instance)
                            shouldBranch = UnityEngine.Random.Range(0f, 1f) <= currentWaypoint.branchRatio ? true : false;
                        else
                        {
                            //if (currentWaypoint.branches[i].direction == controller.branchTo)
                            shouldBranch = true;
                        }
                    }


                }
            }
            if (shouldBranch)
            {
                if (!ClicLevelManager.Instance)
                {
                    var connectedBranches = currentWaypoint.branches.FindAll(branch =>
        controller.IsConnected(firstIndex, GetAdjacentPieceIndex(branch.exitDirection)));

                    if (connectedBranches.Count > 0)
                    {
                        //elegir una rama aleatoria entre las conectadas
                        currentWaypoint = connectedBranches[UnityEngine.Random.Range(0, connectedBranches.Count)];
                    }
                }
                //currentWaypoint = currentWaypoint.branches[UnityEngine.Random.Range(0, currentWaypoint.branches.Count - 1)];
                else
                {
                    currentWaypoint = currentWaypoint.branches[controller.branchTo];

                }
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
                    //Direction oppositeDirection = GetOppositeDirection(currentWaypoint.direction);
                    ////int firstIndex = currentIndex;
                    //currentIndex = GetAdjacentPieceIndex(currentWaypoint.direction);
                    ////controller.IsConnected(firstIndex, currentIndex);
                    ////Buscamos el siguiente waypoint en la pieza contigua
                    //currentWaypoint = FindNearestWaypointInAdjacentPiece(oppositeDirection, transform.position);
                    Direction exitDirection = currentWaypoint.exitDirection;
                    currentIndex = GetAdjacentPieceIndex(exitDirection);
                    currentWaypoint = FindNearestWaypointInAdjacentPiece(exitDirection, transform.position);
                }
            }

            controller.MoveToDestinations(currentWaypoint.GetPosition());

        }

    }
    /// <summary>
    /// Devuelve la direccion opuesta a la pasada por parametro
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Metodo empleado para coger el Waypoint mas cercano en la direccion de la ruta
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Metodo para verificar si un Waypoint corresponde con la direccion de la ruta
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Metodo para convertir orientacion en direccion
    /// </summary>
    /// <param name="orientation"></param>
    /// <returns></returns>
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

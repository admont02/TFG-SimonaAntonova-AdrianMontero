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
    private void Awake()
    {
        controller = GetComponent<OtherCar>();
    }
    public void SetInitialWaypoint(Dictionary<int, GameObject> posicionesPiezas_, int index)
    {
        currentIndex = index;
        posicionesPiezas=posicionesPiezas_;
        currentWaypoint = posicionesPiezas[index].GetComponent<WaypointContainer>().GetWaypoint().GetComponent<Waypoint>();
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
                if (currentIndex + 3 < posicionesPiezas.Count) //Si no te sales del l�mite hacia el norte
                    newIndex = currentIndex + 3;
                break;
            case Direction.South:
                if (currentIndex - 3 >= 0) //Si no te sales del l�mite hacia el sur
                    newIndex = currentIndex - 3;
                break;
            case Direction.East:
                if ((currentIndex % 3) < 2) //Si no te sales del l�mite hacia el este (no estar en la �ltima columna)
                    newIndex = currentIndex + 1;
                break;
            case Direction.West:
                if ((currentIndex % 3) > 0) //Si no te sales del l�mite hacia el oeste (no estar en la primera columna)
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
                return waypoint;  // Retorna el waypoint de entrada que corresponde con la direcci�n opuesta
            }
        }

        return null;  // Si no se encuentra, podr�a ser un error o un caso no esperado
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
                    //currentWaypoint = currentWaypoint.previous;
                    //direction = 1;
                 
                    Direction oppositeDirection = GetOppositeDirection(currentWaypoint.direction);
                    currentIndex = GetAdjacentPieceIndex(currentWaypoint.direction);

                    // Buscamos el siguiente waypoint en la pieza contigua
                    currentWaypoint = FindNextWaypointInAdjacentPiece(oppositeDirection);
                }

                //}
                //else if (direction == 1)
                //{
                //    if (currentWaypoint.previous != null)
                //    {
                //        currentWaypoint = currentWaypoint.previous;

                //    }
                //    else
                //    {


                //        currentWaypoint = currentWaypoint.next;
                //        direction = 0;
                //    }
                //}
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
}

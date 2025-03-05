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
    private void Awake()
    {
        controller = GetComponent<OtherCar>();
    }
    public void SetInitialWaypoint(Dictionary<int, GameObject> posicionesPiezas,int index)
    {
        currentWaypoint = posicionesPiezas[index].GetComponent<WaypointContainer>().GetWaypoint().GetComponent<Waypoint>();
    }
    void Start()
    {
        if (randomDirection)
            direction = Mathf.RoundToInt(Random.Range(0f, 1f));
        controller.MoveToDestinations(currentWaypoint.GetPosition());
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.reachedDestination)
        {
            bool shouldBranch = false;
            if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
            {
                shouldBranch = Random.Range(0f, 1f) <= currentWaypoint.branchRatio ? true : false;
            }
            if (shouldBranch)
            {
                currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count - 1)];
            }
            else
            {
                if (direction == 0)
                {
                    if (currentWaypoint.next != null)
                    {
                        currentWaypoint = currentWaypoint.next;

                    }
                    else
                    {
                        currentWaypoint = currentWaypoint.previous;
                        direction = 1;
                    }

                }
                else if (direction == 1)
                {
                    if (currentWaypoint.previous != null)
                    {
                        currentWaypoint = currentWaypoint.previous;

                    }
                    else
                    {
                        currentWaypoint = currentWaypoint.next;
                        direction = 0;
                    }
                }
            }

            controller.MoveToDestinations(currentWaypoint.GetPosition());

        }

    }
}

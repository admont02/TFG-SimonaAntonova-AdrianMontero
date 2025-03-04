using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointContainer : MonoBehaviour
{
    [SerializeField]
    private GameObject WaypointRoot;

    [SerializeField]
    private GameObject Waypoints;
    public void Calculate()
    {
        if (Waypoints == null)
            Waypoints = WaypointRoot.transform.GetChild(0).gameObject;

    }
    public GameObject GetWaypoints()
    {
        return Waypoints;
    }
}

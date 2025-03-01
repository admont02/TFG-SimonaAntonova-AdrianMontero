using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointContainer : MonoBehaviour
{
    [SerializeField]
    private GameObject WaypointRoot;

    private List<GameObject> Waypoints = new();
    public void Calculate()
    {
        for (int i = 0; i < WaypointRoot.transform.childCount; i++)
        {
            Waypoints.Add(WaypointRoot.transform.GetChild(i).gameObject);
        }
    }
    public List<GameObject> GetWaypoints()
    {
        return Waypoints;
    }
}

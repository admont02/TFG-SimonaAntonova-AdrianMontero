using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public enum Direction { NONE,North, South, East, West } //direccion del waypoint
/// <summary>
/// Clase que representa un Waypoint
/// </summary>
public class Waypoint : MonoBehaviour
{

    public Waypoint previous;
    public Waypoint next;

    [Range(0f, 10f)]
    public float width = 10f;

    public List<Waypoint> branches = new();

    [Range(0f, 1f)]
    public float branchRatio = 1.0f;

    public Direction entryDirection; //Dirección de entrada
    public Direction exitDirection; //Dirección de salida

    public Vector3 GetPosition()
    {
        return transform.position;
        Vector3 minBound = transform.position + transform.right * width / 2f;
        Vector3 maxBound = transform.position - transform.right * width / 2f;
        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
}

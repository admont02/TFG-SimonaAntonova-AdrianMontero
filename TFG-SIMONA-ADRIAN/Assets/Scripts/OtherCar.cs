using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OtherCar : MonoBehaviour
{
    protected List<Vector3> destinations = new List<Vector3>();
    //  public List<Transform> targets; // Lista de destinos
    protected int currentTargetIndex = 0;
    protected NavMeshAgent agent;
    protected float arrivalThreshold = 1.5f; // Umbral de distancia para considerar que ha llegado
    protected bool hasReachedFirstDestination = false;
    protected Material outline;
    public bool move = false;
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();

        if (destinations.Count > 0)
        {
            agent.destination = destinations[currentTargetIndex]; // Establecer el primer destino del coche
        }
    }

    void Update()
    {

        if (destinations.Count == 0 || !GameManager.Instance.canCarMove && !move)
        {

            agent.velocity = Vector3.zero;
            agent.isStopped = true;

            GetComponent<Rigidbody>().velocity = Vector3.zero; // Detiene el movimiento rb.angularVelocity =
            return;
        }
        agent.isStopped = false;

        if (!agent.pathPending && agent.remainingDistance <= arrivalThreshold && agent.velocity.sqrMagnitude == 0f && !agent.isStopped)
        {
            OnArrival();
        }
    }

    void OnArrival()
    {
        //Debug.Log("El coche ha llegado a su destino: " + currentTargetIndex);
        currentTargetIndex++;

        if (currentTargetIndex < destinations.Count)
        {
            hasReachedFirstDestination = true;

            //agent.destination = targets[currentTargetIndex].position; // Establecer el siguiente destino
            agent.SetDestination(destinations[currentTargetIndex]);
        }
        else if (!agent.isStopped)
        {
            // Debug.Log("El coche ha llegado a todos sus destinos.");
            DetenerMovimiento(); // Detener el movimiento del coche
        }
    }

    void DetenerMovimiento()
    {
        agent.isStopped = true;
        move = false;
        agent.destination = agent.transform.position; // Establecer el destino a su posición actual
    }
    public bool HasArrived()
    {
        return currentTargetIndex >= destinations.Count;
    }
    public void SetDestinations(List<Vector3> newDestinations)
    {
        destinations = newDestinations;
        //if (destinations != null && destinations.Count > 0)
        //{
        //    agent.destination = destinations[currentTargetIndex]; 
        //}
    }
    void OnMouseDown()
    { // Cuando se toca este coche, pasar el orden al GameController gameController.CarTouched(carOrder); }
        Debug.Log(this.name);
        ////gameObject.GetComponentInChildren<MeshCollider>().gameObject.GetComponent<MeshRenderer>().material = outline;
        //if (!GameManager.Instance.priorityCarList.Contains(gameObject))
        //    GameManager.Instance.priorityCarList.Add(gameObject);
        //else
        //    GameManager.Instance.priorityCarList.Remove(gameObject);

        //Destroy(this.gameObject);
        if (ClicLevelManager.Instance != null)
            ClicLevelManager.Instance.CarClicked(gameObject);

    }

    public void MoveToDestinations()
    {
        agent.isStopped = false;
        move = true;

        // Baja prioridad para colisionar fácilmente, alta prioridad para evitar colisiones
        if (destinations.Count > 0)
        {
            agent.SetDestination(destinations[currentTargetIndex]);
        }
    }
}
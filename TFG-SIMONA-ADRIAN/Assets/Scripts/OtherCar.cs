using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OtherCar : MonoBehaviour
{
    private List<Vector3> destinations;
  //  public List<Transform> targets; // Lista de destinos
    private int currentTargetIndex = 0;
    private NavMeshAgent agent;
    public float arrivalThreshold = 1.5f; // Umbral de distancia para considerar que ha llegado
    public bool hasReachedFirstDestination = false;
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
        if (destinations.Count == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= arrivalThreshold && agent.velocity.sqrMagnitude == 0f && !agent.isStopped)
        {
            OnArrival();
        }
    }

    void OnArrival()
    {
        Debug.Log("El coche ha llegado a su destino: " + currentTargetIndex);
        currentTargetIndex++;

        if (currentTargetIndex < destinations.Count)
        {
            hasReachedFirstDestination = true;

            //agent.destination = targets[currentTargetIndex].position; // Establecer el siguiente destino
            agent.SetDestination(destinations[currentTargetIndex]);
        }
        else if (!agent.isStopped)
        {
            Debug.Log("El coche ha llegado a todos sus destinos.");
            DetenerMovimiento(); // Detener el movimiento del coche
        }
    }

    void DetenerMovimiento()
    {
        agent.isStopped = true;
        agent.destination = agent.transform.position; // Establecer el destino a su posición actual
    }
    bool HasArrived()
    {
        return agent.isStopped;
    }
    public void SetDestinations(List<Vector3> newDestinations)
    {
        destinations = newDestinations; 
        //if (destinations != null && destinations.Count > 0)
        //{
        //    agent.destination = destinations[currentTargetIndex]; 
        //}
    }
}

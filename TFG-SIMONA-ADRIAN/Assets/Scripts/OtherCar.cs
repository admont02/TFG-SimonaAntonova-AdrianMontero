using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OtherCar : MonoBehaviour
{
    private List<Vector3> destinations = new List<Vector3>();
    //  public List<Transform> targets; // Lista de destinos
    private int currentTargetIndex = 0;
    private NavMeshAgent agent;
    public float arrivalThreshold = 1.5f; // Umbral de distancia para considerar que ha llegado
    public bool hasReachedFirstDestination = false;
    public Material outline;
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

        if (destinations.Count == 0 || !GameManager.Instance.canCarMove)
        {
            agent.isStopped = true;
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
}
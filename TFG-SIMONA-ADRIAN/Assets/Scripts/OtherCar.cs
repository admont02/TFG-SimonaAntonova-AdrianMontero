using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OtherCar : MonoBehaviour
{
    public Transform target; // El objetivo al que el coche se dirigirá
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = target.position; // Establecer el destino del coche
    }

    void Update()
    {
        
    }
}


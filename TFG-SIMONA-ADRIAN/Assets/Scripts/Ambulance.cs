using UnityEngine;
using UnityEngine.AI;

public class Ambulance : OtherCar
{
    public GameObject sirenLights;
    public AudioSource sirenSound;
    public bool lightsOn = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (destinations.Count > 0)
        {
            agent.destination = destinations[currentTargetIndex]; // Establecer el primer destino del coche
        }

        if (lightsOn)
        {
            sirenLights.SetActive(true);
            sirenSound.Play();
        }
        else
        {
            sirenLights.SetActive(false);
            sirenSound.Stop();
        }
    }

   

    public void ToggleSiren(bool state)
    {
        lightsOn = state;
        sirenLights.SetActive(state);
        if (state)
        {
            sirenSound.Play();
        }
        else
        {
            sirenSound.Stop();
        }
    }
}

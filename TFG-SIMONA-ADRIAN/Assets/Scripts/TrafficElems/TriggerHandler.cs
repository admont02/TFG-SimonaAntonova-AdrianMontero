using UnityEngine;
//using Xasu.HighLevel;

public class TriggerHandler : MonoBehaviour
{
    public GameObject trafficLight; // El semáforo que tiene los materiales

    private void OnTriggerEnter(Collider other)
    {
        OtherCar otherCarScript = other.GetComponentInParent<OtherCar>();

        if (other.gameObject.layer == 3)
        {

            if (gameObject.name == ("TriggerPlayer"))
            {

                if (trafficLight.GetComponent<SimpleTrafficLight>().red.activeSelf)
                {
                    //GameObjectTracker.Instance.Interacted("traffic-light-error", GameObjectTracker.TrackedGameObject.GameObject);
                    if(!GameManager.Instance.incorrectLevel.Contains("Has pasado un Semáforo con luz roja."))
                    GameManager.Instance.incorrectLevel.Add("Has pasado un Semáforo con luz roja.");
                }
            }
        }
        else
        {
            if (gameObject.name == ("TriggerIA") && trafficLight.GetComponent<SimpleTrafficLight>().red.activeSelf)
            {
                if (otherCarScript != null)
                {
                    otherCarScript.StopCar();
                    Debug.Log("Semáforo con luz roja: Coche detenido.");
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        OtherCar otherCarScript = other.GetComponentInParent<OtherCar>();

        if (otherCarScript != null && gameObject.name == "TriggerIA") // Coche IA
        {
            if (trafficLight.GetComponent<SimpleTrafficLight>().green.activeSelf)
            {
                otherCarScript.ResumeCar(); // Reanudamos el movimiento del coche cuando el semáforo esté verde
                Debug.Log("Semáforo con luz verde: Coche reanudado.");
            }
        }
    }
}

using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    public GameObject trafficLight; // El sem�foro que tiene los materiales

    private void OnTriggerEnter(Collider other)
    {
        OtherCar otherCarScript = other.GetComponentInParent<OtherCar>();

        if (other.gameObject.layer == 3)
        {

            if (gameObject.name == ("TriggerPlayer"))
            {

                if (trafficLight.GetComponent<SimpleTrafficLight>().red.activeSelf)
                {
                    GameManager.Instance.incorrectLevel.Add("Sem�foro con luz roja: Prohibido el paso.");
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
                    Debug.Log("Sem�foro con luz roja: Coche detenido.");
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
                otherCarScript.ResumeCar(); // Reanudamos el movimiento del coche cuando el sem�foro est� verde
                Debug.Log("Sem�foro con luz verde: Coche reanudado.");
            }
        }
    }
}

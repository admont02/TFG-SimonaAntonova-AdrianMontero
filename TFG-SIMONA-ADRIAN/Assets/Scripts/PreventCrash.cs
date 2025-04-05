using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventCrash : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 || other.gameObject.GetComponent<OtherCar>()) //Si detecta al jugador/otro coche
        {
            GetComponentInParent<OtherCar>().isCarInFront = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3 || other.gameObject.GetComponent<OtherCar>()) //Cuando el jugador/otro coche salga del trigger
        {
            GetComponentInParent<OtherCar>().isCarInFront = false;
        }
    }
}

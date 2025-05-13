using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventCrash : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 || other.gameObject.GetComponentInParent<OtherCar>() && other.gameObject.GetComponent<PreventCrash>()==null) //Si detecta al jugador/otro coche
        {
            Debug.Log("coche delante");
            GetComponentInParent<OtherCar>().isCarInFront = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3 || other.gameObject.GetComponentInParent<OtherCar>() && other.gameObject.GetComponent<PreventCrash>() == null) //Cuando el jugador/otro coche salga del trigger
        {
            GetComponentInParent<OtherCar>().isCarInFront = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponentInParent<OtherCar>())
        {
            if (other.gameObject.GetComponentInParent<OtherCar>().destroyedByTrash)
            {
                GetComponentInParent<OtherCar>().isCarInFront = false;
            }
        }
    }
}

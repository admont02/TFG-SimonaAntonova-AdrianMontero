using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deslumbramiento : MonoBehaviour
{
    bool correcto = false;


    /*
     
     if (allCorrect)
        {
            Debug.Log("correctas");
            //fog.SetActive(false);
            //fog.GetComponent<Renderer>().material = fogDisipada;
            // TO DO: cambiar a que se haga clear solo de las luces, no de todos los errores
            //GameManager.Instance.incorrectLevel.Clear();
            GameManager.Instance.incorrectLevel.Remove("Luces incorrectas para niebla intensa");
        }
        else
        {
            if (GameManager.Instance.incorrectLevel.Count < 2)
                GameManager.Instance.incorrectLevel.Add("Luces incorrectas para niebla intensa");
            //fog.GetComponent<Renderer>().material = fogIntensa;
        }
     
     */
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Debug.Log("COLLIDER STAY WITH " + other.gameObject.transform.parent.name);
            if (!other.gameObject.transform.parent.GetComponent<CarLights>().largasLights[0].enabled)
            {
                correcto = true;
                if (GameManager.Instance.incorrectLevel.Contains("Has deslumbrado a un usuario. Cambia de luces cortas a largas cuando se aproxime otro vehiculo."))
                    GameManager.Instance.incorrectLevel.Remove("Has deslumbrado a un usuario. Cambia de luces cortas a largas cuando se aproxime otro vehiculo.");
            }
            else
            {
                correcto = false;
                if (!GameManager.Instance.incorrectLevel.Contains("Has deslumbrado a un usuario. Cambia de luces cortas a largas cuando se aproxime otro vehiculo."))
                    GameManager.Instance.incorrectLevel.Add("Has deslumbrado a un usuario. Cambia de luces cortas a largas cuando se aproxime otro vehiculo.");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Debug.Log("COLLIDER exit WITH " + other.transform.parent.gameObject.name);
            if (other.gameObject.transform.parent.GetComponent<CarLights>().largasLights[0].enabled)
            {
                correcto = false;

                if (!GameManager.Instance.incorrectLevel.Contains("Has deslumbrado a un usuario. Cambia de luces cortas a largas cuando se aproxime otro vehiculo."))
                    GameManager.Instance.incorrectLevel.Add("Has deslumbrado a un usuario. Cambia de luces cortas a largas cuando se aproxime otro vehiculo.");

            }
        }
    }

}
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
            if (!other.gameObject.GetComponent<CarLights>().largasLights[0].enabled)
            {
                correcto = true;
                GameManager.Instance.incorrectLevel.Remove("Has deslumbrado a un usuario");
            }
            else
            {
                correcto = false;
                GameManager.Instance.incorrectLevel.Add("Has deslumbrado a un usuario");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            if (other.gameObject.GetComponent<CarLights>().largasLights[0].enabled)
            {
                correcto = false;

                if (!GameManager.Instance.incorrectLevel.Contains("Has deslumbrado a un usuario"))
                    GameManager.Instance.incorrectLevel.Add("Has deslumbrado a un usuario");

            }
        }
    }

}
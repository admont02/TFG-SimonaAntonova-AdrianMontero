using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Clase encargada de detectar deslumbramientos a otros vehiculos
/// </summary>
public class Deslumbramiento : MonoBehaviour
{
    bool correcto = false;


    
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
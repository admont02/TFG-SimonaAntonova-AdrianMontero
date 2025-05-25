using System.Collections;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Clase que representa una ambulancia
/// </summary>
public class Ambulance : MonoBehaviour
{
    public GameObject sirenLights;//luces
    public AudioSource sirenSound;//sonido
    public bool lightsOn = true;

    public Light redLight;
    public Light blueLight;
    public float blinkInterval = 0.5f;

    void Start()
    {
        
        if (lightsOn)
        {
            if (sirenLights != null)
                sirenLights.SetActive(true);
            sirenSound.Play();
            StartCoroutine(BlinkLights());
        }
        else
        {
            if (sirenLights != null)
                sirenLights.SetActive(false);
            sirenSound.Stop();
        }
    }
    private IEnumerator BlinkLights()
    {
        while (true)
        {
            redLight.enabled = !redLight.enabled;
            blueLight.enabled = !blueLight.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}

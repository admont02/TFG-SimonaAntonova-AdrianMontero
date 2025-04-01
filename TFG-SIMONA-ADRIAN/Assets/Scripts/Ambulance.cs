using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Ambulance : MonoBehaviour
{
    public GameObject sirenLights;
    public AudioSource sirenSound;
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

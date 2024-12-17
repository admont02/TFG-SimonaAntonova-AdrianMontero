using System;
using UnityEngine;
using UnityEngine.UI;
public class CarLights : MonoBehaviour
{
    public Light[] antinieblaLights;
    public Light[] posicionLights;
    public Light[] cortasLights;
    public Light[] largasLights;
    public Button antinieblaButton;
    public Button posicionButton;
    public Button cortasButton;
    public Button largasButton;

    void Start()
    {

        foreach (Light light in antinieblaLights)
        {
            light.color = Color.yellow;
            light.intensity = 1.5f;
            light.range = 50f;
        }

        foreach (Light light in posicionLights)
        {
            light.color = Color.white;
            light.intensity = 1f;
            light.range = 30f;
        }

        foreach (Light light in cortasLights)
        {
            light.color = Color.white;
            light.intensity = 2f;
            light.range = 100f;
            light.spotAngle = 45f; // Si es Spot Light
        }

        foreach (Light light in largasLights)
        {
            light.color = Color.white;
            light.intensity = 2.5f;
            light.range = 200f;
            light.spotAngle = 60f; // Si es Spot Light
        }
        antinieblaButton.onClick.AddListener(ToggleAntinieblaLights); 
        posicionButton.onClick.AddListener(TogglePosicionLights); 
        cortasButton.onClick.AddListener(ToggleCortasLights); 
        largasButton.onClick.AddListener(ToggleLargasLights);
    }

    public void ToggleAntinieblaLights()
    {
        foreach (Light light in antinieblaLights)
        {
            light.enabled = !light.enabled;
        }
        CheckCorrectLights();
    }

    public void TogglePosicionLights()
    {
        foreach (Light light in posicionLights)
        {
            light.enabled = !light.enabled;
        }
        CheckCorrectLights();
    }

    public void ToggleCortasLights()
    {
        foreach (Light light in cortasLights)
        {
            light.enabled = !light.enabled;
        }
        CheckCorrectLights();
    }

    public void ToggleLargasLights()
    {
        foreach (Light light in largasLights)
        {
            light.enabled = !light.enabled;
        }
        CheckCorrectLights();
    }

    private void CheckCorrectLights()
    {
        bool antinieblaOn = Array.Exists(antinieblaLights, light => light.enabled);
        bool cortasOn = Array.Exists(cortasLights, light => light.enabled);

        if (antinieblaOn && cortasOn)
        {
            RenderSettings.fogDensity = 0.01f; // Reducir la densidad de la niebla
        }
        else
        {
            RenderSettings.fogDensity = 0.05f; // Aumentar la densidad de la niebla
        }
    }
}

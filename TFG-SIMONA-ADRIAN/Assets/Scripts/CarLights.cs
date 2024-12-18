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
        if (antinieblaLights != null)
            foreach (Light light in antinieblaLights)
            {
                light.color = Color.yellow;
                light.intensity = 1.5f;
                light.range = 50f;
            }

        if (posicionLights != null)
            foreach (Light light in posicionLights)
            {
                light.color = Color.white;
                light.intensity = 1f;
                light.range = 30f;
            }

        if (cortasLights != null)
            foreach (Light light in cortasLights)
            {
                light.color = Color.white;
                light.intensity = 2f;
                light.range = 100f;
                light.spotAngle = 45f; // Si es Spot Light
            }

        if (largasLights != null)
            foreach (Light light in largasLights)
            {
                light.color = Color.white;
                light.intensity = 2.5f;
                light.range = 200f;
                light.spotAngle = 60f; // Si es Spot Light
            }
        if (antinieblaButton != null)
            antinieblaButton.onClick.AddListener(ToggleAntinieblaLights);
        if (posicionButton != null)
            posicionButton.onClick.AddListener(TogglePosicionLights);
        if (cortasButton != null)
            cortasButton.onClick.AddListener(ToggleCortasLights);
        if (largasButton != null)
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

using System;
using System.Collections.Generic;
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
    public List<string> objetivoLuces; // Lista de tipos de luces que deben estar encendidas

    private bool antinieblaOn = false;
    private bool posicionOn = false;
    private bool cortasOn = false;
    private bool largasOn = false;

    void Start()
    {
        // Configurar luces
        // ...

        if (antinieblaButton != null)
            antinieblaButton.onClick.AddListener(ToggleAntinieblaLights);
        if (posicionButton != null)
            posicionButton.onClick.AddListener(TogglePosicionLights);
        if (cortasButton != null)
            cortasButton.onClick.AddListener(ToggleCortasLights);
        if (largasButton != null)
            largasButton.onClick.AddListener(ToggleLargasLights);

        // CheckCorrectLights(); // Comprobar las luces iniciales
    }

    public void ToggleAntinieblaLights()
    {
        antinieblaOn = !antinieblaOn;
        foreach (Light light in antinieblaLights)
        {
            light.enabled = antinieblaOn;
        }
        CheckCorrectLights();
    }

    public void TogglePosicionLights()
    {
        posicionOn = !posicionOn;
        foreach (Light light in posicionLights)
        {
            light.enabled = posicionOn;
        }
        CheckCorrectLights();
    }

    public void ToggleCortasLights()
    {
        cortasOn = !cortasOn;
        foreach (Light light in cortasLights)
        {
            light.enabled = cortasOn;
        }
        CheckCorrectLights();
    }

    public void ToggleLargasLights()
    {
        largasOn = !largasOn;
        foreach (Light light in largasLights)
        {
            light.enabled = largasOn;
        }
        CheckCorrectLights();
    }

    private void CheckCorrectLights()
    {
        bool allCorrect = true;

        if (objetivoLuces.Contains("antinieblas") && !antinieblaOn
            || objetivoLuces.Contains("cortas") && !cortasOn
            || objetivoLuces.Contains("posicion") && !posicionOn
            || objetivoLuces.Contains("largas") && !largasOn)
        {
            allCorrect = false;
        }
        //luces encendidas de mas
        if (!objetivoLuces.Contains("antinieblas") && antinieblaOn
           || !objetivoLuces.Contains("cortas") && cortasOn
           || !objetivoLuces.Contains("posicion") && posicionOn
           || !objetivoLuces.Contains("largas") && largasOn)
        {
            allCorrect = false;
        }

        if (allCorrect)
        {
            Debug.Log("correctas");
            RenderSettings.fogDensity = 0.01f; // Reducir la densidad de la niebla
        }
        else
        {
            RenderSettings.fogDensity = 0.05f; // Aumentar la densidad de la niebla
        }
    }
}

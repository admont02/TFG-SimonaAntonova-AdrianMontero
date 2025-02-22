using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarLights : MonoBehaviour
{
    public GameObject fog;
    public Material fogDisipada;
    private Material fogIntensa;
    public Light[] antinieblaLights;
    public Light[] antinieblaBack;
    public Light[] posicionLights;
    public Light[] cortasLights;
    public Light[] largasLights;
    public Button antinieblaButton;
    public Button antinieblaBackButton;
    public Button posicionButton;
    public Button cortasButton;
    public Button largasButton;
    public List<string> objetivoLuces; // Lista de tipos de luces que deben estar encendidas

    private List<string> lucesSeleccionadas = new(); // Lista de tipos de luces que deben estar encendidas

    private bool antinieblaOn = false;
    private bool antinieblaBackOn = false;
    private bool posicionOn = false;
    private bool cortasOn = false;
    private bool largasOn = false;

    void Start()
    {
        // Configurar luces
        // ...
        fogIntensa = fog.GetComponent<Renderer>().material;

        if (antinieblaButton != null)
            antinieblaButton.onClick.AddListener(ToggleAntinieblaLights);
        if (antinieblaBackButton != null)
            antinieblaBackButton.onClick.AddListener(ToggleAntinieblaBackLights);
        if (posicionButton != null)
            posicionButton.onClick.AddListener(TogglePosicionLights);
        if (cortasButton != null)
            cortasButton.onClick.AddListener(ToggleCortasLights);
        if (largasButton != null)
            largasButton.onClick.AddListener(ToggleLargasLights);

        CheckCorrectLights(); // Comprobar las luces iniciales
    }

    public void ToggleAntinieblaLights()
    {
        antinieblaOn = !antinieblaOn;
        foreach (Light light in antinieblaLights)
        {
            light.enabled = antinieblaOn;
        }
        if (antinieblaOn)
            lucesSeleccionadas.Add("antinieblasDelanteras");
        else
            lucesSeleccionadas.Remove("antinieblasDelanteras");
        CheckCorrectLights();
    }
    public void ToggleAntinieblaBackLights()
    {
        antinieblaBackOn = !antinieblaBackOn;
        foreach (Light light in antinieblaBack)
        {
            light.enabled = antinieblaBackOn;
        }
        if (antinieblaBackOn)
            lucesSeleccionadas.Add("antinieblasTraseras");
        else
            lucesSeleccionadas.Remove("antinieblasTraseras");
        CheckCorrectLights();
    }

    public void TogglePosicionLights()
    {
        posicionOn = !posicionOn;
        foreach (Light light in posicionLights)
        {
            light.enabled = posicionOn;
        }
        if (posicionOn)
            lucesSeleccionadas.Add("posicion");
        else
            lucesSeleccionadas.Remove("posicion");
        CheckCorrectLights();
    }

    public void ToggleCortasLights()
    {
        cortasOn = !cortasOn;
        foreach (Light light in cortasLights)
        {
            light.enabled = cortasOn;
        }
        if (cortasOn)
            lucesSeleccionadas.Add("cortas");
        else
            lucesSeleccionadas.Remove("cortas");
        CheckCorrectLights();
    }

    public void ToggleLargasLights()
    {
        largasOn = !largasOn;
        foreach (Light light in largasLights)
        {
            light.enabled = largasOn;
        }
        if (largasOn)
            lucesSeleccionadas.Add("largas");
        else
            lucesSeleccionadas.Remove("largas");
        CheckCorrectLights();
    }

    private void CheckCorrectLights()
    {
        bool allCorrect = true;
        if (objetivoLuces.Count == lucesSeleccionadas.Count)
        {
            foreach (var item in objetivoLuces)
            {
                if (!lucesSeleccionadas.Contains(item))
                {
                    allCorrect = false;
                    break;
                }
            }
        }
        else
            allCorrect = false;

        if (allCorrect)
        {
            Debug.Log("correctas");
            //fog.SetActive(false);
            fog.GetComponent<Renderer>().material = fogDisipada;
            // TO DO: cambiar a que se haga clear solo de las luces, no de todos los errores
            //GameManager.Instance.incorrectLevel.Clear();
        }
        else
        {
            if (GameManager.Instance.incorrectLevel.Count < 2)
                GameManager.Instance.incorrectLevel.Add("Luces incorrectas para niebla intensa");
            fog.GetComponent<Renderer>().material = fogIntensa;
        }
    }
}

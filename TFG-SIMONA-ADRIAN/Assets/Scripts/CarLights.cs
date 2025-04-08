using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xasu.HighLevel;

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
    public Button comenzarButton;
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
        antinieblaButton = GameManager.Instance.antinieblaDelanteras.GetComponent<Button>();
        antinieblaBackButton = GameManager.Instance.antinieblaTraseras.GetComponent<Button>();
        posicionButton = GameManager.Instance.posicion.GetComponent<Button>();
        cortasButton = GameManager.Instance.cortas.GetComponent<Button>();
        largasButton = GameManager.Instance.largas.GetComponent<Button>();
        comenzarButton = GameManager.Instance.comenzar.GetComponent<Button>();
        if (posicionButton != null)
            posicionButton.onClick.AddListener(TogglePosicionLights);
        if (cortasButton != null)
            cortasButton.onClick.AddListener(ToggleCortasLights);
        if (largasButton != null)
            largasButton.onClick.AddListener(ToggleLargasLights);
        if (antinieblaButton != null)
            antinieblaButton.onClick.AddListener(ToggleAntinieblaLights);
        if (antinieblaBackButton != null)
            antinieblaBackButton.onClick.AddListener(ToggleAntinieblaBackLights);
        if (comenzarButton != null)
            comenzarButton.onClick.AddListener(ActivateLevel);


        CheckCorrectLights(); // Comprobar las luces iniciales
    }
    void ActivateLevel()
    {
        GameManager.Instance.canCarMove = true;
        comenzarButton.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        comenzarButton.gameObject.SetActive(false);

        if (CheckCorrectLights())
        {
            //fog.SetActive(false);
            fog.GetComponent<Renderer>().material = fogDisipada;
        }
        else
        {
            fog.GetComponent<Renderer>().material = fogIntensa;
        }
    }
    private void Update()
    {
        if (GameManager.Instance.dialogueSystem.dialoguePanel.activeSelf) return;
        // Posición
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            ToggleCortasLights();
        }
        // 
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            ToggleLargasLights();
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            TogglePosicionLights();
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            ToggleAntinieblaLights();
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            ToggleAntinieblaBackLights();
        }

        // Si acaba el nivel
        if (GameManager.Instance.finDeNivel)
        {
            comenzarButton.transform.parent.gameObject.SetActive(false);
            

        }
    }

    public void ToggleAntinieblaLights()
    {
        antinieblaOn = !antinieblaOn;
        foreach (Light light in antinieblaLights)
        {
            light.enabled = antinieblaOn;
        }
        GameManager.Instance.antinieblaDelanteras.GetComponent<Image>().color = antinieblaOn ? Color.yellow : Color.white;
        if (antinieblaOn)
        {
            GameObjectTracker.Instance.Interacted("antinieblasDelanteras-on");
            lucesSeleccionadas.Add("antinieblasDelanteras");
        }
        else
        {
            GameObjectTracker.Instance.Interacted("antinieblasDelanteras-off");
            lucesSeleccionadas.Remove("antinieblasDelanteras");
        }
        CheckCorrectLights();
    }
    public void ToggleAntinieblaBackLights()
    {
        antinieblaBackOn = !antinieblaBackOn;
        foreach (Light light in antinieblaBack)
        {
            light.enabled = antinieblaBackOn;
        }
        GameManager.Instance.antinieblaTraseras.GetComponent<Image>().color = antinieblaBackOn ? Color.yellow : Color.white;
        if (antinieblaBackOn)
        {
            GameObjectTracker.Instance.Interacted("antinieblasTraseras-on");
            lucesSeleccionadas.Add("antinieblasTraseras");
        }
        else
        {
            GameObjectTracker.Instance.Interacted("antinieblasTraseras-off");
            lucesSeleccionadas.Remove("antinieblasTraseras");
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
        GameManager.Instance.posicion.GetComponent<Image>().color = posicionOn ? Color.yellow : Color.white;
        if (posicionOn)
        {


            GameObjectTracker.Instance.Interacted("posicion-on");
            lucesSeleccionadas.Add("posicion");
        }
        else
        {
            if (cortasOn)
                ToggleCortasLights();
            if (largasOn)
                ToggleLargasLights();

            GameObjectTracker.Instance.Interacted("posicion-off");
            lucesSeleccionadas.Remove("posicion");
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
        GameManager.Instance.cortas.GetComponent<Image>().color = cortasOn ? Color.yellow : Color.white;
        if (cortasOn)
        {
            if (!posicionOn)
                TogglePosicionLights();
            if (largasOn)
                ToggleLargasLights();
            GameObjectTracker.Instance.Interacted("cortas-on");
            lucesSeleccionadas.Add("cortas");
        }
        else
        {
            GameObjectTracker.Instance.Interacted("cortas-off");
            lucesSeleccionadas.Remove("cortas");
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
        GameManager.Instance.largas.GetComponent<Image>().color = largasOn ? Color.yellow : Color.white;
        if (largasOn)
        {
            if (!posicionOn)
                TogglePosicionLights();
            if (cortasOn)
                ToggleCortasLights();

            GameObjectTracker.Instance.Interacted("largas-on");
            lucesSeleccionadas.Add("largas");
        }
        else
        {
            GameObjectTracker.Instance.Interacted("largas-off");
            lucesSeleccionadas.Remove("largas");
        }
        CheckCorrectLights();
    }

    private bool CheckCorrectLights()
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
            //fog.GetComponent<Renderer>().material = fogDisipada;
            // TO DO: cambiar a que se haga clear solo de las luces, no de todos los errores
            //GameManager.Instance.incorrectLevel.Clear();
            GameManager.Instance.incorrectLevel.Remove("Luces incorrectas. Recuerda que de noche en una via interurbana insuficientemente iluminada debes llevar encendidas las luces largas y de posición. Si cambias las luces para no deslumbrar, recuerda cambiar de nuevo las luces antes de entrar a la meta del nivel.");
        }
        else
        {
            if (GameManager.Instance.incorrectLevel.Count < 2)
                GameManager.Instance.incorrectLevel.Add("Luces incorrectas. Recuerda que de noche en una via interurbana insuficientemente iluminada debes llevar encendidas las luces largas y de posición. Si cambias las luces para no deslumbrar, recuerda cambiar de nuevo las luces antes de entrar a la meta del nivel.");
            //fog.GetComponent<Renderer>().material = fogIntensa;
        }
        return allCorrect;
    }
}

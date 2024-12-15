using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public DialogueSystem dialogueSystem;
    public bool canCarMove = false; // Booleano para controlar el movimiento del coche

    private LevelLoader nivelLoader;
    public CarController carController;
    public OtherCar otherCar;
    private GameObject playerTarget;
    private bool completed = false;
    private List<GameObject> cochesIA = new List<GameObject>();

    public List<string> incorrectLevel;

    void Awake()
    {
        // Asegurarse de que solo hay una instancia de GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener el GameManager entre escenas

            // Inicializar cualquier cosa necesaria al inicio de la escena
            InitializeNivelLoader();
            InitializeDialogue();

        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeDialogue()
    {
        if (dialogueSystem != null)
        {
            dialogueSystem.StartDialogue();
        }
    }
    private void Update()
    {
        //if (!completed)
        //    ComprobarNivel();
    }
    void InitializeNivelLoader()
    {
        nivelLoader = FindObjectOfType<LevelLoader>();
        if (nivelLoader != null)
        {
            nivelLoader.CargarNivel();
        }
        else
        {
            //Debug.LogError("No se encontró NivelLoader en la escena.");
        }
    }
    public void ComprobarNivel()
    {
        if (incorrectLevel.Count > 0)
        {
            Debug.Log("¡Nivel incorrecto!");
        }
        else 
        {
            dialogueSystem.ShowCompletedDialog();
            Debug.Log("¡Nivel completado correctamente!");
        }
    }

    internal void SetPlayerTarget(GameObject targetPoint)
    {
        playerTarget = targetPoint;
    }
    public void AddCocheIA(GameObject coche)
    {
        cochesIA.Add(coche);
    }
}

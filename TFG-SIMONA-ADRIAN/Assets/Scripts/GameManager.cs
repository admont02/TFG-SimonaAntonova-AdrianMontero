using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public DialogueSystem dialogueSystem;
    public bool canCarMove = false; // Booleano para controlar el movimiento del coche

    private LevelLoader nivelLoader;
    public CarController carController;
    public OtherCar otherCar;

    void Awake()
    {
        // Asegurarse de que solo hay una instancia de GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener el GameManager entre escenas

            // Inicializar cualquier cosa necesaria al inicio de la escena
            InitializeDialogue();
            InitializeNivelLoader();
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
            //dialogueSystem.StartDialogue();
        }
    }
    private void Update()
    {
        ComprobarNivel();
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
        if (carController.HasMoved() && !otherCar.hasReachedFirstDestination)
        {
            Debug.Log("¡Nivel incorrecto!");
        }
        else if (!carController.HasMoved() && otherCar.hasReachedFirstDestination)
        {
            {
                Debug.Log("¡Nivel completado correctamente!");
            }
        }
        Debug.Log("entro");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public DialogueSystem dialogueSystem;
    public bool canCarMove = false; // Booleano para controlar el movimiento del coche

    private LevelLoader nivelLoader;

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

    void InitializeNivelLoader()
    {
        nivelLoader = FindObjectOfType<LevelLoader>();
        if (nivelLoader != null)
        {
            nivelLoader.CargarNivel();
        }
        else
        {
            Debug.LogError("No se encontró NivelLoader en la escena.");
        }
    }
}

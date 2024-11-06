using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public DialogueSystem dialogueSystem;
    public bool canCarMove = false; // Booleano para controlar el movimiento del coche

    void Awake()
    {
        // Asegurarse de que solo hay una instancia de GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener el GameManager entre escenas
        }
        else
        {
            Destroy(gameObject);
        }

        // Inicializar cualquier cosa necesaria al inicio de la escena
        InitializeDialogue();
    }

    void InitializeDialogue()
    {
        if (dialogueSystem != null)
        {
            dialogueSystem.StartDialogue();
        }
    }
}

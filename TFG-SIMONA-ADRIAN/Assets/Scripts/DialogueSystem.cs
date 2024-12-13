using UnityEngine;
using TMPro;
using UnityEngine.UI; // Necesario para usar UI
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    public float typingSpeed = 0.05f; // Velocidad a la que se escribe el texto
    public AudioClip typingSound; // Sonido de tecleo
    private AudioSource audioSource;
    private string[] dialogues; // Array para almacenar los di�logos
    private int index;
    private bool isTyping = false;
    public Button dialogueBackground; // Fondo del di�logo que detectar� clics

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        //dialogues = new string[] {
        //    "�Bienvenido a la autoecuela!",
        //    "Este es un sistema de di�logo de prueba."
        //};

        // Asegurarnos de que la primera frase est� vac�a al inicio
        dialogueText.text = "";

        // Asignar el m�todo al evento de clic del bot�n
        dialogueBackground.onClick.AddListener(NextSentence);

    }

    public void StartDialogue()
    {
        index = 0;
        dialoguePanel.SetActive(true);
        GameManager.Instance.canCarMove = false;

        StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        isTyping = true;
        foreach (char letter in dialogues[index].ToCharArray())
        {
            dialogueText.text += letter;
            if (typingSound != null && !GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().PlayOneShot(typingSound);
            }
            yield return new WaitForSeconds(typingSpeed);
        }
        if (GetComponent<AudioSource>().isPlaying && typingSound != null)
        {
            GetComponent<AudioSource>().Stop();
        }
        isTyping = false;
    }

    public void NextSentence()
    {
        if (isTyping)
            return; // No avanzar si el texto se est� escribiendo

        if (index < dialogues.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(TypeSentence());
        }
        else
        {
            dialoguePanel.SetActive(false);
            GameManager.Instance.canCarMove = true;
        }
    }
    public void SetLevelDialog(string[] d)
    {
        dialogues = d;
    }
}

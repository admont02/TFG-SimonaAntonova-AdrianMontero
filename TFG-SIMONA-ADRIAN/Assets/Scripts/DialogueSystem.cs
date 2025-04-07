using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    public GameObject restartButton;
    public GameObject menuButton;
    public float typingSpeed = 0.05f; // Velocidad a la que se escribe el texto
    public AudioClip typingSound; // Sonido de tecleo
    public AudioClip winSound; // Sonido de win
    public AudioClip loseSound; // Sonido de lose



    private AudioSource audioSource;
    private string[] dialogues; // Array para almacenar los diálogos
    private string[] winDialogues; // Array para almacenar los diálogos de victoria
    private string[] wrongDialogues; // Array para almacenar los diálogos de incorrecto
    private int index;
    private bool isTyping = false;
    //private bool levelEnded = false;
    public Button dialogueBackground; // Fondo del diálogo que detectará clics
    public ParticleSystem confettiEffect;
    bool isEnd = false;


    void Awake()
    {

        dialogueText.text = ""; // Asegurarnos de que la primera frase esté vacía al inicio
        dialogueBackground.onClick.AddListener(NextSentence);
    }

    public void StartDialogue(bool end)
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        ResetDialogue();
        index = 0;
        dialoguePanel?.SetActive(true);
        if (end)
        {
            restartButton?.SetActive(true);
            menuButton?.SetActive(true);
        }
        GameManager.Instance.canCarMove = false;
        StartCoroutine(TypeSentence());
    }

    public void ShowCompletedDialog()
    {
        //levelEnded = true;
        dialogues = winDialogues;
        isEnd = true;

        if (confettiEffect != null)
        {
            Debug.Log("El sistema de partículas existe y está por reproducirse.");
            audioSource.PlayOneShot(winSound);
            confettiEffect.Play();
        }
        StartDialogue(true);
    }
    public void ShowIncorrectLevelDialog(string[] incorrect)
    {
        //levelEnded = true;
        if (ClicLevelManager.Instance == null)
            dialogues = incorrect;
        else
            dialogues = wrongDialogues;
        isEnd = true;
        audioSource.PlayOneShot(loseSound);
        //if (confettiEffect != null) { confettiEffect.Play(); }
        StartDialogue(true);
    }



    IEnumerator TypeSentence()
    {
        if (!dialoguePanel.activeSelf)
        {
            yield break; // Salimos de la coroutine
        }
        isTyping = true;
        foreach (char letter in dialogues[index].ToCharArray())
        {
            dialogueText.text += letter;
            if (typingSound != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(typingSound);
            }
            yield return new WaitForSeconds(typingSpeed);
        }
        if (audioSource.isPlaying && typingSound != null)
        {
            audioSource.Stop();
        }
        isTyping = false;
        typingSpeed = 0.05f;
        //if (levelEnded)
        //{
        //    GameManager.Instance.canCarMove = true;
        //    SceneManager.LoadScene("Menu");
        //}
    }

    public void NextSentence()
    {
        if (isTyping)
        {
            typingSpeed = 0.01f;
            //return; // No avanzar si el texto se está escribiendo
        }
        else
        {
            if (index < dialogues.Length - 1)
            {
                index++;
                dialogueText.text = "";
                StartCoroutine(TypeSentence());
            }
            else
            {
                if (!isEnd)
                {

                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    dialoguePanel.SetActive(false);
                    if (GameManager.Instance.carController != null)
                    {
                        if (GameManager.Instance.CurrentLevel == "Luces")
                            GameManager.Instance.LightsPanel.SetActive(true);
                        else
                        {
                            GameManager.Instance.canCarMove = true;
                        }
                    }
                    if (GameManager.Instance.CurrentLevel == "Prioridad")
                        GameManager.Instance.PerspectiveButton.SetActive(true);
                }
            }

        }
    }

    public void SetLevelDialog(string[] d, string[] cDialogs, string[] wDialogs)
    {
        dialogues = d;
        winDialogues = cDialogs;
        wrongDialogues = wDialogs;
    }

    public void ResetDialogue()
    {
        dialogueText.text = "";
        index = 0;
        StopAllCoroutines(); // Detener todas las corrutinas para asegurarnos de que no queden diálogos antiguos en ejecución
    }
}

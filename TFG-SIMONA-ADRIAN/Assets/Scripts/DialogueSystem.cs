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
    private AudioSource audioSource;
    private string[] dialogues; // Array para almacenar los diálogos
    private string[] winDialogues; // Array para almacenar los diálogos de victoria
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
        dialoguePanel.SetActive(true);
        if (end)
        {
            restartButton.SetActive(true);
            menuButton.SetActive(true);
        }
        GameManager.Instance.canCarMove = false;
        StartCoroutine(TypeSentence());
    }

    public void ShowCompletedDialog()
    {
        //levelEnded = true;
        dialogues = winDialogues;
        isEnd = true;

        if (confettiEffect != null) { confettiEffect.Play(); }
        StartDialogue(true);
    }
    public void ShowIncorrectLevelDialog(string[] incorrect)
    {
        //levelEnded = true;
        dialogues = incorrect;
        isEnd = true;
        //if (confettiEffect != null) { confettiEffect.Play(); }
        StartDialogue(true);
    }

    IEnumerator TypeSentence()
    {
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
        //if (levelEnded)
        //{
        //    GameManager.Instance.canCarMove = true;
        //    SceneManager.LoadScene("Menu");
        //}
    }

    public void NextSentence()
    {
        if (isTyping)
            return; // No avanzar si el texto se está escribiendo

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


                dialoguePanel.SetActive(false);
                if (GameManager.Instance.carController != null)
                    GameManager.Instance.canCarMove = true;
            }
        }
    }

    public void SetLevelDialog(string[] d, string[] cDialogs)
    {
        dialogues = d;
        winDialogues = cDialogs;
    }

    private void ResetDialogue()
    {
        dialogueText.text = "";
        index = 0;
        StopAllCoroutines(); // Detener todas las corrutinas para asegurarnos de que no queden diálogos antiguos en ejecución
    }
}

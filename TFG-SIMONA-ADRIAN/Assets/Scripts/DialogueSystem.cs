using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
/// <summary>
/// Clase encargada de gestionar todo lo relacionado con los dialogos
/// </summary>
public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    public GameObject restartButton;
    public GameObject menuButton;
    public float typingSpeed = 0.05f; //Velocidad a la que se escribe el texto
    public AudioClip typingSound; //Sonido de tecleo
    public AudioClip winSound; //Sonido de win
    public AudioClip loseSound; //Sonido de lose



    private AudioSource audioSource;
    private string[] dialogues; //Array para almacenar los diálogos
    private string[] winDialogues; //Array para almacenar los diálogos de victoria
    private string[] wrongDialogues; //Array para almacenar los diálogos de incorrecto
    private int index;
    private bool isTyping = false;
    //private bool levelEnded = false;
    public Button dialogueBackground; //Fondo del diálogo que detectará clics
    public ParticleSystem confettiEffect;
    bool isEnd = false;


    void Awake()
    {

        dialogueText.text = ""; //primera frase esté vacía al inicio
        dialogueBackground.onClick.AddListener(NextSentence);
    }
    /// <summary>
    /// Metodo que inicia la escritura de los dialogos
    /// </summary>
    /// <param name="end"></param>
    public void StartDialogue(bool end)
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.07f;

        ResetDialogue();
        index = 0;
        dialoguePanel?.SetActive(true);

        GameManager.Instance.canCarMove = false;
        StartCoroutine(TypeSentence());

    }
    /// <summary>
    /// Metodo que muestra los dialogos de nivel correcto
    /// </summary>
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
    /// <summary>
    /// Metodo que muestra los dialogos de nivel incorrecto
    /// </summary>
    /// <param name="incorrect"></param>
    public void ShowIncorrectLevelDialog(string[] incorrect)
    {
        //levelEnded = true;
        if (wrongDialogues == null)
            dialogues = incorrect;
        else
            dialogues = wrongDialogues;
        isEnd = true;
        audioSource.PlayOneShot(loseSound);
        //if (confettiEffect != null) { confettiEffect.Play(); }
        StartDialogue(true);
    }


    /// <summary>
    /// Metodo encargado de escribir las frases
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// Cambio de frase
    /// </summary>
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
                if (GameManager.Instance.finDeNivel)
                {
                    restartButton?.SetActive(true);
                    menuButton?.SetActive(true);
                }
            }

        }
    }
    /// <summary>
    /// Metodo que establece los dialogos del nivel
    /// </summary>
    /// <param name="d"></param>
    /// <param name="cDialogs"></param>
    /// <param name="wDialogs"></param>
    public void SetLevelDialog(string[] d, string[] cDialogs, string[] wDialogs)
    {
        dialogues = d;
        winDialogues = cDialogs;
        wrongDialogues = wDialogs;
    }
    /// <summary>
    /// Metodo que resetea los dialogos
    /// </summary>
    public void ResetDialogue()
    {
        dialogueText.text = "";
        index = 0;
        StopAllCoroutines(); // Detener todas las corrutinas para asegurarnos de que no queden diálogos antiguos en ejecución
    }
}

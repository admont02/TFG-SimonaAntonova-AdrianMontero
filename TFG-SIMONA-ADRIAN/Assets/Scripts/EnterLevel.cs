using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterLevel : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip sparkle;


    public Transform player;  // Referencia al transform del jugador
    public float proximityDistance = 5f;  // Distancia a la que la señal se agranda
    public Vector3 enlargedScale = new Vector3(4f, 4f, 4f);  // Tamaño agrandado
    private Vector3 originalScale;
    private bool near = false;
    public TextMeshProUGUI text;
    public GameObject panel;
    public Button botonJugar;
    public Button botonCerrar;
    public string levelString;
    public Texture2D handCursor;
    public ParticleSystem sparkEffect; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < proximityDistance)
        {
            if (!near)
            {
                near = true;
                audioSource.PlayOneShot(audioClip);
                transform.localScale = enlargedScale;
                sparkEffect.Play();
                //SceneManager.LoadScene(gameObject.name);
                //panel.SetActive(true);
                //SceneData.JsonFileName = gameObject.name + ".json";
                //SceneManager.LoadScene("Game");
                //botonJugar.onClick.AddListener(PlayLevel);
            }
            else //si estas cerca
            {
                if (!audioSource.isPlaying) // Asegúrate de que no se solape
                {
                    audioSource.clip = sparkle; // Asigna el sonido para reproducir en bucle
                    audioSource.volume = 0.05f;
                    audioSource.loop = true; // Activa el bucle
                    audioSource.Play();
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {

                        if (hit.collider.gameObject == gameObject)
                        {

                            panel.SetActive(true);
                            botonJugar.onClick.AddListener(PlayLevel);
                            botonCerrar.onClick.AddListener(CerrarPanel);
                            text.text = levelString;
                        }
                    }
                }
            }
        }
        else
        {
            if (near)
                near = false;
            sparkEffect.Stop();
            transform.localScale = originalScale;
            if (audioSource.isPlaying)
            {
                audioSource.Stop(); // Detiene el audio
                audioSource.loop = false; // Desactiva el bucle para futuros usos
            }
        }
    }

    void PlayLevel()
    {
        SceneData.lastCarPosition = player.transform.position;
        SceneData.hasLastPosition = true;
        SceneData.lastCarRotation = player.transform.rotation;
        SceneData.JsonFileName = gameObject.name + ".json";
        SceneManager.LoadScene("Game");
    }
    void CerrarPanel()
    {
        panel.SetActive(false);
        botonJugar.onClick.RemoveAllListeners();
        botonCerrar.onClick.RemoveAllListeners();
    }

    void OnMouseEnter()
    {
        if (near)
            Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto); // Cambiar el cursor
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Restaurar el cursor original
    }

}

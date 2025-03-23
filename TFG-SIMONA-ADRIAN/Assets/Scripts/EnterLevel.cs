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
                //SceneManager.LoadScene(gameObject.name);
                //panel.SetActive(true);
                //SceneData.JsonFileName = gameObject.name + ".json";
                //SceneManager.LoadScene("Game");
                //botonJugar.onClick.AddListener(PlayLevel);
            }
            else //si estas cerca
            {
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
            transform.localScale = originalScale;
        }
    }

    void PlayLevel()
    {
        SceneData.JsonFileName = gameObject.name + ".json";
        SceneManager.LoadScene("Game");
    }
    void CerrarPanel()
    {
        panel.SetActive(false);
        botonJugar.onClick.RemoveAllListeners();
        botonCerrar.onClick.RemoveAllListeners();
    }
}

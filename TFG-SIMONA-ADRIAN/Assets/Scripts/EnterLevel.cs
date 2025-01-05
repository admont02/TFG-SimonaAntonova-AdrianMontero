using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterLevel : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip audioClip;

    public Transform player;  // Referencia al transform del jugador
    public float proximityDistance = 5f;  // Distancia a la que la señal se agranda
    public Vector3 enlargedScale = new Vector3(4f, 4f, 4f);  // Tamaño agrandado
    private Vector3 originalScale;
    private bool near = false;
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
                // SceneManager.LoadScene(gameObject.name);
            }

        }
        else
        {
            if (near)
                near = false;
            transform.localScale = originalScale;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // si es player cambia al nivel del trigger
        //if (other.gameObject.transform == player)

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrafficLight : MonoBehaviour
{
    public GameObject red; // Array de materiales que quieres alternar
    public GameObject amber; // Array de materiales que quieres alternar
    public GameObject green; // Array de materiales que quieres alternar

    public float greenSeconds = 20.0f;
    public float amberSeconds = 2.0f;
    public float redSeconds = 15.0f;

    private void Start()
    {

        Vector3 position = gameObject.transform.position;
        position.y = 7.57f;
        gameObject.transform.position = position;


        // Obtén el MeshRenderer del GameObject
        StartCoroutine(ChangeLightColorCoroutine());
    }

    private IEnumerator ChangeLightColorCoroutine()
    {
        float currentSecs = 1.0f;
        bool fromRed = false;

        while (true)
        {
            // Estaba rojo
            if (red.activeSelf)
            {
                currentSecs = amberSeconds;
                // Encender ambar
                amber.SetActive(true);
                // Apagar rojo
                red.SetActive(false);
                fromRed = true;
            }
            // Estaba Ambar
            else if (amber.activeSelf)
            {
                // Apagar ambar
                amber.SetActive(false);
                if (fromRed)
                {
                    // Encender verde
                    currentSecs = greenSeconds;
                    green.SetActive(true);
                }
                else
                {
                    // Encender rojo
                    currentSecs = redSeconds;
                    red.SetActive(true);
                }

            }
            // Estaba verde
            else if (green.activeSelf)
            {
                currentSecs = amberSeconds;
                // Encender ambar
                amber.SetActive(true);
                // Apagar verde
                green.SetActive(false);
                fromRed = false;
            }

            // Espera 2 segundos antes de cambiar al siguiente material
            yield return new WaitForSeconds(currentSecs);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // si es player cambia al nivel del trigger
        if (red.activeSelf && other.gameObject.layer == 3)
            GameManager.Instance.incorrectLevel.Add("Semáforo con luz roja: Prohibido el paso.");

    }
}

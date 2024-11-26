using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrafficLight : MonoBehaviour
{
    public GameObject red; // Array de materiales que quieres alternar
    public GameObject amber; // Array de materiales que quieres alternar
    public GameObject green; // Array de materiales que quieres alternar

    private void Start()
    {
        // Obtén el MeshRenderer del GameObject
        StartCoroutine(ChangeMaterialCoroutine());
    }

    private IEnumerator ChangeMaterialCoroutine()
    {
        float currentSecs = 1.0f;
        bool fromRed = false;

        while (true)
        {
            // Estaba rojo
            if (red.activeSelf)
            {
                currentSecs = 1.0f;
                // Encender ambar
                amber.SetActive(true);
                // Apagar rojo
                red.SetActive(false);
                fromRed = true;
            }
            // Estaba Ambar
            else if (amber.activeSelf)
            {
                currentSecs = 5.0f;
                // Apagar ambar
                amber.SetActive(false);
                if (fromRed)
                {
                    // Encender verde
                    green.SetActive(true);
                }
                else
                {
                    // Encender rojo
                    red.SetActive(true);
                }

            }
            // Estaba verde
            else if (green.activeSelf)
            {
                currentSecs = 1.0f;
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
}

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SimpleTrafficLight : MonoBehaviour
{
    public GameObject red; // Array de materiales que quieres alternar
    public GameObject amber; // Array de materiales que quieres alternar
    public GameObject green; // Array de materiales que quieres alternar
    public GameObject indicator; // Array de materiales que quieres alternar

    public Material redMaterial; // Material para el rojo
    public Material amberMaterial; // Material para el amarillo
    public Material greenMaterial;

    public float greenSeconds = 20.0f;
    public float amberSeconds = 2.0f;
    public float redSeconds = 15.0f;

    private void Start()
    {

        Vector3 position = gameObject.transform.position;
        position.y = 7.57f;
        gameObject.transform.position = position;
        UpdateIndicatorMaterial();

        // Obtén el MeshRenderer del GameObject
        StartCoroutine(ChangeLightColorCoroutine());
    }
    private void UpdateIndicatorMaterial()
    {
        // Obtén el MeshRenderer del indicador y cambia su material según el color activo
        MeshRenderer indicatorRenderer = indicator.GetComponent<MeshRenderer>();
        if (red.activeSelf)
        {
            indicatorRenderer.material = redMaterial;
        }
        else if (amber.activeSelf)
        {
            indicatorRenderer.material = amberMaterial;
        }
        else if (green.activeSelf)
        {
            indicatorRenderer.material = greenMaterial;
        }
    }
    private IEnumerator ChangeLightColorCoroutine()
    {
        while (GameManager.Instance.dialogueSystem.dialoguePanel.activeSelf)
        {
            yield return null;
        }
        float currentSecs = 10.0f;
        bool fromRed = false;
        if (red.activeSelf)
        {
            currentSecs = redSeconds; //Mantener el rojo por redSeconds
        }
        else if (amber.activeSelf)
        {
            currentSecs = amberSeconds; //Mantener el ambar por amberSeconds
        }
        else if (green.activeSelf)
        {
            currentSecs = greenSeconds; //Mantener el verde por greenSeconds
        }

        // Pausar antes de iniciar el cambio de luces
        yield return new WaitForSeconds(currentSecs);
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

                // Encender verde
                currentSecs = greenSeconds;
                green.SetActive(true);



            }
            // Estaba verde
            else if (green.activeSelf)
            {
                currentSecs = redSeconds;
                // Encender ambar
                red.SetActive(true);
                // Apagar verde
                green.SetActive(false);

            }
            UpdateIndicatorMaterial();
            // Espera 2 segundos antes de cambiar al siguiente material
            yield return new WaitForSeconds(currentSecs);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // si es player cambia al nivel del trigger
        if (red.activeSelf && other.gameObject.layer == 3 && !GameManager.Instance.incorrectLevel.Contains("Has pasado un Semáforo con luz roja."))
            GameManager.Instance.incorrectLevel.Add("Has pasado un Semáforo con luz roja.");

    }
}

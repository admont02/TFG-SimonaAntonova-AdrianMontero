using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMenu : MonoBehaviour
{
    public float velocidad = 10f; // Velocidad de movimiento del coche
    public float rotacionVelocidad = 5f; // Velocidad de rotación del coche
    private Vector3 destino; // El punto donde el coche se mueve
    private bool moviendo = false; // ¿Está el coche en movimiento?
    public GameObject panelInfo;
    public GameObject clickEffectPrefab;

    GameObject actual;
    private void Awake()
    {
        actual = null;
    }
    void Update()
    {
        if (GameManager.Instance.dialogueSystem.dialoguePanel.activeSelf || panelInfo.activeSelf)
            return;
        // Detecta clic en el mapa
        if (Input.GetMouseButtonDown(0)) // Clic izquierdo del ratón
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Raycast desde la cámara hacia el punto de clic
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("MenuRoad"))
                {
                    destino = hit.point; // Guarda el punto de destino donde el coche debe ir
                    moviendo = true; // El coche empieza a moverse
                    Quaternion rot = Quaternion.Euler(90, 0, 0);
                    if (actual != null)
                        Destroy(actual);
                    actual = Instantiate(clickEffectPrefab, hit.point, rot);
                    AudioSource audioSource = actual.GetComponent<AudioSource>();
                    if (audioSource != null && !audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
            }
        }

        // Mueve el coche hacia el destino
        if (moviendo)
        {
            MoverCoche();
        }
    }

    // Función que mueve el coche hacia el destino
    void MoverCoche()
    {
        // Calcula la dirección hacia el destino
        Vector3 direccion = destino - transform.position;
        direccion.y = 0; // Asegura que el coche no se mueva en el eje Y (solo en el plano horizontal)

        // Si el coche está cerca del destino, lo detenemos
        if (direccion.magnitude < 1f)
        {
            moviendo = false;
            Destroy(actual);
            return;
        }

        // Mueve el coche en la dirección calculada
        transform.Translate(direccion.normalized * velocidad * Time.deltaTime, Space.World);

        // Rotación suave hacia la dirección del destino
        Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, rotacionVelocidad * Time.deltaTime);
    }
}



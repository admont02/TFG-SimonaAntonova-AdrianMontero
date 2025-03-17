using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2 : MonoBehaviour
{
    public Transform target; // El coche que la cámara seguirá
    public float distance = 10.0f; // Distancia desde la cámara al coche
    public float height = 5.0f; // Altura de la cámara respecto al coche
    public float heightDamping = 2.0f; // Suavizado de altura
    public float rotationDamping = 3.0f; // Suavizado de rotación

    void Awake()
    {
        if (target != null)
        {
            // Coloca la cámara en la posición inicial correcta
            SetInitialCameraPosition();
        }
    }

    void LateUpdate()
    {
        if (!target)
            return;

        // Actualiza la posición y la rotación de la cámara cada frame
        UpdateCameraPosition();
    }

    void SetInitialCameraPosition()
    {
        // Establece la rotación inicial de la cámara mirando al coche
        transform.rotation = Quaternion.Euler(0, target.eulerAngles.y, 0);

        // Establece la posición de la cámara detrás del coche a la altura y distancia adecuadas
        Vector3 initialPosition = target.position - transform.rotation * Vector3.forward * distance;
        initialPosition.y = target.position.y + height;

        transform.position = initialPosition;

        // Asegura que la cámara mire al coche
        transform.LookAt(target);
    }

    void UpdateCameraPosition()
    {
        // Calcula la rotación deseada
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Suaviza la rotación
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Suaviza la altura
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convierte el ángulo en una rotación
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Establece la posición de la cámara detrás del coche
        transform.position = target.position - currentRotation * Vector3.forward * distance;

        // Establece la altura de la cámara
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Siempre mira al coche
        transform.LookAt(target);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2 : MonoBehaviour
{
    public Transform target; // El coche que la c�mara seguir�
    public float distance = 10.0f; // Distancia desde la c�mara al coche
    public float height = 5.0f; // Altura de la c�mara respecto al coche
    public float heightDamping = 2.0f; // Suavizado de altura
    public float rotationDamping = 3.0f; // Suavizado de rotaci�n

    void Awake()
    {
        if (target != null)
        {
            // Coloca la c�mara en la posici�n inicial correcta
            SetInitialCameraPosition();
        }
    }

    void LateUpdate()
    {
        if (!target)
            return;

        // Actualiza la posici�n y la rotaci�n de la c�mara cada frame
        UpdateCameraPosition();
    }

    void SetInitialCameraPosition()
    {
        // Establece la rotaci�n inicial de la c�mara mirando al coche
        transform.rotation = Quaternion.Euler(0, target.eulerAngles.y, 0);

        // Establece la posici�n de la c�mara detr�s del coche a la altura y distancia adecuadas
        Vector3 initialPosition = target.position - transform.rotation * Vector3.forward * distance;
        initialPosition.y = target.position.y + height;

        transform.position = initialPosition;

        // Asegura que la c�mara mire al coche
        transform.LookAt(target);
    }

    void UpdateCameraPosition()
    {
        // Calcula la rotaci�n deseada
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Suaviza la rotaci�n
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Suaviza la altura
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convierte el �ngulo en una rotaci�n
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Establece la posici�n de la c�mara detr�s del coche
        transform.position = target.position - currentRotation * Vector3.forward * distance;

        // Establece la altura de la c�mara
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Siempre mira al coche
        transform.LookAt(target);
    }
}


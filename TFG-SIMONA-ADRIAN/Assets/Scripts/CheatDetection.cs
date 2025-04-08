using UnityEngine;

public class CheatDetection : MonoBehaviour
{
    // Define los l�mites permitidos de rotaci�n en el eje Y
    public float minRotation = -30f; // Extremo inferior (en grados)
    public float maxRotation = 30f;  // Extremo superior (en grados)

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer==3)
        {
            // Obtener la rotaci�n actual del coche en el eje Y
            float vehicleRotationY = other.transform.eulerAngles.y;
            Debug.Log(vehicleRotationY);

            // Comprobar si est� fuera del rango permitido
            if (IsOutsideAllowedRotation(vehicleRotationY))
            {
                Debug.Log("�El jugador est� en el carril contrario o mal orientado!");
                if (!GameManager.Instance.incorrectLevel.Contains("Has invadido el carril contrario."))
                    GameManager.Instance.incorrectLevel.Add("Has invadido el carril contrario.");
                // Acciones como penalizar o mostrar un aviso
            }
            
        }
    }

    private bool IsOutsideAllowedRotation(float rotationY)
    {
        // Ajustar la rotaci�n para que est� dentro del rango de -180 a 180 grados
        //rotationY = NormalizeAngle(rotationY);

        // Comprobar si est� dentro del rango permitido
        return rotationY <= minRotation && rotationY >= maxRotation;
    }

    private float NormalizeAngle(float angle)
    {
        // Convierte cualquier �ngulo en un rango de -180 a 180 grados
        if (angle > 180f)
            angle -= 360f;
        if (angle < -180f)
            angle += 360f;
        return angle;
    }
}

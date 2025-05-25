using UnityEngine;
/// <summary>
/// Clase utilizada para detectar trampas en vias continuas
/// </summary>
public class CheatDetection : MonoBehaviour
{
    
    public float minRotation = -30f; 
    public float maxRotation = 30f;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer==3)
        {
            //Obtener la rotación actual del coche en el eje Y
            float vehicleRotationY = other.transform.eulerAngles.y;
            Debug.Log(vehicleRotationY);

            //Comprobar si está fuera del rango permitido
            if (IsOutsideAllowedRotation(vehicleRotationY))
            {
                Debug.Log("¡El jugador está en el carril contrario o mal orientado!");
                if (!GameManager.Instance.incorrectLevel.Contains("Has invadido el carril contrario."))
                    GameManager.Instance.incorrectLevel.Add("Has invadido el carril contrario.");
               
            }
            
        }
    }

    private bool IsOutsideAllowedRotation(float rotationY)
    {
        // Comprobar si está dentro del rango permitido
        return rotationY <= minRotation && rotationY >= maxRotation;
    }

}

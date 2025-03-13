using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotondaTrigger : MonoBehaviour
{
    private const float leftTurnThreshold = -30f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {


            float entryAngle = NormalizeAngle(other.transform.eulerAngles.y);

            // Verificar si el jugador entra desde la izquierda
            if (entryAngle < leftTurnThreshold || entryAngle > 180)
            {
                Debug.LogWarning("Entrada incorrecta en la rotonda.");
                // Añadir lógica para manejar la entrada incorrecta (por ejemplo, reducir la velocidad)
                GameManager.Instance.incorrectLevel.Add("Entrada incorrecta en la rotonda: Prohibido el paso.");
            }
        }
    }

    private float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Xasu.HighLevel;
/// <summary>
///  Clase para detectar si se realiza una glorieta en sentido contrario
/// </summary>
public class RotondaTrigger : MonoBehaviour
{
    private const float leftTurnThreshold = -20f;
    [SerializeField]
    public float lowerLimit;
    [SerializeField]
    public float greaterLimit;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {


            //float entryAngle = NormalizeAngle(other.transform.eulerAngles.y);
            float entryAngle = NormalizeAngle(other.transform.eulerAngles.y);

            // Verificar si el jugador entra desde la izquierda
            if (entryAngle < lowerLimit && entryAngle > greaterLimit)
            {
                //GameObjectTracker.Instance.Interacted("roundabout-error", GameObjectTracker.TrackedGameObject.GameObject);
                Debug.LogWarning("Entrada incorrecta en la rotonda.");
                // Añadir lógica para manejar la entrada incorrecta (por ejemplo, reducir la velocidad)
                if (!GameManager.Instance.incorrectLevel.Contains("Rotonda realizada en sentido contrario."))
                    GameManager.Instance.incorrectLevel.Add("Rotonda realizada en sentido contrario.");
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

using UnityEngine;
/// <summary>
///  Clase para detectar si se incumple una se�al de ceda el paso
/// </summary>
public class YieldSign : MonoBehaviour
{
    public Collider yieldTrigger; // Trigger para detectar si llegas al ceda
    public Collider passingTrigger; // Trigger para detectar si alguien est� pasando

    private OtherCar currentIAAtYield; // Referencia al cocheIA en el yield
    private bool isSomeonePassing = false; // Indica si hay alguien pasando

    private void OnTriggerEnter(Collider other)
    {
        // Obtener referencia al componente OtherCar en el padre del objeto que entra al trigger
        OtherCar car = other.transform.GetComponentInParent<OtherCar>();

        // Detectar si un cocheIA llega al yield
        if (yieldTrigger.bounds.Contains(other.transform.position) && car != null)
        {
            currentIAAtYield = car;
            Debug.Log("CocheIA ha llegado al yield.");

            // Si hay alguien pasando, detener el cocheIA
            if (isSomeonePassing && currentIAAtYield != null)
            {
                currentIAAtYield.StopCar();
                Debug.Log("CocheIA detenido en el yield porque alguien est� pasando.");
            }
        }

        // Detectar si alguien est� pasando (jugador o cocheIA)
        if (passingTrigger.bounds.Contains(other.transform.position) &&
            (other.gameObject.layer == 3 || car != null))
        {
            isSomeonePassing = true;
            Debug.Log("Alguien est� pasando por la intersecci�n.");

            // Si un cocheIA est� en el yield, detenerlo
            if (currentIAAtYield != null)
            {
                currentIAAtYield.StopCar();
                Debug.Log("CocheIA detenido porque alguien est� pasando.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Obtener referencia al componente OtherCar en el padre del objeto que sale del trigger
        OtherCar car = other.transform.GetComponentInParent<OtherCar>();

        // Detectar si el cocheIA sale del yield
        if (yieldTrigger.bounds.Contains(other.transform.position) && car != null && car == currentIAAtYield)
        {
            currentIAAtYield.ResumeCar(); // Reanudar el cocheIA al salir
            Debug.Log("CocheIA ha salido del yield y se reanuda.");
            currentIAAtYield = null;
        }

        // Detectar si alguien deja de pasar
        if (passingTrigger.bounds.Contains(other.transform.position) &&
            (other.gameObject.layer == 3 || car != null))
        {
            isSomeonePassing = false;
            Debug.Log("Nadie est� pasando por la intersecci�n.");

            // Si hay un cocheIA en el yield, reanudarlo
            if (currentIAAtYield != null)
            {
                currentIAAtYield.ResumeCar();
                Debug.Log("CocheIA reanudado porque nadie est� pasando.");
            }
        }

        // Detectar si el jugador sale del yield mientras hab�a alguien pasando
        if (yieldTrigger.bounds.Contains(other.transform.position) && other.gameObject.layer == 3)
        {
            if (isSomeonePassing)
            {
                Debug.Log("Jugador sali� del yield mientras hab�a alguien pasando. Error generado.");
                AddPlayerError();
            }
        }
    }

    private void AddPlayerError()
    {
        // Generar el error para el jugador
        if (!GameManager.Instance.incorrectLevel.Contains("No has respetado la se�al de Ceda el Paso. Recuerda que debes esperar hasta que la intersecci�n est� libre."))
        {
            GameManager.Instance.incorrectLevel.Add("No has respetado la se�al de Ceda el Paso. Recuerda que debes esperar hasta que la intersecci�n est� libre.");
        }
    }
}

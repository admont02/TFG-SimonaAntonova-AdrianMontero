using UnityEngine;
/// <summary>
///  Clase para detectar si se incumple una señal de ceda el paso
/// </summary>
public class YieldSign : MonoBehaviour
{
    public Collider yieldTrigger; // Trigger para detectar si llegas al ceda
    public Collider passingTrigger; // Trigger para detectar si alguien está pasando

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
                Debug.Log("CocheIA detenido en el yield porque alguien está pasando.");
            }
        }

        // Detectar si alguien está pasando (jugador o cocheIA)
        if (passingTrigger.bounds.Contains(other.transform.position) &&
            (other.gameObject.layer == 3 || car != null))
        {
            isSomeonePassing = true;
            Debug.Log("Alguien está pasando por la intersección.");

            // Si un cocheIA está en el yield, detenerlo
            if (currentIAAtYield != null)
            {
                currentIAAtYield.StopCar();
                Debug.Log("CocheIA detenido porque alguien está pasando.");
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
            Debug.Log("Nadie está pasando por la intersección.");

            // Si hay un cocheIA en el yield, reanudarlo
            if (currentIAAtYield != null)
            {
                currentIAAtYield.ResumeCar();
                Debug.Log("CocheIA reanudado porque nadie está pasando.");
            }
        }

        // Detectar si el jugador sale del yield mientras había alguien pasando
        if (yieldTrigger.bounds.Contains(other.transform.position) && other.gameObject.layer == 3)
        {
            if (isSomeonePassing)
            {
                Debug.Log("Jugador salió del yield mientras había alguien pasando. Error generado.");
                AddPlayerError();
            }
        }
    }

    private void AddPlayerError()
    {
        // Generar el error para el jugador
        if (!GameManager.Instance.incorrectLevel.Contains("No has respetado la señal de Ceda el Paso. Recuerda que debes esperar hasta que la intersección esté libre."))
        {
            GameManager.Instance.incorrectLevel.Add("No has respetado la señal de Ceda el Paso. Recuerda que debes esperar hasta que la intersección esté libre.");
        }
    }
}

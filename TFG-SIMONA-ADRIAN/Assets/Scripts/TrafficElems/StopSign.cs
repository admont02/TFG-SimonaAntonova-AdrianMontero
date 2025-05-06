using System.Collections;
using UnityEngine;
//using Xasu.HighLevel;

public class StopSign : MonoBehaviour
{
    public float stopDuration = 3.0f; //Duración del tiempo que el coche debe detenerse
    private float timeStopped = 0.0f; //Tiempo que el jugador ha permanecido detenido
    private bool isInsideTrigger = false; //Si el jugador está dentro del trigger
    bool IAstopped = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            isInsideTrigger = true;
            timeStopped = 0.0f;
            StartCoroutine(StopPlayer());

        }
        else if (other.GetComponentInParent<OtherCar>())
        {
            OtherCar car = other.GetComponentInParent<OtherCar>();
            if (car != null && !IAstopped)
            {
                StartCoroutine(StopCarCoroutine(car));
            }
        }
    }
    private IEnumerator StopCarCoroutine(OtherCar car)
    {
        car.StopCar();
        yield return new WaitForSeconds(stopDuration);
        car.ResumeCar();
        IAstopped = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            isInsideTrigger = false;
            CheckPlayerLeaving();

        }
        else if (other.GetComponentInParent<OtherCar>())
        {
            IAstopped = false;
        }
    }

    private void CheckPlayerLeaving()
    {
        if (timeStopped < stopDuration)
        {
            //GameObjectTracker.Instance.Interacted("stop-sign-error", GameObjectTracker.TrackedGameObject.GameObject);
            Debug.Log("El coche salió del área de la señal de stop antes de completar los 3 segundos.");
            if (!GameManager.Instance.incorrectLevel.Contains("No has respetado la señal de Stop. Recuerda que esta señal indica que debes detenerte completamente antes de continuar"))
                GameManager.Instance.incorrectLevel.Add("No has respetado la señal de Stop. Recuerda que esta señal indica que debes detenerte completamente antes de continuar");

        }
    }

    private IEnumerator StopPlayer()
    {


        while (isInsideTrigger)
        {
            timeStopped += Time.deltaTime;
            if (timeStopped >= stopDuration)
            {

                Debug.Log("El coche ha permanecido detenido durante los 3 segundos en la señal de stop.");
                break;
            }
            yield return null;
        }

    }
}

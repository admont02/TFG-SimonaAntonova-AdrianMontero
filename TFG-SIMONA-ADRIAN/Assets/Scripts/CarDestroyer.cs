using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xasu.HighLevel;

public class CarDestroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<OtherCar>())
        {
            // Desactivar el objeto en lugar de destruirlo inmediatamente
            other.gameObject.GetComponentInParent<OtherCar>().destroyedByTrash = true;

            // Llamar a una corrutina para destruirlo después
            StartCoroutine(DestroyAfterDelay(other.transform.parent.gameObject, 0.1f));
        }
        //else if(other.gameObject.layer == 3)
        //{
        //    GameObjectTracker.Instance.Interacted("out-of-bonds-error", GameObjectTracker.TrackedGameObject.GameObject);
        //    Debug.LogWarning("Has salido de los límites del mapa.");
        //    if (!GameManager.Instance.incorrectLevel.Contains("Has salido de los límites del mapa."))
        //        GameManager.Instance.incorrectLevel.Add("Has salido de los límites del mapa.");
        //}
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log(obj);
        Destroy(obj);
    }

}

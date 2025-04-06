using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log(obj);
        Destroy(obj);
    }

}

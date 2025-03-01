using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // si es player cambia al nivel del trigger
        if (other.gameObject.layer == 3)
        {
            GameManager.Instance.ComprobarNivel();
            Destroy(GetComponent<BoxCollider>());
        }
    }
}

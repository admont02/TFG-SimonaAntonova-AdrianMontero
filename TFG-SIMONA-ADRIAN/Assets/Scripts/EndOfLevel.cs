using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Script asociado al objetivo de los niveles (estrella) para acabar cuando se entre en contacto
/// </summary>
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

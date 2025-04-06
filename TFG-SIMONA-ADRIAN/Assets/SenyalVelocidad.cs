using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SenyalVelocidad : MonoBehaviour
{
    public GameObject texto;
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3 && Int32.Parse(texto.GetComponent<TextMeshPro>().text) < (10 + other.gameObject.GetComponentInParent<PrometeoCarController>().carSpeed))
        {
            GameManager.Instance.incorrectLevel.Add("Has sobrepasado el límite de velocidad en: " 
                + (other.gameObject.GetComponentInParent<PrometeoCarController>().carSpeed - Int32.Parse(texto.GetComponent<TextMeshPro>().text)) + " Km/h.");

            Debug.Log("Has sobrepasado el límite de velocidad en: "

                + (other.gameObject.GetComponentInParent<PrometeoCarController>().carSpeed - Int32.Parse(texto.GetComponent<TextMeshPro>().text)) + " Km/h.");
        }
    }
}

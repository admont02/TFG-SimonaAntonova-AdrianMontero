using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SenyalVelocidad : MonoBehaviour
{

    public GameObject texto;

    public void setMaxVelocity(int maxVelocity)
    {
        texto.GetComponent<TextMeshPro>().text = maxVelocity.ToString();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3 && (10 + Int32.Parse(texto.GetComponent<TextMeshPro>().text)) <= (other.gameObject.GetComponentInParent<PrometeoCarController>().carSpeed))
        {
            GameManager.Instance.incorrectLevel.Add("Has sobrepasado el l�mite de velocidad en: "
                + (other.gameObject.GetComponentInParent<PrometeoCarController>().carSpeed - Int32.Parse(texto.GetComponent<TextMeshPro>().text)) + " Km/h.");

            Debug.Log("Has sobrepasado el l�mite de velocidad en: "

                + (other.gameObject.GetComponentInParent<PrometeoCarController>().carSpeed - Int32.Parse(texto.GetComponent<TextMeshPro>().text)) + " Km/h.");
        }
    }
}

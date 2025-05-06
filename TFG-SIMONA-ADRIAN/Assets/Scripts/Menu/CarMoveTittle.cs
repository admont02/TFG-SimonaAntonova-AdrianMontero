using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarMoveTittle : MonoBehaviour
{

    public TMP_Text textoUI;
    public float amplitud = 5f;  // Cuánto se mueve cada letra
    public float velocidad = 5f; // Velocidad de vibración

    void Update()
    {
        string originalText = textoUI.text;
        string textoModificado = "";

        for (int i = 0; i < originalText.Length; i++)
        {
            float offset = Mathf.Sin(Time.time * velocidad + i) * amplitud;
            //textoModificado += $"<voffset={offset}em>{originalText[i]}</voffset>";
        }

        textoUI.text = textoModificado;
    }
}

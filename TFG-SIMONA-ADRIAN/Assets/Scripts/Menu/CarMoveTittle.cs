using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMoveTittle : MonoBehaviour
{
    public Transform centroTitulo;  // Asigna el objeto que es el título
    public float radio = 3f;       // Distancia del coche al título
    public float velocidad = 2f;   // Velocidad de giro

    private float angulo = 0f;

    void Update()
    {
        angulo += velocidad * Time.deltaTime;
        float x = centroTitulo.position.x + Mathf.Cos(angulo) * radio;
        float y = centroTitulo.position.y + Mathf.Sin(angulo) * radio;
        transform.position = new Vector3(x, y, transform.position.z);
    }
}

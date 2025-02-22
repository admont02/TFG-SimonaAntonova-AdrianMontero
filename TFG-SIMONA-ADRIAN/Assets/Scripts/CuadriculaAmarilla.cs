using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuadriculaAmarilla : MonoBehaviour
{
    private readonly string incorrectString = "Cuadr�cula de marcas amarillas: Prohibido entrar en el cruze cuando es posible quedarse inmovilizado en �l";
    private void Start()
    {
        Vector3 position = gameObject.transform.position;
        position.y = 0.1f;
        gameObject.transform.position = position;

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Rigidbody>() != null)
            if (other.gameObject.layer == 3 && (Vector3.Dot(other.gameObject.transform.parent.forward, other.gameObject.GetComponentInParent<Rigidbody>().velocity) < 2.0f))
            {
                //Debug.Log(Vector3.Dot(other.gameObject.transform.parent.forward, other.gameObject.GetComponentInParent<Rigidbody>().velocity));
                if (GameManager.Instance != null)
                    if (!GameManager.Instance.incorrectLevel.Contains(incorrectString))
                        GameManager.Instance.incorrectLevel.Add(incorrectString);
            }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuadriculaAmarilla : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3 && (Vector3.Dot(other.gameObject.transform.parent.forward, other.gameObject.GetComponentInParent<Rigidbody>().velocity) < 2.0f))
        {
            Debug.Log(Vector3.Dot(other.gameObject.transform.parent.forward, other.gameObject.GetComponentInParent<Rigidbody>().velocity));

            GameManager.Instance.incorrectLevel.Add("Cuadrícula de marcas amarillas: Prohibido entrar en el cruze cuando es posible quedarse inmovilizado en él");
        }
    }
}

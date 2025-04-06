using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private void Start()
    {

        Debug.Log("hola start");
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hola collision");
        if (collision.gameObject.layer == 3)
        {

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hola trigger");
        if (other.gameObject.layer == 3)
        {

            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float duration = 1f; // Tiempo antes de destruir

    void Start()
    {
        Destroy(gameObject, duration);
    }
}

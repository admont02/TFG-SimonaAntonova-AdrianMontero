using UnityEngine;

public class LockRotation : MonoBehaviour
{
    private Quaternion initialRotation; // Rotaci�n inicial del hijo

    void Start()
    {
        // Guarda la rotaci�n inicial del hijo
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // Mant�n la rotaci�n inicial del hijo
        transform.rotation = initialRotation;
    }
}

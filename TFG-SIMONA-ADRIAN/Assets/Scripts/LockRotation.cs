using UnityEngine;

public class LockRotation : MonoBehaviour
{
    private Quaternion initialRotation; // Rotación inicial del hijo

    void Start()
    {
        // Guarda la rotación inicial del hijo
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // Mantén la rotación inicial del hijo
        transform.rotation = initialRotation;
    }
}

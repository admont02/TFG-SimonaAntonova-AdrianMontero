using UnityEngine;
/// <summary>
/// Clase que gestiona el parpadeo del objetivo en el minimapa
/// </summary>
public class MeshBlinkEffect : MonoBehaviour
{
    public float blinkSpeed = 1f; 
    private Material meshMaterial; 
    private Color originalColor; 
    public Color blinkColor = Color.yellow;

    private void Start()
    {
       
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshMaterial = meshRenderer.material; 
            originalColor = meshMaterial.color; 
        }
    }

    private void Update()
    {
        if (meshMaterial != null)
        {
            
            float t = Mathf.PingPong(Time.time * blinkSpeed, 1f); 
            meshMaterial.color = Color.Lerp(originalColor, blinkColor, t);
        }
    }
}

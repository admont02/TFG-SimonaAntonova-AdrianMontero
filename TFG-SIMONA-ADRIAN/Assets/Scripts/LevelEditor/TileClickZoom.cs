using UnityEngine;
/// <summary>
/// Clase que gestiona el zoom sobre un tile en el editor
/// </summary>
public class TileClickZoom : MonoBehaviour
{
    public float zoomScale = 3f; //Zoom del tile
    public float zoomDuration = 0.5f; //Duración animación zoom
    private Vector3 originalScale; //Escala original del tile
    private bool isZoomed = false;
    private GameObject gridParent;

    void Start()
    {
        //Guarda la escala tile 
        originalScale = transform.localScale;
        zoomScale = 700 / GetComponent<RectTransform>().sizeDelta.x;
        Debug.Log("or scali" + GetComponent<RectTransform>().sizeDelta.x);

        gridParent = transform.parent.gameObject;
    }

    void OnMouseDown()
    {
        if (!isZoomed)
        {
            //zoom en el tile clicado
            Debug.Log($"Tile {name} clicado, aplicando zoom.");

            //Ampliar el tile
            StartCoroutine(ZoomTile());

            //Ocultar otros tiles
            ToggleOtherTilesVisibility(false);

            //Cambiar estado a zoom activado
            isZoomed = true;
        }
    }

    void Update()
    {
        // Detectar si se presiona la tecla Escape para salir del zoom
        if (isZoomed && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Saliendo del zoom.");

            //Restaurar el tile a su tamaño original
            StartCoroutine(ResetZoomTile());

            //Mostrar los demás tiles
            ToggleOtherTilesVisibility(true);

            //Cambiar estado a zoom desactivado
            isZoomed = false;
        }
    }

    private System.Collections.IEnumerator ZoomTile()
    {
        float elapsedTime = 0f;
        Vector3 targetScale = originalScale * zoomScale;

        //Centra la cámara en el tile clicado
        FocusCameraOnTile();

        //Animación de zoom
        while (elapsedTime < zoomDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / zoomDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    private System.Collections.IEnumerator ResetZoomTile()
    {
        float elapsedTime = 0f;
        Vector3 targetScale = originalScale;

        //Efecto como de desvanecimiento para volver al tamaño original
        while (elapsedTime < zoomDuration)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, elapsedTime / zoomDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; //tamaño original
    }

    private void FocusCameraOnTile()
    {
        //Centrar la cámara en el tile clicado
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }

    private void ToggleOtherTilesVisibility(bool visible)
    {
        //Ocultar o mostrar todos los tiles menos este
        foreach (Transform tile in gridParent.transform)
        {
            if (tile != transform)
            {
                tile.gameObject.SetActive(visible);
            }
        }
    }
}

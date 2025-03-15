using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition; // Guarda la posición inicial
    private Canvas canvas;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        // Guarda la posición original del objeto en Start
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Ajustar propiedades para que sea más visible y no bloquee eventos
        canvasGroup.alpha = 0.6f; // Hace la pieza más transparente durante el drag
        canvasGroup.blocksRaycasts = false; // Permite que los eventos pasen a través
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Mueve el objeto mientras se arrastra
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restaura la opacidad y vuelve a bloquear los raycasts
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Devuelve el objeto a su posición original
        rectTransform.anchoredPosition = originalPosition;
    }
}

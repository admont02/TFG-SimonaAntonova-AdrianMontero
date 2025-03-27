using UnityEngine;
using UnityEngine.EventSystems;
public enum DraggableType { Pieza, TrafficElem };

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition; // Guarda la posici�n inicial
    private Canvas canvas;
    public DraggableType draggableType;
    public Transform gridParent;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        // Guarda la posici�n original del objeto en Start
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Ajustar propiedades para que sea m�s visible y no bloquee eventos
        canvasGroup.alpha = 0.6f; // Hace la pieza m�s transparente durante el drag
        canvasGroup.blocksRaycasts = false; // Permite que los eventos pasen a trav�s
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Mueve el objeto mientras se arrastra
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        //Detectar si el objeto fue soltado sobre una celda v�lida del grid
        if (eventData.pointerEnter != null && eventData.pointerEnter.transform.IsChildOf(gridParent) && draggableType!=DraggableType.TrafficElem)
        {
            Transform cell = eventData.pointerEnter.transform;

            //Verificar si la celda ya tiene una pieza colocada
            DraggableItem existingItem = cell.GetComponentInChildren<DraggableItem>();

            if (existingItem != null)
            {
                // No reemplazar una pieza por un elemento de tr�fico o viceversa
                if (existingItem.draggableType != this.draggableType)
                {
                    Debug.Log($"No se puede sustituir {existingItem.draggableType} con {this.draggableType}");
                    rectTransform.anchoredPosition = originalPosition; // Regresa a la posici�n inicial
                    return;
                }
            }

            
            Debug.Log($"Pieza {name} colocada en {cell.name}");

            GameObject copy = Instantiate(gameObject, cell);
            copy.name = gameObject.name; 
            copy.transform.localPosition = Vector3.zero;
            rectTransform.anchoredPosition = originalPosition;

        }
        else
        {
            //Si no se suelta sobre una celda v�lida, regresar a la posici�n original
            rectTransform.anchoredPosition = originalPosition;
        }
    }



}

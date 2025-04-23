using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public enum DraggableType { Pieza, TrafficElem, Car };

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition; // Guarda la posición inicial
    private Vector2 originalSize; // Guarda la posición inicial
    private Canvas canvas;
    private int originalSiblingIndex;


    public DraggableType draggableType;
    public Transform gridParent;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        originalSiblingIndex = transform.GetSiblingIndex();

        // Guarda la posición original del objeto en Start
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalSize = eventData.pointerDrag.GetComponent<RectTransform>().sizeDelta;
        // Ajustar propiedades para que sea más visible y no bloquee eventos
        canvasGroup.alpha = 0.6f; // Hace la pieza más transparente durante el drag
        canvasGroup.blocksRaycasts = false; // Permite que los eventos pasen a través

        Debug.Log(rectTransform.anchoredPosition + ",  og:" + originalPosition);
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
        Debug.Log(rectTransform.anchoredPosition + ",  og:" + originalPosition);

        //Detectar si el objeto fue soltado sobre una celda válida del grid
        if (eventData.pointerEnter != null && eventData.pointerEnter.transform.IsChildOf(gridParent) && draggableType == DraggableType.Pieza)
        {
            Transform cell = eventData.pointerEnter.transform;

            //Verificar si la celda ya tiene una pieza colocada
            DraggableItem existingItem = cell.GetComponentInChildren<DraggableItem>();

            if (existingItem != null)
            {
                //No reemplazar una pieza por un elemento de tráfico o viceversa
                if (existingItem.draggableType != this.draggableType)
                {
                    Debug.Log($"No se puede sustituir {existingItem.draggableType} con {this.draggableType}");
                    var a = transform.parent;
                    transform.SetParent(gridParent, false);
                    transform.SetParent(a, false);
                    transform.SetSiblingIndex(originalSiblingIndex);
                    return;
                }
                else
                {
                    Debug.Log("cambiar pieza");

                    Destroy(existingItem.gameObject);

                    Debug.Log($"Pieza {name} colocada en {cell.parent.name}");

                    GameObject copy = Instantiate(gameObject, cell.parent);
                    copy.name = gameObject.name;
                    copy.transform.localPosition = Vector3.zero;
                    var a = transform.parent;
                    transform.SetParent(gridParent, false);
                    transform.SetParent(a, false);
                    transform.SetSiblingIndex(originalSiblingIndex);
                }
            }
            else
            {
                if (cell.GetComponent<InteractivePoint>() == null)
                {
                    Debug.Log($"Pieza {name} colocada en {cell.name}");

                    GameObject copy = Instantiate(gameObject, cell);
                    copy.name = gameObject.name;
                    copy.transform.localPosition = Vector3.zero;
                    var a = transform.parent;
                    transform.SetParent(gridParent, false);

                    transform.SetParent(a, false);
                    transform.SetSiblingIndex(originalSiblingIndex);
                    rectTransform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

                    //rectTransform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    var a = transform.parent;
                    transform.SetParent(gridParent, false);
                    transform.SetParent(a, false);
                    transform.SetSiblingIndex(originalSiblingIndex);
                }

            }


        }
        else
        {
            // Si no se suelta sobre una celda válida, volver al grid original
            var a = transform.parent;
            transform.SetParent(gridParent, false);
            transform.SetParent(a, false);
            transform.SetSiblingIndex(originalSiblingIndex);

            //rectTransform.localScale = Vector3.one;

            //LayoutRebuilder.ForceRebuildLayoutImmediate(gridParent.GetComponent<RectTransform>());
        }

    }



}

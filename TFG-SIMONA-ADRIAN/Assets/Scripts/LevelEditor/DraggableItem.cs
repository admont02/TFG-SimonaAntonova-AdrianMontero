using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
//Tipo del elemento arrastrable
public enum DraggableType { Pieza, TrafficElem, Car };
/// <summary>
/// Componente asociado tanto a las piezas como a los elementos del editor. Se encarga de gestionar el drag&drop dentro del mapa. 
/// </summary>
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition; //Guarda la posición inicial
    private Vector2 originalSize; //Guarda el tamaño inicial
    private Canvas canvas;
    private int originalSiblingIndex;


    public DraggableType draggableType;
    public Transform gridParent;
    private Transform originalParent;
    public bool drag = true;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        originalSiblingIndex = transform.GetSiblingIndex();

        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!drag) return;
        originalSize = eventData.pointerDrag.GetComponent<RectTransform>().sizeDelta;
        canvasGroup.alpha = 0.6f; 
        canvasGroup.blocksRaycasts = false; 
        originalParent = transform.parent;//Guardar padre original.
        transform.SetParent(canvas.transform, true);
        Debug.Log(rectTransform.anchoredPosition + ",  og:" + originalPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!drag) return;

        //Mueve el objeto mientras se arrastra
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!drag) return;

        transform.SetParent(originalParent, true);
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
                    //cambiar la pieza por otra
                    Debug.Log("cambiar pieza");

                    Destroy(existingItem.gameObject);

                    Debug.Log($"Pieza {name} colocada en {cell.parent.name}");

                    GameObject copy = Instantiate(gameObject, cell.parent);
                    copy.name = gameObject.name;
                    copy.transform.localPosition = Vector3.zero;
                    copy.GetComponent<TooltipTriggerUI>().enabled = false;
                    copy.GetComponent<DraggableItem>().drag = false;

                    Transform childTransform = copy.transform.GetChild(0);
                    if (childTransform != null)
                    {
                        foreach (Transform subChild in childTransform)
                        {
                            InteractivePoint interactivePoint = subChild.GetComponent<InteractivePoint>();
                            if (interactivePoint != null)
                            {
                                interactivePoint.gameObject.SetActive(true);
                                Debug.Log($"Activado InteractivePoint en {subChild.name}");
                            }
                        }
                    }
                    
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
                    copy.GetComponent<TooltipTriggerUI>().enabled = false;
                    copy.GetComponent<DraggableItem>().drag = false;
                    
                    var a = transform.parent;
                    transform.SetParent(gridParent, false);

                    transform.SetParent(a, false);
                    transform.SetSiblingIndex(originalSiblingIndex);
                    if (rectTransform.childCount > 0)
                        rectTransform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    //Activar todos los hijos que tengan el script InteractivePoint
                    if (transform.childCount > 0)
                    {
                        Transform childTransform = copy.transform.GetChild(0);
                        if (childTransform != null)
                        {
                            foreach (Transform subChild in childTransform)
                            {
                                InteractivePoint interactivePoint = subChild.GetComponent<InteractivePoint>();
                                if (interactivePoint != null)
                                {
                                    interactivePoint.gameObject.SetActive(true);
                                    Debug.Log($"Activado InteractivePoint en {subChild.name}");
                                }
                            }
                        }
                    }



                    //rectTransform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    //rectTransform.anchoredPosition = originalPosition; //Regresa a la posici?n inicial
                    //rectTransform.sizeDelta = originalSize;
                    var a = transform.parent;
                    transform.SetParent(gridParent, false);
                    transform.SetParent(a, false);
                    transform.SetSiblingIndex(originalSiblingIndex);
                }
            }
        }
        else
        {
            //Si no se suelta sobre una celda válida, volver al grid original
            var a = transform.parent;
            transform.SetParent(gridParent, false);
            transform.SetParent(a, false);
            transform.SetSiblingIndex(originalSiblingIndex);
            
        }

    }



}

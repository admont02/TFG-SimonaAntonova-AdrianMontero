using UnityEngine;
using UnityEngine.EventSystems;

public class InteractivePoint : MonoBehaviour, IDropHandler
{
    public int fil; 
    public int col; 

    //Método que se ejecuta cuando algo se suelta sobre el punto
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggable = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (draggable != null && draggable.draggableType==DraggableType.TrafficElem)
        {
            Debug.Log($"Elemento {draggable.name} colocado en el punto interactivo [fil: {fil}, col: {col}]");


            GameObject copy = Instantiate(draggable.gameObject, transform);
            copy.name = draggable.gameObject.name;
            copy.transform.localPosition = Vector3.zero;
          

            //Ajustar el tamaño del objeto para que coincida con el InteractivePoint
            RectTransform interactiveRect = GetComponent<RectTransform>();
            RectTransform draggableRect = copy.GetComponent<RectTransform>();

            if (interactiveRect != null && draggableRect != null)
            {
                draggableRect.sizeDelta = interactiveRect.sizeDelta; //Igualar tamaños
            }

            //json?
        }
    }

}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// Clase que representa los lugares de una pieza donde se puede colocar un elemento durante la creacion de un nivel en el editor.
/// </summary>
public class InteractivePoint : MonoBehaviour, IDropHandler
{
    public DraggableType acceptedType;
    public int fil; 
    public int col;

    public string orientacion;

    //Método que se ejecuta cuando algo se suelta sobre el punto
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggable = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (draggable != null && draggable.draggableType==acceptedType)
        {
            Debug.Log($"Elemento {draggable.name} colocado en el punto interactivo [fil: {fil}, col: {col}]");

            if (transform.childCount > 0)
            {
                //Borrar el hijo existente
                Transform existingChild = transform.GetChild(0); 
                Debug.Log($"Eliminando hijo existente: {existingChild.name}");
                Destroy(existingChild.gameObject); 
            }
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
            if (copy.GetComponent<BotonCoche>())
            {
                //añadir boton de vehiculo no manejable para poder configurarlo
                copy.GetComponent<BotonCoche>().AddButton();

                copy.GetComponent<CanvasGroup>().enabled = false;
                copy.GetComponent<DraggableItem>().enabled = false;
                copy.GetComponent<TooltipTriggerUI>().enabled = false;

            }
            
        }
    }

}

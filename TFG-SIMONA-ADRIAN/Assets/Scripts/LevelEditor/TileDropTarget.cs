using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// Clase encargada del cambio de imagen al dropear una pieza sobre un tile en el editor
/// </summary>
public class TileDropTarget : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //Obtener el objeto que se dropea
        GameObject droppedItem = eventData.pointerDrag;

        if (droppedItem != null && droppedItem.GetComponent<DraggableItem>().draggableType == DraggableType.Pieza)
        {
            Image droppedImage = droppedItem.GetComponent<Image>();
            Image tileImage = GetComponent<Image>();

            if (droppedImage != null && tileImage != null)
            {
                //Cambiar la imagen del tile por la imagen de la pieza
                tileImage.sprite = droppedImage.sprite;
                tileImage.color = droppedImage.color;
            }
            //Ajustar el tamaño del objeto para que coincida con el InteractivePoint
            RectTransform interactiveRect = tileImage.GetComponent<RectTransform>();
            RectTransform draggableRect = droppedImage.GetComponent<RectTransform>();

            if (interactiveRect != null && draggableRect != null)
            {
                var aux = draggableRect.sizeDelta;
                draggableRect.sizeDelta = interactiveRect.sizeDelta; //Igualar tamaños
                if (draggableRect.childCount > 0)
                    draggableRect.GetChild(0).GetComponent<RectTransform>().localScale =
                        (draggableRect.GetChild(0).GetComponent<RectTransform>().localScale * interactiveRect.sizeDelta) / aux;
                
            }
        }
    }
}

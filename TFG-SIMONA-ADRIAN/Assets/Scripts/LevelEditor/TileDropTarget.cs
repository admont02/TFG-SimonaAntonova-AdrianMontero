using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileDropTarget : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Obtener el objeto que se está arrastrando
        GameObject droppedItem = eventData.pointerDrag;

        if (droppedItem != null && droppedItem.GetComponent<DraggableItem>().draggableType == DraggableType.Pieza)
        {
            // Obtener el componente Image de la pieza y del tile
            Image droppedImage = droppedItem.GetComponent<Image>();
            Image tileImage = GetComponent<Image>();

            if (droppedImage != null && tileImage != null)
            {
                // Cambiar la imagen del tile por la imagen de la pieza
                tileImage.sprite = droppedImage.sprite;

                // (Opcional) Cambiar el color para que coincida con la pieza
                tileImage.color = droppedImage.color;
            }

            //Ajustar el tamaño del objeto para que coincida con el InteractivePoint
            RectTransform interactiveRect = tileImage.GetComponent<RectTransform>();
            RectTransform draggableRect = droppedImage.GetComponent<RectTransform>();

            if (interactiveRect != null && draggableRect != null)
            {
                var aux = draggableRect.sizeDelta;
                draggableRect.sizeDelta = interactiveRect.sizeDelta; //Igualar tamaños
                draggableRect.GetChild(0).GetComponent<RectTransform>().localScale =
                    (draggableRect.GetChild(0).GetComponent<RectTransform>().localScale * interactiveRect.sizeDelta) / aux;

                //while (0 < draggableRect.GetChild(0).childCount)
                //{
                //    draggableRect.GetChild(0).GetChild(0).SetParent(draggableRect);
                //}
            }
        }
    }
}

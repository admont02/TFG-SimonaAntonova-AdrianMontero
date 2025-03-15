using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileDropTarget : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Obtener el objeto que se está arrastrando
        GameObject droppedItem = eventData.pointerDrag;

        if (droppedItem != null)
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
        }
    }
}

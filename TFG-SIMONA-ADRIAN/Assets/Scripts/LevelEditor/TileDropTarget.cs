using UnityEngine;
using UnityEngine.EventSystems;

public class TileDropTarget : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;

        if (droppedItem != null)
        {
            RectTransform droppedRect = droppedItem.GetComponent<RectTransform>();
            droppedRect.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }
}

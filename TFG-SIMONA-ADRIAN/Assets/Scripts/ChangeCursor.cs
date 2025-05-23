using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D customCursor; // Tu textura personalizada para el cursor
    public Vector2 cursorHotspot = Vector2.zero; // Anclaje del cursor

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(customCursor, cursorHotspot, CursorMode.Auto); // Cambia el cursor
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, cursorHotspot, CursorMode.Auto); // Restaurar el cursor original
    }
}

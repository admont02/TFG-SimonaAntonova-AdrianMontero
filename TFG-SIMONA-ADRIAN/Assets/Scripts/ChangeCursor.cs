using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Clase que se encarga de cambiar el cursor cuando esta sobre un cuadro de dialogo
/// </summary>
public class ChangeCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D customCursor; 
    public Vector2 cursorHotspot = Vector2.zero; 

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(customCursor, cursorHotspot, CursorMode.Auto); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, cursorHotspot, CursorMode.Auto); 
    }
}

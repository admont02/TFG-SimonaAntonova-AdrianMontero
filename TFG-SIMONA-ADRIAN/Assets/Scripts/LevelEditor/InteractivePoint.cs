using UnityEngine;
using UnityEngine.EventSystems;

public class InteractivePoint : MonoBehaviour, IDropHandler
{
    public int fil; // Fila asociada al punto
    public int col; // Columna asociada al punto

    // M�todo que se ejecuta cuando algo se suelta sobre el punto
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggable = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (draggable != null)
        {
            Debug.Log($"Elemento {draggable.name} colocado en el punto interactivo [fil: {fil}, col: {col}]");
            draggable.transform.position = transform.position;

            //Aqu� puedes guardar la informaci�n en el JSON o mapa
            //LevelManager.Instance.AddStop(fil, col, draggable.name); // Ejemplo de guardado
        }
    }
}

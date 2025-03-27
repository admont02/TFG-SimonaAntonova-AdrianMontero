using UnityEngine;
using UnityEngine.EventSystems;

public class InteractivePoint : MonoBehaviour, IDropHandler
{
    public int fil; // Fila asociada al punto
    public int col; // Columna asociada al punto

    // Método que se ejecuta cuando algo se suelta sobre el punto
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggable = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (draggable != null)
        {
            Debug.Log($"Elemento {draggable.name} colocado en el punto interactivo [fil: {fil}, col: {col}]");
            draggable.transform.position = transform.position;

            //Aquí puedes guardar la información en el JSON o mapa
            //LevelManager.Instance.AddStop(fil, col, draggable.name); // Ejemplo de guardado
        }
    }
}

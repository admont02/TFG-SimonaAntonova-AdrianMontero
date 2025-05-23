using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
/// <summary>
/// Clase que detecta si se coloca el raton sobre un elemento del editor para mostrar el tooltip
/// </summary>
public class TooltipTriggerUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string tooltipMessage; 
    public TooltipManager tooltipManager;

    public void OnPointerEnter(PointerEventData eventData)
    {

        Vector3 offsetPosition = transform.position + new Vector3(0, 3.5f,0); 
        tooltipManager.ShowTooltip(tooltipMessage, offsetPosition);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipManager.HideTooltip();
    }
}

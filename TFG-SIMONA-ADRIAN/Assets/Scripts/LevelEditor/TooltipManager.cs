using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// Clase que gestiona el tooltip informativo al colocar el raton sobre un elemento o pieza del editor
/// </summary>
public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance;
    public GameObject tooltipPanel;  
    public TMP_Text tooltipText;
   // public RectTransform canvasTransform;

    void Awake()
    {
        instance = this;
        tooltipPanel.SetActive(false); 
    }

    public void ShowTooltip(string message, Vector3 position)
    {
        tooltipText.text = message; 
        tooltipPanel.transform.position = position; 
        tooltipPanel.SetActive(true);
    }

    public void HideTooltip()
    {
        
        tooltipPanel.SetActive(false);
    }
}

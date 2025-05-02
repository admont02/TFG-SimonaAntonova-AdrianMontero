using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        Debug.Log("escondo");
        tooltipPanel.SetActive(false);
    }
}

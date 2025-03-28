using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject[] panels; //Los paneles a navegar
    public int currentPanelIndex = 0; //El panel actual

    void Start()
    {
        UpdatePanelVisibility();
    }

    public void ShowNextPanel()
    {
        //Avanza al siguiente panel
        currentPanelIndex = (currentPanelIndex + 1) % panels.Length;
        UpdatePanelVisibility();
    }

    public void ShowPreviousPanel()
    {
        //Retrocede al panel anterior
        currentPanelIndex = (currentPanelIndex - 1 + panels.Length) % panels.Length;
        UpdatePanelVisibility();
    }

    private void UpdatePanelVisibility()
    {
        //Muestra solo el panel activo
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == currentPanelIndex);
        }
    }
}

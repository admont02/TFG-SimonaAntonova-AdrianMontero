using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownChanged : MonoBehaviour
{
    //public TMP_Dropdown dropdown;
    public Dropdown dropdown;
    public GameObject LucesPanel;
    public GameObject PrioridadPanel;
    public GameObject MapaPrioridad;
    public GameObject Mapa;

    void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        OnDropdownValueChanged(dropdown.value); // Llamar al inicio para estado inicial
    }

    void OnDropdownValueChanged(int index)
    {
        string selectedOption = dropdown.options[index].text;
        if (selectedOption == "Prioridad")
        {
            PrioridadPanel.SetActive(true);
            MapaPrioridad.SetActive(true);
            Mapa.SetActive(false);
        }
        else
        {
            PrioridadPanel.SetActive(false);
            MapaPrioridad.SetActive(false);
            Mapa.SetActive(true);
        }
        if (selectedOption == "Luces")
        {
            LucesPanel.SetActive(true);
        }
        else
        {
            LucesPanel.SetActive(false);


        }
    }
}

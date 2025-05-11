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
    public GameObject dimensiones;
    public GameObject alto;
    public GameObject ancho;
    public GameObject botonGenerar;


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
            dimensiones.SetActive(false);
            alto.SetActive(false);
            ancho.SetActive(false);
            botonGenerar.SetActive(false);
        }
        else
        {
            PrioridadPanel.SetActive(false);
            MapaPrioridad.SetActive(false);
            Mapa.SetActive(true);
            dimensiones.SetActive(true);
            alto.SetActive(true);
            ancho.SetActive(true);
            botonGenerar.SetActive(true);
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

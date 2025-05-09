using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownChanged : MonoBehaviour
{
    //public TMP_Dropdown dropdown;
    public Dropdown dropdown;
    public GameObject objetoQueQuieroActivar;

    void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        OnDropdownValueChanged(dropdown.value); // Llamar al inicio para estado inicial
    }

    void OnDropdownValueChanged(int index)
    {
        string selectedOption = dropdown.options[index].text;

        if (selectedOption == "Luces")
        {
            objetoQueQuieroActivar.SetActive(true);
        }
        else
        {
            objetoQueQuieroActivar.SetActive(false);
        }
    }
}

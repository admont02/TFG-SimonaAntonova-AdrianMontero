using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
/// <summary>
/// Clase encargada de la configuración de los vehículos no manejables en el editor de niveles
/// </summary>
public class IACarConfig : MonoBehaviour
{
    [SerializeField]
    GameObject panelConfig;
    [SerializeField]
    Toggle recto;
    [SerializeField]
    Toggle dcha;
    [SerializeField]
    Toggle izqda;
    [SerializeField]
    public TMP_InputField index;
    [SerializeField]
    GameObject toggleGroup;
    private CocheIAEditorData cocheActual; // Referencia al coche seleccionado

    public void MostrarPanel(CocheIAEditorData coche)
    {
        if (panelConfig.activeSelf)
            OcultarPanel();
        cocheActual = coche;
        panelConfig.SetActive(true); //Mostrar el panel
        toggleGroup.SetActive(true);
        index.gameObject.SetActive(true);
        //Cargar los datos del coche en el panel

        index.text = coche.index.ToString();
        recto.isOn = coche.recto;
        dcha.isOn = coche.dcha;
        izqda.isOn = coche.izqda;

        index.onValueChanged.AddListener(delegate { ActualizarID(); });
        recto.onValueChanged.AddListener(delegate { cocheActual.recto = recto.isOn; });
        dcha.onValueChanged.AddListener(delegate { cocheActual.dcha = dcha.isOn; });
        izqda.onValueChanged.AddListener(delegate { cocheActual.izqda = izqda.isOn; });
    }
    private void ActualizarID()
    {
        if (int.TryParse(index.text, out int nuevoID))
        {
            cocheActual.index = nuevoID - 1;
        }
    }

    public void OcultarPanel()
    {
        panelConfig.SetActive(false);
        cocheActual = null;

        index.onValueChanged.RemoveAllListeners();
        recto.onValueChanged.RemoveAllListeners();
        dcha.onValueChanged.RemoveAllListeners();
        izqda.onValueChanged.RemoveAllListeners();
    }

}

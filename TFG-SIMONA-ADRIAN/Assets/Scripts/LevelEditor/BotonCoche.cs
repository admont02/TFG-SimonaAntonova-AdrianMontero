using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Clase que crea un botón en el icono de los vehículos no controlables para configurarlos desde el editor.
/// </summary>
public class BotonCoche : MonoBehaviour
{
    public CocheIAEditorData cocheData; //Datos del coche asociado
    public IACarConfig uiManager;
    private Button boton;
    public GameObject mapaPrioridad;

    void Start()
    {
        uiManager = FindObjectOfType<IACarConfig>(); 
       
    }
    public void AddButton()
    {
        if (mapaPrioridad.activeSelf)
        {
            uiManager.MostrarPanel(cocheData);
            Debug.Log("Añadido boton");
            boton = transform.gameObject.AddComponent<Button>();
            boton.onClick.AddListener(() => uiManager.MostrarPanel(cocheData));
        }
        
    }
}

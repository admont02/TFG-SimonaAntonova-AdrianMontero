using UnityEngine;
using UnityEngine.UI;

public class BotonCoche : MonoBehaviour
{
    public CocheIAEditorData cocheData; // Datos del coche asociado
    public IACarConfig uiManager;
    private Button boton;
    public GameObject mapaPrioridad;

    void Start()
    {
        uiManager = FindObjectOfType<IACarConfig>(); // Buscar el UI Manager en la escena
       // Asignar evento al botón
    }
    public void AddButton()
    {
        if (mapaPrioridad.activeSelf)
        {
            Debug.Log("Añadido boton");
            boton = transform.gameObject.AddComponent<Button>();
            boton.onClick.AddListener(() => uiManager.MostrarPanel(cocheData));
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Se encarga de cargar el menu desde los creditos
/// </summary>
public class MenuLoader : MonoBehaviour
{

    public float timeToLoadMenu = 19f;
    private void Start()
    {
        Invoke("LoadMenu", timeToLoadMenu);
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}

using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int filas { get; internal set; }
    public int columnas { get; internal set; }

    public DialogueSystem dialogueSystem;
    public bool canCarMove = false; // Booleano para controlar el movimiento del coche

    private LevelLoader nivelLoader;
    public CarController carController;
    public OtherCar otherCar;
    private GameObject playerTarget;
    public List<GameObject> cochesIA = new List<GameObject>();

    public List<string> incorrectLevel = new List<string>();
    public List<GameObject> priorityCarList = new List<GameObject>();

    public CinemachineVirtualCamera virtualCamera;

    void Awake()
    {
        incorrectLevel.Add("Nivel Incorrecto, errores: ");
        // Asegurarse de que solo hay una instancia de GameManager
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Mantener el GameManager entre escenas

            // Inicializar cualquier cosa necesaria al inicio de la escena
            InitializeNivelLoader();
            InitializeDialogue();

        }
        else
        {
            Destroy(gameObject);
        }

    }

    void InitializeDialogue()
    {
        if (dialogueSystem != null)
        {
            dialogueSystem.StartDialogue(false);
        }
    }
    public void SetPlayer(Transform t)
    {
        virtualCamera.LookAt = t;
        virtualCamera.Follow = t;
    }
    private void Update()
    {
        //if (!completed)
        //    ComprobarNivel();
        //Debug.Log(priorityCarList.Count);
    }
    void InitializeNivelLoader()
    {

        nivelLoader = FindObjectOfType<LevelLoader>();
        if (nivelLoader != null)
        {
            nivelLoader.CargarNivel();
        }
        else
        {
            Debug.LogError("No se encontró NivelLoader en la escena.");
        }
    }
    public void ComprobarNivel()
    {
        //int id = 0;
        //foreach (var item in priorityCarList)
        //{
        //    if (item.name[item.name.Length - 1].ToString() != id.ToString())
        //    {
        //        incorrectLevel.Add("Prioridades incorrectas");
        //        break;
        //    }
        //    id++;
        //}
        if (incorrectLevel.Count > 1)
        {
            dialogueSystem.ShowIncorrectLevelDialog(incorrectLevel.ToArray());
            Debug.Log("¡Nivel incorrecto!");
            foreach (string nivel in incorrectLevel)
            {
                Debug.Log(nivel);
            }
        }
        else
        {
            dialogueSystem.ShowCompletedDialog();
            Debug.Log("¡Nivel completado correctamente!");
        }
    }

    internal void SetPlayerTarget(GameObject targetPoint)
    {
        playerTarget = targetPoint;
    }
    public void AddCocheIA(GameObject coche)
    {
        cochesIA.Add(coche);
    }
    public void EnableFog()
    {
        carController.gameObject.GetComponent<CarLights>().fog.SetActive(true);
        //RenderSettings.fog = true;
        //RenderSettings.fogColor = Color.gray;
        //RenderSettings.fogMode = FogMode.ExponentialSquared;
        //RenderSettings.fogStartDistance = 0.5f;
        //RenderSettings.fogEndDistance = 80f;
        //RenderSettings.fogDensity = 0.05f;
    }
    public void ChangeScene(string sceneName)
    {
        if (sceneName == "World")
            SceneData.JsonFileName = "menu.json";
        SceneManager.LoadScene(sceneName);
    }
    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}

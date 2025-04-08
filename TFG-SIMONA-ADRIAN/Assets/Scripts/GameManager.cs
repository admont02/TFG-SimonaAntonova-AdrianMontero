using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Xasu.HighLevel;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int filas { get; internal set; }
    public int columnas { get; internal set; }
    public float scale { get; internal set; }
    public string CurrentLevel { get; internal set; }

    public DialogueSystem dialogueSystem;
    public bool canCarMove = false; // Booleano para controlar el movimiento del coche

    private LevelLoader nivelLoader;
    public PrometeoCarController carController;
    public OtherCar otherCar;
    public GameObject playerTarget;
    public GameObject rain;
    public List<GameObject> cochesIA = new List<GameObject>();

    public List<string> incorrectLevel = new List<string>();
    public List<GameObject> priorityCarList = new List<GameObject>();
    public GameObject minimapCamera;
    public CinemachineVirtualCamera virtualCamera;
    public GameObject clicLevelCam;
    public GameObject velocidad;
    public Digrafo graph;

    public TextMeshProUGUI velText;

    public GameObject PerspectiveButton;
    public GameObject LightsPanel;
    public GameObject antinieblaDelanteras;
    public GameObject antinieblaTraseras;
    public GameObject posicion;
    public GameObject largas;
    public GameObject cortas;
    public GameObject comenzar;
    private int currentLevel;
    public Material graySkybox;
    public Light directionalLight;

    private float timer = 0f;
    public float trackingInterval = 10f;
    private bool inPerspectView;

    public bool finDeNivel { get; private set; }

    void Awake()
    {
        finDeNivel = false;
        incorrectLevel.Add("Nivel Incorrecto, errores: ");
        // Asegurarse de que solo hay una instancia de GameManager
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Mantener el GameManager entre escenas

            // Inicializar cualquier cosa necesaria al inicio de la escena
            InitializeNivelLoader();

            InitializeDialogue();
            if (ClicLevelManager.Instance != null)
            {
                velocidad.SetActive(false);
                Debug.Log("hola");
                minimapCamera.SetActive(false);
                //SetPlayer(cochesIA[0].transform);
                Camera.main.gameObject.SetActive(false);
                clicLevelCam.gameObject.SetActive(true);
                clicLevelCam.tag = "MainCamera";
                //clicLevelCam.GetComponentInChildren<CinemachineBrain>().enabled = true;
                //for (int i = 0; i < cochesIA.Count; i++)
                //{
                //    //cochesIA[i].GetComponent<OtherCar>().icon.SetActive(false);
                //    cochesIA[i].GetComponentInChildren<CinemachineVirtualCamera>().enabled = true;

                //}
            }
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Start()
    {
        if (currentLevel != 0)
            CompletableTracker.Instance.Initialized("nivel" + currentLevel.ToString());

    }
    void InitializeDialogue()
    {
        if (dialogueSystem != null)
        {
            if (SceneData.JsonFileName == "menu.json")
            {
                if (SceneData.firstTime)
                {
                    dialogueSystem.StartDialogue(false);
                    SceneData.firstTime = false;
                }
                else
                {
                    Debug.Log("de vuelta al menu");
                    dialogueSystem.StopAllCoroutines();
                    dialogueSystem.dialoguePanel.SetActive(false);
                    dialogueSystem.ResetDialogue();
                    dialogueSystem.GetComponent<AudioSource>().Stop();

                    // Detenemos todas las corrutinas para evitar sonidos y texto

                    // dialogueSystem.ResetDialogue();

                }

            }
            else if (SceneData.JsonFileName != "menu.json")
            {
                dialogueSystem.StartDialogue(false);

            }
        }
    }
    public void ActivateCarPerspective()
    {

        StartCoroutine(CarPerspective());
    }
    private IEnumerator CarPerspective()
    {
        if (!inPerspectView)
        {
            inPerspectView = true;
            foreach (var item in cochesIA)
            {
                yield return StartCoroutine(CarPerspectiveCameras(item));
            }
            inPerspectView = false;
        }

    }
    private IEnumerator CarPerspectiveCameras(GameObject item)
    {
        item.GetComponentInChildren<CinemachineVirtualCamera>().enabled = true;
        yield return new WaitForSeconds(3.5f);
        item.GetComponentInChildren<CinemachineVirtualCamera>().enabled = false;

    }
    public void SetPlayer(Transform t)
    {
        virtualCamera.LookAt = t;
        virtualCamera.Follow = t;
    }
    private void Update()
    {
        if (carController != null && !dialogueSystem.dialoguePanel.activeSelf)
        {
            timer += Time.deltaTime;

            if (timer >= trackingInterval) // Comprueba si ha pasado el intervalo
            {
                if (carController != null)
                {
                    //Obtener la posición del coche
                    Vector3 carPosition = carController.transform.position;

                    //Enviar la posición al tracker
                    GameObjectTracker.Instance.Interacted($"player-position/{carPosition.x}-{carPosition.y}-{carPosition.z}");
                }

                // Reinicia el temporizador
                timer = 0f;
            }
            velText.text = carController.GetComponent<PrometeoCarController>().carSpeed.ToString("F0");
        }
        else
        {
            //momentaneio

        }
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
            Debug.Log("No se encontró NivelLoader en la escena.");
        }
    }
    public void ComprobarNivel()
    {
        finDeNivel = true;
        if (incorrectLevel.Count > 1)
        {
            CompletableTracker.Instance.Completed("nivel" + currentLevel.ToString()).WithSuccess(false);

            dialogueSystem.ShowIncorrectLevelDialog(incorrectLevel.ToArray());
            Debug.Log("¡Nivel incorrecto!");
            foreach (string nivel in incorrectLevel)
            {
                Debug.Log(nivel);
            }
        }
        else
        {
            CompletableTracker.Instance.Completed("nivel" + currentLevel.ToString()).WithSuccess(true);
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
    public void EnableRain()
    {
        //Debug.Log("lluvia");
        //rain.SetActive(true);
        GameObject instantiatedRain = Instantiate(rain);

        // Configurar como hijo del jugador
        instantiatedRain.transform.SetParent(carController.transform);

        // Opcional: Ajustar la posición y rotación
        instantiatedRain.transform.localPosition = new Vector3(0, 60, 0); // Centrado en el jugador
        instantiatedRain.transform.localRotation = Quaternion.identity;
        RenderSettings.ambientLight = Color.gray; // Cambia la luz ambiental a gris
        ChangeSkybox();
        instantiatedRain.SetActive(true);
    }
    public void SetNight()
    {
        //RenderSettings.ambientLight = Color.gray; // Cambia la luz ambiental a gris
        ChangeSkybox();
        directionalLight.intensity = 0.5f;


        directionalLight.color = Color.black;
    }
    public void SetDeslumbramiento()
    {
        foreach (var item in cochesIA)
        {
            item.GetComponent<OtherCar>().Deslumbramiento?.gameObject.SetActive(true);
        }
    }

    void ChangeSkybox()
    {
        if (graySkybox != null)
        {
            RenderSettings.skybox = graySkybox; // Cambia el Skybox
            DynamicGI.UpdateEnvironment(); // Actualiza la iluminación global
        }
    }

    public void ChangeScene(string sceneName)
    {
        if (sceneName == "Menu")
            SceneData.JsonFileName = "menu.json";
        SceneManager.LoadScene(sceneName);
    }
    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void SetCurrentLevel(int nivel)
    {
        currentLevel = nivel;
    }
    public async void Finalized()
    {

        await CompletableTracker.Instance.Progressed("Juego", CompletableTracker.CompletableType.Game, 1f);
        await CompletableTracker.Instance.Completed("Juego", CompletableTracker.CompletableType.Game);
        var wants = Simva.SimvaPlugin.Instance.WantsToQuit();
        if (wants)
        {
            if (Application.isEditor)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            else
            {
                Application.Quit();
            }
        }

    }
}

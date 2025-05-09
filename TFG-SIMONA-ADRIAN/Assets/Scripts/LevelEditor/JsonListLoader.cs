using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JsonListLoader : MonoBehaviour
{
    public Transform contentParent; // El parent de los botones (ScrollView Content)
    public GameObject buttonPrefab; // Prefab de botón con un Text hijo
    public GameObject button; // Prefab de botón con un Text hijo

    void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Editor");
        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);

            if (transform.GetChild(0).gameObject.activeSelf)
            {
                if (Directory.Exists(path))
                {
                    string[] files = Directory.GetFiles(path, "*.json");

                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileName(file);
                        string fileName2 = Path.GetFileNameWithoutExtension(file);

                        Debug.Log(file);
                        Debug.Log(fileName);
                        GameObject newButton = Instantiate(buttonPrefab, contentParent);
                        newButton.GetComponentInChildren<TextMeshProUGUI>().text = fileName2;

                        // Guardamos el nombre para el evento del botón
                        string capturedFileName = fileName;
                        newButton.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            Debug.Log("Has hecho clic en: " + capturedFileName);
                            PlayLevel(capturedFileName);
                        });
                    }
                }
                else
                {
                    Debug.LogError("No se encuentra la carpeta: " + path);
                }

            }
        });

    }

    void PlayLevel(string a)
    {
        SceneData.JsonFileName = a;
        SceneManager.LoadScene("Game");
    }
}

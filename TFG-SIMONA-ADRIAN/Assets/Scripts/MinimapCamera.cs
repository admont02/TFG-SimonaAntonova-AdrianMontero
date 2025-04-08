using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform player;
    public Transform target;
    public Camera minimapCamera;
    public float minZoom = 5f;
    public float maxZoom = 20f;
    public float initialMaxZoom = 200f;
    public float zoomSpeed = 5f;
    public float followSpeed = 5f;
    public float minDistance = 20f;
    public float maxDistance = 100f;
    public GameObject smallMap;
    public GameObject bigMap;

    Vector3 positionMap;

    private float currentZoom;

    void Start()
    {
        smallMap.SetActive(true);
        bigMap.SetActive(false);
        // Inicializar el zoom actual al mínimo
        currentZoom = maxZoom;
        player = GameManager.Instance.carController.transform;
        target = GameManager.Instance.playerTarget.transform;

        positionMap = new Vector3((GameManager.Instance.columnas * GameManager.Instance.scale) / 2, minimapCamera.transform.position.y, (GameManager.Instance.filas * GameManager.Instance.scale) / 2);
    }

    void Update()
    {

        if (!bigMap.activeSelf)
        {

            // Seguir al jugador
            FollowPlayer();
        }

        // Ajustar el zoom según la distancia entre el jugador y el objetivo
        AdjustZoom();


        // Actualizar la cámara del minimapa
        UpdateMinimapCameraPosition();

        if (Input.GetKeyDown(KeyCode.M))
        {
            smallMap.SetActive(!smallMap.activeSelf);
            bigMap.SetActive(!bigMap.activeSelf);
            if (smallMap.activeSelf)
            {
                maxZoom = currentZoom;
            }
            if (bigMap.activeSelf)
            {
                maxZoom = (GameManager.Instance.scale * 5) * GameManager.Instance.columnas / 10;
                minimapCamera.transform.position = positionMap;
                //minimapCamera.orthographicSize = currentZoom;
            }
        }
    }
    public void ActivateBigMap()
    {
        smallMap.SetActive(false);
        bigMap.SetActive(true);

        maxZoom = (GameManager.Instance.scale * 5) * GameManager.Instance.columnas / 10;
        minimapCamera.transform.position = positionMap;

    }
    public void ActivateSmallMap()
    {
        smallMap.SetActive(true);
        bigMap.SetActive(false);
        maxZoom = currentZoom;

    }

    // Método para seguir al jugador
    private void FollowPlayer()
    {
        Vector3 desiredPosition = new Vector3(player.position.x, minimapCamera.transform.position.y, player.position.z);
        minimapCamera.transform.position = desiredPosition;
        //minimapCamera.transform.position = Vector3.Lerp(minimapCamera.transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }

    // Ajustar el zoom según la distancia entre el jugador y el objetivo
    private void AdjustZoom()
    {
        float distanceToTarget = Vector3.Distance(player.position, target.position);

        // Normalizar la distancia entre la distancia mínima y máxima
        float zoomFactor = Mathf.InverseLerp(minDistance, maxDistance, distanceToTarget);

        // Calcular el nuevo zoom en función de la distancia
        currentZoom = Mathf.Lerp(minZoom, maxZoom, zoomFactor);

        // Ajustar el zoom de la cámara
        minimapCamera.orthographicSize = currentZoom;
    }

    // Actualizar la posición de la cámara del minimapa
    private void UpdateMinimapCameraPosition()
    {
        // Aquí puedes hacer ajustes adicionales como un "recentrado" de la cámara o zoom específico.
        // Por ejemplo, limitar el rango de movimiento si es necesario.
    }
}

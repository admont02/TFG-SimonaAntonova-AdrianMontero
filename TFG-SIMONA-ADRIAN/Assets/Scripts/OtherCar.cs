using System.Collections;

using UnityEngine;
/// <summary>
/// Clase que gestiona el comportamiento general del coche en combinacion con WaypointNavigator
/// </summary>
public class OtherCar : MonoBehaviour
{

    public bool reachedDestination = false;
    public Vector3 destination;
    public float stopDistance = 5.5f;
    public float movementSpeed = 1;
    public float rotationSpeed = 120;
    private Vector3 lastPosition;
    private Vector3 velocity;
    public bool isStopped = false;
    public float brakeDistance = 5f; //distancia a la que el coche empieza a frenar
    public bool isCarInFront = false;
    public GameObject arrow;
    public GameObject LeftLight;
    public GameObject RightLight;
    public GameObject LeftLightFront;
    public GameObject RightLightFront;
    public int branchTo { get; set; }
    public string orientacion { get; set; }
    public bool clickMove { get; set; }
    public int carID { get; internal set; }
    public GameObject icon;

    public GameObject bodyMain;
    public GameObject seams;
    public GameObject Deslumbramiento;

    public Material[] carColors;
    public Material[] seamColors;
    public bool destroyedByTrash = false;

    private void Awake()
    {
        //Color
        if (carColors.Length > 0 && bodyMain != null)
        {
            int randomIndex = Random.Range(0, carColors.Length);
            bodyMain.GetComponent<Renderer>().material = carColors[randomIndex];
        }


        if (seamColors.Length > 0 && seams != null)
        {
            int randomIndex = Random.Range(0, seamColors.Length);
            seams.GetComponent<Renderer>().material = seamColors[randomIndex];
        }
    }


    /// <summary>
    /// Efecto de parpadeo en intermitentes
    /// </summary>
    /// <param name="luz"></param>
    /// <returns></returns>
    private IEnumerator Intermitente(Light luz)
    {
        while (true)
        {
            luz.enabled = !luz.enabled; // Alterna el estado de la luz
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void Start()
    {
        //Si es nivel de clic
        if (ClicLevelManager.Instance != null)
        {
            if (isCarInFront)
                movementSpeed = 0;
            clickMove = false;
            //arrow.SetActive(true);
            Vector3 localArrowRotation = Vector3.zero;
            //establecer en que direccion se mueve el vehiculo
            switch (branchTo)
            {
                case 1: // Norte
                    Debug.Log("TENGO ORIENTACION: " + orientacion + " Y BRANCH TO " + branchTo);
                    localArrowRotation = new Vector3(-90, 0, -90); // Hacia adelante
                    break;
                case 2: // Oeste
                    StartCoroutine(Intermitente(LeftLight.GetComponent<Light>()));
                    StartCoroutine(Intermitente(LeftLightFront.GetComponent<Light>()));
                    localArrowRotation = new Vector3(-90, 0, 180); // Hacia la izqda
                    break;
                case 0: // Este
                    StartCoroutine(Intermitente(RightLight.GetComponent<Light>()));
                    StartCoroutine(Intermitente(RightLightFront.GetComponent<Light>()));
                    localArrowRotation = new Vector3(-90, -90, 0); // Hacia la dcha
                    break;
                case 4: // Sur
                    localArrowRotation = new Vector3(-90, 180, 0); // Hacia atr�s
                    break;
            }

            arrow.transform.localRotation = Quaternion.Euler(localArrowRotation);
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (isStopped || !GameManager.Instance.canCarMove && !ClicLevelManager.Instance || ClicLevelManager.Instance && !clickMove) return;
        if (transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;
            float destinationDistance = destinationDirection.magnitude;
            if (destinationDistance >= stopDistance)
            {
                if (isCarInFront)
                {
                    //Reduce la velocidad si est� demasiado cerca de otro coche
                    movementSpeed = 0;
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                }
                else
                {
                    movementSpeed = 15f; //Restablecer velocidad normal
                }
                reachedDestination = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
            }

            else reachedDestination = true;

            velocity = (transform.position - lastPosition) / Time.deltaTime;
            velocity.y = 0;
            var velocityMagnitude = velocity.magnitude;
            velocity = velocity.normalized;
            var fwdDotProduct = Vector3.Dot(transform.forward, velocity);
            var rightDotProduct = Vector3.Dot(transform.right, velocity);
        }

    }
    /// <summary>
    /// Establece el prox destino del vehiculo
    /// </summary>
    /// <param name="des"></param>
    public void MoveToDestinations(Vector3 des)
    {
        destination = des;
        reachedDestination = false;
    }
    /// <summary>
    /// Detiene el vehiculo
    /// </summary>
    public void StopCar()
    {
        isStopped = true;
    }
    /// <summary>
    /// Reanuda el vehiculo
    /// </summary>
    public void ResumeCar()
    {
        isStopped = false;
    }
    /// <summary>
    /// Comrpueba la conexion de 2 piezas en el grafo
    /// </summary>
    /// <param name="currentId"></param>
    /// <param name="nextId"></param>
    /// <returns></returns>
    public bool IsConnected(int currentId, int nextId)
    {
        var ady = GameManager.Instance.graph.getAdy(currentId);
        return ady.Contains(nextId);
    }

    void OnMouseDown()
    {
        Debug.Log(this.name);
        //coche clicado
        if (ClicLevelManager.Instance != null && !ClicLevelManager.Instance.checking)
            ClicLevelManager.Instance.CarClicked(gameObject);

    }

}
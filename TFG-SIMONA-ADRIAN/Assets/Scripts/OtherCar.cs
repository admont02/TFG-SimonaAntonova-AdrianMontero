using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class OtherCar : MonoBehaviour
{

    public bool reachedDestination = false;
    public Vector3 destination;
    public float stopDistance = 2.5f;
    public float movementSpeed = 1;
    public float rotationSpeed = 120;
    private Vector3 lastPosition;
    private Vector3 velocity;
    public bool isStopped = false;
    public float brakeDistance = 5f;         // Distancia a la que el coche empieza a frenar
    private bool isCarInFront = false;
    public GameObject arrow;
    public int branchTo { get; set; }
    public bool clickMove { get; set; }
    public int carID { get; internal set; }

    private void Start()
    {

        if (ClicLevelManager.Instance != null)
        {
            clickMove = false;
            arrow.SetActive(true);
            Vector3 localArrowRotation = Vector3.zero;

            switch (branchTo)
            {
                case 1: // Norte
                    localArrowRotation = new Vector3(-90, 0, -90); // Hacia adelante
                    break;
                case 2: // Este
                    localArrowRotation = new Vector3(-90, 0, 180); // Hacia la derecha
                    break;
                case 3: // Oeste
                    localArrowRotation = new Vector3(-90, -90, 0); // Hacia la izquierda
                    break;
                case 4: // Sur
                    localArrowRotation = new Vector3(-90, 180, 0); // Hacia atrás
                    break;
            }

            arrow.transform.localRotation = Quaternion.Euler(localArrowRotation);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //hay que cambiar segunda variable 
        if (isStopped || !GameManager.Instance.canCarMove && !ClicLevelManager.Instance|| ClicLevelManager.Instance && !clickMove) return;
        if (transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;
            float destinationDistance = destinationDirection.magnitude;
            if (destinationDistance >= stopDistance)
            {
                if (isCarInFront)
                {
                    // Reduce la velocidad si el jugador está demasiado cerca
                    movementSpeed = Mathf.Lerp(movementSpeed, 0, Time.deltaTime * 2f); // Frenado suave
                }
                else
                {
                    movementSpeed = 15f; // Restablecer velocidad normal
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
    public void MoveToDestinations(Vector3 des)
    {
        destination = des;
        reachedDestination = false;
    }
    public void StopCar()
    {
        isStopped = true;
    }

    public void ResumeCar()
    {
        isStopped = false;
    }
    public bool IsConnected(int currentId, int nextId)
    {
        var ady = GameManager.Instance.graph.getAdy(currentId);
        return ady.Contains(nextId);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3|| other.gameObject.GetComponent<OtherCar>()) //Si detecta al jugador/otro coche
        {
            isCarInFront = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3||other.gameObject.GetComponent<OtherCar>()) //Cuando el jugador/otro coche salga del trigger
        {
            isCarInFront = false;
        }
    }
    void OnMouseDown()
    { // Cuando se toca este coche, pasar el orden al GameController gameController.CarTouched(carOrder); }
        Debug.Log(this.name);
        ////gameObject.GetComponentInChildren<MeshCollider>().gameObject.GetComponent<MeshRenderer>().material = outline;
        //if (!GameManager.Instance.priorityCarList.Contains(gameObject))
        //    GameManager.Instance.priorityCarList.Add(gameObject);
        //else
        //    GameManager.Instance.priorityCarList.Remove(gameObject);

        //Destroy(this.gameObject);
        if (ClicLevelManager.Instance != null)
            ClicLevelManager.Instance.CarClicked(gameObject);

    }
    //public void SetBranchTo(int b)
    //{
    //    branchTo = b;
    //}
}
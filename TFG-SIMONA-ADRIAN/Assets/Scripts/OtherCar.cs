using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class OtherCar : MonoBehaviour
{

    public bool reachedDestination=false;
    public Vector3 destination;
    public float stopDistance = 2.5f;
    public float movementSpeed = 1;
    public float rotationSpeed = 120;
    private Vector3 lastPosition;
    private Vector3 velocity;
    public bool isStopped = false;
    public float brakeDistance = 5f;         // Distancia a la que el coche empieza a frenar
    private bool isPlayerInFront = false;
    // Update is called once per frame
    void Update()
    {
        if(isStopped|| !GameManager.Instance.canCarMove) return;
        if (transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;
            float destinationDistance = destinationDirection.magnitude;
            if (destinationDistance >= stopDistance)
            {
                if (isPlayerInFront)
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
        if (other.gameObject.layer == 3) // Si detecta al jugador
        {
            isPlayerInFront=true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3) // Cuando el jugador salga del trigger
        {
            isPlayerInFront=false;
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
}
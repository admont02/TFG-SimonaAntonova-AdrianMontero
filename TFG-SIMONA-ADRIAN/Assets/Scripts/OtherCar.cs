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
}
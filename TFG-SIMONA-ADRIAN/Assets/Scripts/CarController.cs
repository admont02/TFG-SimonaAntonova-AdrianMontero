using UnityEngine;

public class CarController : MonoBehaviour
{
    public float motorTorque = 2000f;
    public float brakeTorque = 2000f;
    public float maxSpeed = 35f;
    public float steeringRange = 50f; 
    public float steeringRangeAtMaxSpeed = 25f; 
    public float centreOfGravityOffset = -1f;

    private WheelControl[] wheels;
    private Rigidbody rigidBody;
    private bool hasMoved = false;

    public float steeringSmoothness = 0.2f;  
    private float previousSteerAngle = 0f;

    
    public bool enableSteeringAssist = true;
    public bool enableSpeedControl = true;
    public float assistSteeringFactor = 0.5f; 
    public float maxAssistSpeed = 10f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;
        wheels = GetComponentsInChildren<WheelControl>();
    }

    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.canCarMove)
        {
            StopCar();
            return;
        }

        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");
        bool isBraking = Input.GetKey(KeyCode.Space);

        if (vInput != 0 || hInput != 0 || isBraking)
        {
            hasMoved = true;
        }

        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.velocity);
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        float targetSteerAngle = hInput * currentSteerRange;
        float smoothSteerAngle = Mathf.LerpAngle(previousSteerAngle, targetSteerAngle, steeringSmoothness);
        previousSteerAngle = smoothSteerAngle;

        foreach (var wheel in wheels)
        {
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = smoothSteerAngle;
            }

            if (isBraking)
            {
                wheel.WheelCollider.brakeTorque = brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
            else
            {
                wheel.WheelCollider.brakeTorque = 0;

                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                }
            }
        }

        
        if (enableSteeringAssist && hInput != 0)
        {
            Vector3 assistDirection = Vector3.Lerp(transform.forward, transform.right * hInput, assistSteeringFactor);
            rigidBody.AddForce(assistDirection * motorTorque * 0.1f);
        }

        
        if (enableSpeedControl && forwardSpeed > maxAssistSpeed)
        {
            rigidBody.AddForce(-transform.forward * motorTorque * 0.5f);
        }

       
        if (hInput != 0)
        {
            Vector3 rotationForce = transform.up * hInput * currentSteerRange * 0.1f;
            rigidBody.AddTorque(rotationForce, ForceMode.Force);
        }
    }

    public bool HasMoved()
    {
        return hasMoved;
    }

    private void StopCar()
    {
        foreach (var wheel in wheels)
        {
            wheel.WheelCollider.brakeTorque = brakeTorque;
            wheel.WheelCollider.motorTorque = 0;
        }

        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
    }
}

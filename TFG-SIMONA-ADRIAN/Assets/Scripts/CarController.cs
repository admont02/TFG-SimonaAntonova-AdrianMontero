using UnityEngine;

public class CarController : MonoBehaviour
{
    public float motorTorque = 4000f;
    public float brakeTorque = 2000f;
    public float maxSpeed = 35f;
    public float steeringRange = 70f;
    public float steeringRangeAtMaxSpeed = 40f;
    public float centreOfGravityOffset = -1f;

    private WheelControl[] wheels;
    private Rigidbody rigidBody;
    private bool hasMoved = false;

    public float steeringSmoothness = 0.05f;
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
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, Mathf.Abs(forwardSpeed));
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Calcular el ángulo de giro suavizado
        float targetSteerAngle = hInput * currentSteerRange;
        float smoothSteerAngle = Mathf.LerpAngle(previousSteerAngle, targetSteerAngle, steeringSmoothness);
        previousSteerAngle = smoothSteerAngle;

        foreach (var wheel in wheels)
        {
            if (wheel.steerable)
            {
                // Aplicar giro independientemente del movimiento hacia adelante
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

        // Asistencia de dirección para giros más suaves
        if (enableSteeringAssist && hInput != 0)
        {
            Vector3 assistDirection = Vector3.Lerp(transform.forward, transform.right * hInput, assistSteeringFactor);
            rigidBody.AddForce(assistDirection * motorTorque * 0.1f);
        }

        // Control de velocidad máxima
        if (enableSpeedControl && forwardSpeed > maxAssistSpeed)
        {
            rigidBody.AddForce(-transform.forward * motorTorque * 0.5f);
        }

        // Aplicar un torque de rotación para facilitar el giro
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

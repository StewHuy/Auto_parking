using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;  // For file operations
using System.Text;  // For StringBuilder

[RequireComponent(typeof(InputTest))]
[RequireComponent(typeof(Rigidbody))]
public class CarControlSpeed : MonoBehaviour
{
    public InputTest im;
    public List<WheelCollider> throttleWheels;
    public List<GameObject> steeringWheels;
    public List<GameObject> meshes;
    public float strengthCoeffient = 10000f;
    public float maxTurn = 25f;
    public Transform CM;
    public Rigidbody rb;
    public float brakeStrength;
    private string filePath;  // Path to the CSV file
    private StringBuilder csvContent;  // For efficient logging
    private float currentSteerAngle;  // Variable to store the current steering angle
    public float steerSpeed = 5f;  // Speed at which the steering angle changes

    // Start is called before the first frame update
    void Start()
    {
        im = GetComponent<InputTest>();
        rb = GetComponent<Rigidbody>();
        if (CM)
        {
            rb.centerOfMass = CM.position;
        }

        // Set the file path and create the file with headers
        filePath = Application.dataPath + "/SteeringAngles.csv";
        CreateCSVFile();
        csvContent = new StringBuilder();
        currentSteerAngle = 0f;  // Initialize the current steering angle
    }

    // Create the CSV file and write headers
    void CreateCSVFile()
    {
        if (!File.Exists(filePath))
        {
            string header = "Time,SteeringAngle,PositionX,PositionZ\n";
            File.WriteAllText(filePath, header);
        }
    }

    // Append a new line to the CSV file
    void WriteToCSV(string content)
    {
        File.AppendAllText(filePath, content);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float targetSteerAngle = maxTurn * im.steer;
        currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteerAngle, steerSpeed * Time.deltaTime);

        foreach (WheelCollider wheel in throttleWheels)
        {
            if (im.brake)
            {
                wheel.motorTorque = 0f;
                wheel.brakeTorque = brakeStrength;
            }
            else
            {
                wheel.motorTorque = strengthCoeffient * im.throttle * Time.deltaTime;
                wheel.brakeTorque = 0f;
            }
        }

        foreach (GameObject wheel in steeringWheels)
        {
            WheelCollider wheelCollider = wheel.GetComponent<WheelCollider>();
            wheelCollider.steerAngle = currentSteerAngle;
            wheel.transform.localEulerAngles = new Vector3(0f, wheelCollider.steerAngle, 0f);
        }

        foreach (GameObject mesh in meshes)
        {
            mesh.transform.Rotate(rb.velocity.magnitude * (transform.InverseTransformDirection(rb.velocity).z >= 0 ? 1 : -1) / (2 * Mathf.PI * 0.33f), 0f, 0f);
        }

        // Log the steering angle and position
        string logEntry = Time.time + "," + currentSteerAngle + "," + transform.position.x + "," + transform.position.z + "\n";
        csvContent.Append(logEntry);
    }

    // LateUpdate is called once per frame, after all FixedUpdate calls
    void LateUpdate()
    {
        // Write to CSV in LateUpdate to ensure it's done after physics updates
        if (csvContent.Length > 0)
        {
            WriteToCSV(csvContent.ToString());
            csvContent.Clear();
        }
    }
}

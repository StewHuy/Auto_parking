using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
[RequireComponent(typeof(InputTest))]
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public InputTest im;
    public List<WheelCollider> throttleWheels;
    public List<GameObject> steeringWheels;
    public List<GameObject> meshes;
    public float strengthCoeffient=10000f;
    public float maxTurn = 25f;
    public Transform CM;
    public Rigidbody rb;
    public float brakeStrength;

    // Start is called before the first frame update
    void Start()
    {
        im = GetComponent<InputTest>();
        rb= GetComponent<Rigidbody>();
        if (CM)
        {
            rb.centerOfMass = CM.position;
        }

    }

    // Update is called once per frame
    void FixedUpdate()    {
        foreach(WheelCollider wheel in throttleWheels)
        {
            if (im.brake)
            {
                wheel.motorTorque = 0f;
                wheel.brakeTorque = brakeStrength * Time.deltaTime;
            }else
            {
                wheel.motorTorque = strengthCoeffient * Time.deltaTime * im.throttle;
                wheel.brakeTorque = 0f;
            }
        }
        foreach (GameObject wheel in steeringWheels)
        {
            wheel.GetComponent<WheelCollider>().steerAngle = maxTurn*im.steer;
            wheel.transform.localEulerAngles = new Vector3(0f,im.steer*maxTurn,0f);
        }
        foreach (GameObject mesh in meshes)
        {
            mesh.transform.Rotate(rb.velocity.magnitude * (transform.InverseTransformDirection(rb.velocity).z>= 0 ? 1 : -1) /(2*Mathf.PI*0.33f),0f,0f);//tornary ogeratir - 1 > 0 > "?" : "!"
        }
    }
}
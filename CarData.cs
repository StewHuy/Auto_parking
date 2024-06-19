using UnityEngine;

public class CarData : MonoBehaviour
{
    // Assign these in the Unity Editor
    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public Transform rearLeftWheel;
    public Transform rearRightWheel;
    public float wheelbase;
    private void Start()
    {
        if (frontLeftWheel != null && rearLeftWheel != null && frontRightWheel != null && rearRightWheel != null)
        {
            // Calculate the midpoint of the front and rear axles
            Vector3 frontAxleMidpoint = (frontLeftWheel.position + frontRightWheel.position) / 2;
            Vector3 rearAxleMidpoint = (rearLeftWheel.position + rearRightWheel.position) / 2;

            // Calculate the wheelbase as the distance between the midpoints
            wheelbase = Vector3.Distance(frontAxleMidpoint, rearAxleMidpoint);

            // Output the wheelbase
            Debug.Log("Wheelbase: " + wheelbase + " units");
        }
        else
        {
            Debug.LogError("Please assign all four wheel transforms in the Unity Editor.");
        }
    }
}

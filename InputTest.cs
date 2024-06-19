using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    public float throttle;
    public float steer;
    public bool brake, Type1 = false, Type2 = false, Type3 = false;
    public GameObject parking_space_per, parking_space_par;
    public float currentX;
    public float currentZ;
    private static readonly float[] xValues = { 0.70f, 1.12f, 1.51f, 1.91f, 2.62f, 3.12f, 3.67f, 4.25f, 4.84f, 5.34f, 5.74f, 6.08f, 6.39f, 6.72f, 7.05f, 7.4f, 7.67f, 7.98f, 8.33f, 8.60f };
    private static readonly float[] zValues = { 0.05f, 0.10f, 0.15f, 0.22f, 0.37f, 0.52f, 0.71f, 0.92f, 1.23f, 1.57f, 1.78f, 2.00f, 2.22f, 2.47f, 2.75f, 3.02f, 3.36f, 3.64f, 4.00f, 4.38f };
    private static readonly float[] tValues = { 1.0f, 1.3f, 1.5f, 1.7f, 2.0f, 2.2f, 2.4f, 2.6f, 2.8f, 3.0f, 3.1f, 3.2f, 3.3f, 3.4f, 3.5f, 3.6f, 3.7f, 3.8f, 3.9f, 4.0f };
    public float x_target, z_target;
    public float xp, zp;
    private Coroutine currentCoroutine = null;

    void Start()
    {
        if (parking_space_per == null || parking_space_par == null)
        {
            Debug.LogError("Parking space is not assigned!");
            return;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(ParkingPerpendicular());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(ParkingParallel());
        }
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    StartCoroutine(ParkingPerpendicular());
        //}
        // You can keep your original input reading here if needed
        //throttle = Input.GetAxis("Vertical");
        //steer = Input.GetAxis("Horizontal");
        //brake = Input.GetKey(KeyCode.Space);
    }

    IEnumerator ParkingPerpendicular()
    {
        Vector3 position = transform.position;
        Vector3 p_position = parking_space_per.transform.position;
        xp = p_position.x;
        zp = p_position.z;
        currentX = position.x;
        currentZ = position.z;
        z_target = -2.38f;
        x_target = p_position.x + 2f - 0.12f;
        float t;
        t = Interpolate(Math.Abs(z_target - currentZ), zValues, tValues);
        throttle = 1.0f;
        steer = -1f;

        yield return new WaitForSeconds(t);
        steer = 1f;
        yield return new WaitForSeconds(t / 2 - 0.035f);
        steer = 0f;
        position = transform.position;
        currentX = position.x;
        if (currentX > x_target)
        {
            brake = true;
            yield return new WaitForSeconds(1);
            brake = false;
            throttle = -1.0f;
            while (currentX > x_target)
            {
                position = transform.position;
                currentX = position.x;
                currentZ = position.z;

                // Optional: You can add a small delay here if necessary
                yield return null;
            }
        }
        else
        {
            while (currentX < x_target)
            {
                position = transform.position;
                currentX = position.x;
                currentZ = position.z;

                // Optional: You can add a small delay here if necessary
                yield return null;
            }
        }
        // Apply brakes
        brake = true;
        throttle = 0.0f;
        steer = 0.0f;
        yield return new WaitForSeconds(1f);

        brake = false;
        throttle = 1.0f;
        steer = 1f;
        yield return new WaitForSeconds(3f);

        brake = true;
        throttle = 0.0f;
        steer = 0.0f;
        yield return new WaitForSeconds(1f);

        // Reverse to parking-space
        brake = false;
        throttle = -1.0f;
        steer = -1f;
        yield return new WaitForSeconds(4.5f);

        steer = 0f;
        yield return new WaitForSeconds(0.5f);

        brake = true;
        throttle = 0.0f;
        steer = 0.0f;
    }

    IEnumerator ParkingParallel()
    {
        Vector3 position = transform.position;
        Vector3 p_position = parking_space_par.transform.position;
        xp = p_position.x;
        zp = p_position.z;
        currentX = position.x;
        currentZ = position.z;
        z_target = -27.5f;
        x_target = p_position.x + 5.8f;
        float t;
        t = Interpolate(Math.Abs(z_target - currentZ), zValues, tValues);
        throttle = 1.0f;
        steer = -1f;

        yield return new WaitForSeconds(t);
        steer = 1f;
        yield return new WaitForSeconds(t / 2 - 0.035f);
        steer = 0f;
        position = transform.position;
        currentX = position.x;
        if (currentX > x_target)
        {
            brake = true;
            yield return new WaitForSeconds(1);
            brake = false;
            throttle = -1.0f;
            while (currentX > x_target)
            {
                position = transform.position;
                currentX = position.x;
                currentZ = position.z;

                // Optional: You can add a small delay here if necessary
                yield return null;
            }
        }
        else
        {
            while (currentX < x_target)
            {
                position = transform.position;
                currentX = position.x;
                currentZ = position.z;

                // Optional: You can add a small delay here if necessary
                yield return null;
            }
        }
        // Apply brakes
        brake = true;
        throttle = 0.0f;
        steer = 0.0f;
        yield return new WaitForSeconds(1f);

        brake = false;
        throttle = -1.0f;
        steer = -1f;
        yield return new WaitForSeconds(4f);
        brake = true;
        throttle = 0.0f;
        steer = 0.0f;
        yield return new WaitForSeconds(1f);

        // Reverse to parking-space
        brake = false;
        throttle = -1.0f;
        steer = 1f;
        yield return new WaitForSeconds(3.5f);
        brake = true;
        throttle = 0f;
        steer = 0f;
        yield return new WaitForSeconds(1f);
        brake = false;
        throttle = 1f;
        steer = -1f;
        yield return new WaitForSeconds(2f);
        brake = true;
        throttle = 0f;
        steer = 0f;
    }

    private static float Interpolate(float z, float[] zData, float[] yData)
    {
        int size = zData.Length;

        if (z <= zData[0]) return yData[0];
        if (z >= zData[size - 1]) return yData[size - 1];

        int i = 0;
        while (z > zData[i + 1]) i++;

        float z0 = zData[i], z1 = zData[i + 1];
        float y0 = yData[i], y1 = yData[i + 1];

        return y0 + (z - z0) * (y1 - y0) / (z1 - z0);
    }
}

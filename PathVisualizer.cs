using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVisualizer : MonoBehaviour
{
    public GameObject car; // Reference to the car GameObject
    public float recordInterval = 0.2f; // Interval in seconds between each recorded position
    public float squareInterval = 2f; // Distance interval between each recorded square
    public Color pathColor = Color.green; // Color of the path lines
    public Color squareColor = Color.red; // Color of the squares

    private LineRenderer pathLineRenderer;
    private List<Vector3> pathPositions = new List<Vector3>();
    private float timer = 0f;
    private Vector3 lastPos;

    private List<Rectangle> rectangles = new List<Rectangle>();

    void Start()
    {
        pathLineRenderer = gameObject.AddComponent<LineRenderer>();
        pathLineRenderer.startWidth = 0.2f;
        pathLineRenderer.endWidth = 0.2f;
        pathLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        pathLineRenderer.startColor = pathColor;
        pathLineRenderer.endColor = pathColor;

        lastPos = car.transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= recordInterval)
        {
            RecordPosition();
            timer = 0f;
        }

        if ((car.transform.position - lastPos).sqrMagnitude > squareInterval * squareInterval)
        {
            AddSquare();
            lastPos = car.transform.position;
        }

        UpdatePathVisualization();
    }

    void RecordPosition()
    {
        if (car != null)
        {
            pathPositions.Add(car.transform.position);
        }
    }

    void UpdatePathVisualization()
    {
        pathLineRenderer.positionCount = pathPositions.Count;
        pathLineRenderer.SetPositions(pathPositions.ToArray());
    }

    private void AddSquare()
    {
        Transform carTrans = car.transform;
        float carWidth = 2f; // Adjust these values based on your car's dimensions
        float carLength = 4f; // Adjust these values based on your car's dimensions

        Vector3 F = carTrans.position + carTrans.forward * (carLength / 2);
        Vector3 B = carTrans.position - carTrans.forward * (carLength / 2);
        Vector3 center = (F + B) * 0.5f;

        float heading = carTrans.eulerAngles.y * Mathf.Deg2Rad;

        Rectangle rect = GetCornerPositions(center, heading, carWidth, carLength);

        rectangles.Add(rect);
        DrawRectangle(rect);
    }

    private Rectangle GetCornerPositions(Vector3 center, float heading, float width, float length)
    {
        float halfWidth = width / 2;
        float halfLength = length / 2;

        Vector3 FL = center + new Vector3(-halfWidth * Mathf.Cos(heading) - halfLength * Mathf.Sin(heading), 0.5f, halfWidth * Mathf.Sin(heading) - halfLength * Mathf.Cos(heading));
        Vector3 FR = center + new Vector3(halfWidth * Mathf.Cos(heading) - halfLength * Mathf.Sin(heading), 0.5f, -halfWidth * Mathf.Sin(heading) - halfLength * Mathf.Cos(heading));
        Vector3 BL = center + new Vector3(-halfWidth * Mathf.Cos(heading) + halfLength * Mathf.Sin(heading), 0.5f, halfWidth * Mathf.Sin(heading) + halfLength * Mathf.Cos(heading));
        Vector3 BR = center + new Vector3(halfWidth * Mathf.Cos(heading) + halfLength * Mathf.Sin(heading), 0.5f, -halfWidth * Mathf.Sin(heading) + halfLength * Mathf.Cos(heading));

        return new Rectangle(FL, FR, BL, BR);
    }

    private void DrawRectangle(Rectangle rect)
    {
        GameObject rectGO = new GameObject("Rectangle");
        LineRenderer rectLineRenderer = rectGO.AddComponent<LineRenderer>();
        rectLineRenderer.startWidth = 0.1f;
        rectLineRenderer.endWidth = 0.1f;
        rectLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        rectLineRenderer.startColor = squareColor;
        rectLineRenderer.endColor = squareColor;
        rectLineRenderer.positionCount = 5;
        rectLineRenderer.SetPosition(0, rect.FL);
        rectLineRenderer.SetPosition(1, rect.FR);
        rectLineRenderer.SetPosition(2, rect.BR);
        rectLineRenderer.SetPosition(3, rect.BL);
        rectLineRenderer.SetPosition(4, rect.FL);
    }

    public class Rectangle
    {
        public Vector3 FL, FR, BL, BR;

        public Rectangle(Vector3 fl, Vector3 fr, Vector3 bl, Vector3 br)
        {
            FL = fl;
            FR = fr;
            BL = bl;
            BR = br;
        }
    }
}

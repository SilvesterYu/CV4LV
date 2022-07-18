using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundGet : MonoBehaviour
{
    public GameObject bounder;

    private LineRenderer lineRend;

    // Start is called before the first frame update
    void Start()
    {
        lineRend = GetComponent<LineRenderer>();

        lineRend.startColor = Color.red;
        lineRend.endColor = Color.red;

        // set width of the renderer
        lineRend.startWidth = 0.005f;
        lineRend.endWidth = 0.005f;
        lineRend.SetVertexCount(4);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] vertices = bounder.GetComponent<MeshFilter>().mesh.vertices;

        float
            x1 = float.MaxValue,
            y1 = float.MaxValue,
            x2 = 0.0f,
            y2 = 0.0f;

        foreach (Vector3 vert in vertices)
        {
            Vector2 tmp =
                WorldToGUIPoint(bounder.transform.TransformPoint(vert));

            if (tmp.x < x1) x1 = tmp.x;
            if (tmp.x > x2) x2 = tmp.x;
            if (tmp.y < y1) y1 = tmp.y;
            if (tmp.y > y2) y2 = tmp.y;
        }

        Rect bbox = new Rect(x1, y1, x2 - x1, y2 - y1);

        // Debug.Log (bbox);
        lineRend.SetPosition(0, new Vector2(x1, y1));
        lineRend.SetPosition(1, new Vector2(x1, y2));
        lineRend.SetPosition(2, new Vector2(x2, y1));
        lineRend.SetPosition(3, new Vector2(x2, y2));
        // return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    }

    // public static Rect GUI2dRectWithObject(GameObject go)
    // {
    //     return bbox;
    // }
    public static Vector2 WorldToGUIPoint(Vector3 world)
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(world);
        screenPoint.y = (float) Screen.height - screenPoint.y;
        return screenPoint;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawRect : MonoBehaviour
{
    UnityEngine.Camera cam;

    private GUIStyle currentStyle = null;

    public Texture2D BoxBorder; // Set this to your border texture in the Unity Editor

    public float margin = 0;

    private int confidence = 0;

    private float confidence_float = 0;

    private Vector3[] pts = new Vector3[8];

    public void OnGUI()
    {
        Bounds b = gameObject.GetComponent<Renderer>().bounds;
        Camera cam = Camera.main;

        //The object is behind us
        if (cam.WorldToScreenPoint(b.center).z < 0) return;

        //All 8 vertices of the bounds
        pts[0] =
            cam
                .WorldToScreenPoint(new Vector3(b.center.x + b.extents.x,
                    b.center.y + b.extents.y,
                    b.center.z + b.extents.z));
        pts[1] =
            cam
                .WorldToScreenPoint(new Vector3(b.center.x + b.extents.x,
                    b.center.y + b.extents.y,
                    b.center.z - b.extents.z));
        pts[2] =
            cam
                .WorldToScreenPoint(new Vector3(b.center.x + b.extents.x,
                    b.center.y - b.extents.y,
                    b.center.z + b.extents.z));
        pts[3] =
            cam
                .WorldToScreenPoint(new Vector3(b.center.x + b.extents.x,
                    b.center.y - b.extents.y,
                    b.center.z - b.extents.z));
        pts[4] =
            cam
                .WorldToScreenPoint(new Vector3(b.center.x - b.extents.x,
                    b.center.y + b.extents.y,
                    b.center.z + b.extents.z));
        pts[5] =
            cam
                .WorldToScreenPoint(new Vector3(b.center.x - b.extents.x,
                    b.center.y + b.extents.y,
                    b.center.z - b.extents.z));
        pts[6] =
            cam
                .WorldToScreenPoint(new Vector3(b.center.x - b.extents.x,
                    b.center.y - b.extents.y,
                    b.center.z + b.extents.z));
        pts[7] =
            cam
                .WorldToScreenPoint(new Vector3(b.center.x - b.extents.x,
                    b.center.y - b.extents.y,
                    b.center.z - b.extents.z));

        //Get them in GUI space
        for (int i = 0; i < pts.Length; i++)
        pts[i].y = Screen.height - pts[i].y;

        //Calculate the min and max positions
        Vector3 min = pts[0];
        Vector3 max = pts[0];
        for (int i = 1; i < pts.Length; i++)
        {
            min = Vector3.Min(min, pts[i]);
            max = Vector3.Max(max, pts[i]);
        }

        //Construct a rect of the min and max positions and apply some margin
        Rect r = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        r.xMin += margin;
        r.xMax -= margin;
        r.yMin += margin;
        r.yMax -= margin;

        var borderSize = 2; // Border size in pixels
        var style = new GUIStyle();

        //Initialize RectOffset object
        style.border =
            new RectOffset(borderSize, borderSize, borderSize, borderSize);
        style.normal.background = BoxBorder;
        style.fontSize = 30;
        style.normal.textColor = Color.white;

        confidence = (int) Random.Range(80.0f, 100.0f);
        confidence_float = confidence / 100f;

        //Render the box
        // InitStyles();
        GUI.color = Color.white;
        GUI.Box(r, gameObject.name + " " + confidence_float, style);
    }

    void Start()
    {
        cam = UnityEngine.Camera.main;
    }

    // Object Detection
    void Update()
    {
        // Vector3 viewPos =
        //     cam.WorldToViewportPoint(gameObject.transform.position);
        // if (
        //     viewPos.x >= 0 &&
        //     viewPos.x <= 1 &&
        //     viewPos.y >= 0 &&
        //     viewPos.y <= 1 &&
        //     viewPos.z > 0
        // )
        // {
        //     // Your object is in the range of the camera, you can apply your behaviour
        //     Debug.Log(gameObject.name);
        // }
    }

    // private void InitStyles()
    // {
    //     if (currentStyle == null)
    //     {
    //         currentStyle = new GUIStyle(GUI.skin.box);
    //         currentStyle.normal.background =
    //             MakeTex(2, 2, new Color(0f, 1f, 0f, 0.1f));
    //     }
    // }

    // private Texture2D MakeTex(int width, int height, Color col)
    // {
    //     Color[] pix = new Color[width * height];
    //     for (int i = 0; i < pix.Length; ++i)
    //     {
    //         pix[i] = col;
    //     }
    //     Texture2D result = new Texture2D(width, height);
    //     result.SetPixels (pix);
    //     result.Apply();
    //     return result;
    // }
}

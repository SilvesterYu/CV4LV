using UnityEngine;

public class DrawLineLeft : MonoBehaviour
{
    // Trackers
    public Transform TrackerHandLeft;

    public Transform TrackerHandLeftIndex1;

    public Transform TrackerHandLeftIndex2;

    public Transform TrackerHandLeftIndex3;

    public Transform TrackerHandLeftMiddle1;

    public Transform TrackerHandLeftMiddle2;

    public Transform TrackerHandLeftMiddle3;

    public Transform TrackerHandLeftRing1;

    public Transform TrackerHandLeftRing2;

    public Transform TrackerHandLeftRing3;

    public Transform TrackerHandLeftPinky1;

    public Transform TrackerHandLeftPinky2;

    public Transform TrackerHandLeftPinky3;

    public Transform TrackerHandLeftThumb1;

    public Transform TrackerHandLeftThumb2;

    public Transform TrackerHandLeftThumb3;

    // Apply these values in the editor
    public LineRenderer LineRendererPalm;

    public LineRenderer LineRendererIndex;

    public LineRenderer LineRendererMiddle;

    public LineRenderer LineRendererRing;

    public LineRenderer LineRendererThumb;

    public LineRenderer LineRendererPinky;

    void Start()
    {
        // set the color of the line
        LineRendererPalm.startColor = Color.green;
        LineRendererPalm.endColor = Color.green;

        // set width of the renderer
        LineRendererPalm.startWidth = 0.005f;
        LineRendererPalm.endWidth = 0.005f;

        LineRendererPalm.SetVertexCount(7);

        // set the color of the line
        LineRendererIndex.startColor = Color.green;
        LineRendererIndex.endColor = Color.green;

        // set width of the renderer
        LineRendererIndex.startWidth = 0.005f;
        LineRendererIndex.endWidth = 0.005f;

        LineRendererIndex.SetVertexCount(3);

        // set the color of the line
        LineRendererMiddle.startColor = Color.green;
        LineRendererMiddle.endColor = Color.green;

        // set width of the renderer
        LineRendererMiddle.startWidth = 0.005f;
        LineRendererMiddle.endWidth = 0.005f;

        LineRendererMiddle.SetVertexCount(3);

        // set the color of the line
        LineRendererRing.startColor = Color.green;
        LineRendererRing.endColor = Color.green;

        // set width of the renderer
        LineRendererRing.startWidth = 0.005f;
        LineRendererRing.endWidth = 0.005f;

        LineRendererRing.SetVertexCount(3);

        // set the color of the line
        LineRendererThumb.startColor = Color.green;
        LineRendererThumb.endColor = Color.green;

        // set width of the renderer
        LineRendererThumb.startWidth = 0.005f;
        LineRendererThumb.endWidth = 0.005f;

        LineRendererThumb.SetVertexCount(3);

        // set the color of the line
        LineRendererPinky.startColor = Color.green;
        LineRendererPinky.endColor = Color.green;

        // set width of the renderer
        LineRendererPinky.startWidth = 0.005f;
        LineRendererPinky.endWidth = 0.005f;

        LineRendererPinky.SetVertexCount(3);
    }

    private void OnEnable()
    {
        Application.onBeforeRender += UpdateRoute;
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= UpdateRoute;
    }

    public void UpdateRoute()
    {
        LineRendererPalm.SetPosition(0, TrackerHandLeft.position);
        LineRendererPalm.SetPosition(1, TrackerHandLeftThumb1.position);
        LineRendererPalm.SetPosition(2, TrackerHandLeftIndex1.position);
        LineRendererPalm.SetPosition(3, TrackerHandLeftMiddle1.position);
        LineRendererPalm.SetPosition(4, TrackerHandLeftRing1.position);
        LineRendererPalm.SetPosition(5, TrackerHandLeftPinky1.position);
        LineRendererPalm.SetPosition(6, TrackerHandLeft.position);

        LineRendererIndex.SetPosition(0, TrackerHandLeftIndex1.position);
        LineRendererIndex.SetPosition(1, TrackerHandLeftIndex2.position);
        LineRendererIndex.SetPosition(2, TrackerHandLeftIndex3.position);

        LineRendererMiddle.SetPosition(0, TrackerHandLeftMiddle1.position);
        LineRendererMiddle.SetPosition(1, TrackerHandLeftMiddle2.position);
        LineRendererMiddle.SetPosition(2, TrackerHandLeftMiddle3.position);

        LineRendererRing.SetPosition(0, TrackerHandLeftRing1.position);
        LineRendererRing.SetPosition(1, TrackerHandLeftRing2.position);
        LineRendererRing.SetPosition(2, TrackerHandLeftRing3.position);

        LineRendererThumb.SetPosition(0, TrackerHandLeftThumb1.position);
        LineRendererThumb.SetPosition(1, TrackerHandLeftThumb2.position);
        LineRendererThumb.SetPosition(2, TrackerHandLeftThumb3.position);

        LineRendererPinky.SetPosition(0, TrackerHandLeftPinky1.position);
        LineRendererPinky.SetPosition(1, TrackerHandLeftPinky2.position);
        LineRendererPinky.SetPosition(2, TrackerHandLeftPinky3.position);
    }
}

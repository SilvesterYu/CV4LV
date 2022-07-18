using UnityEngine;

public class DrawLine : MonoBehaviour
{
    // Trackers
    public Transform TrackerHandRight;

    public Transform TrackerHandRightIndex1;

    public Transform TrackerHandRightIndex2;

    public Transform TrackerHandRightIndex3;

    public Transform TrackerHandRightMiddle1;

    public Transform TrackerHandRightMiddle2;

    public Transform TrackerHandRightMiddle3;

    public Transform TrackerHandRightRing1;

    public Transform TrackerHandRightRing2;

    public Transform TrackerHandRightRing3;

    public Transform TrackerHandRightPinky1;

    public Transform TrackerHandRightPinky2;

    public Transform TrackerHandRightPinky3;

    public Transform TrackerHandRightThumb1;

    public Transform TrackerHandRightThumb2;

    public Transform TrackerHandRightThumb3;

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
        LineRendererPalm.startColor = Color.red;
        LineRendererPalm.endColor = Color.red;

        // set width of the renderer
        LineRendererPalm.startWidth = 0.005f;
        LineRendererPalm.endWidth = 0.005f;

        LineRendererPalm.SetVertexCount(7);

        // set the color of the line
        LineRendererIndex.startColor = Color.red;
        LineRendererIndex.endColor = Color.red;

        // set width of the renderer
        LineRendererIndex.startWidth = 0.005f;
        LineRendererIndex.endWidth = 0.005f;

        LineRendererIndex.SetVertexCount(3);

        // set the color of the line
        LineRendererMiddle.startColor = Color.red;
        LineRendererMiddle.endColor = Color.red;

        // set width of the renderer
        LineRendererMiddle.startWidth = 0.005f;
        LineRendererMiddle.endWidth = 0.005f;

        LineRendererMiddle.SetVertexCount(3);

        // set the color of the line
        LineRendererRing.startColor = Color.red;
        LineRendererRing.endColor = Color.red;

        // set width of the renderer
        LineRendererRing.startWidth = 0.005f;
        LineRendererRing.endWidth = 0.005f;

        LineRendererRing.SetVertexCount(3);

        // set the color of the line
        LineRendererThumb.startColor = Color.red;
        LineRendererThumb.endColor = Color.red;

        // set width of the renderer
        LineRendererThumb.startWidth = 0.005f;
        LineRendererThumb.endWidth = 0.005f;

        LineRendererThumb.SetVertexCount(3);

        // set the color of the line
        LineRendererPinky.startColor = Color.red;
        LineRendererPinky.endColor = Color.red;

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
        LineRendererPalm.SetPosition(0, TrackerHandRight.position);
        LineRendererPalm.SetPosition(1, TrackerHandRightThumb1.position);
        LineRendererPalm.SetPosition(2, TrackerHandRightIndex1.position);
        LineRendererPalm.SetPosition(3, TrackerHandRightMiddle1.position);
        LineRendererPalm.SetPosition(4, TrackerHandRightRing1.position);
        LineRendererPalm.SetPosition(5, TrackerHandRightPinky1.position);
        LineRendererPalm.SetPosition(6, TrackerHandRight.position);

        LineRendererIndex.SetPosition(0, TrackerHandRightIndex1.position);
        LineRendererIndex.SetPosition(1, TrackerHandRightIndex2.position);
        LineRendererIndex.SetPosition(2, TrackerHandRightIndex3.position);

        LineRendererMiddle.SetPosition(0, TrackerHandRightMiddle1.position);
        LineRendererMiddle.SetPosition(1, TrackerHandRightMiddle2.position);
        LineRendererMiddle.SetPosition(2, TrackerHandRightMiddle3.position);

        LineRendererRing.SetPosition(0, TrackerHandRightRing1.position);
        LineRendererRing.SetPosition(1, TrackerHandRightRing2.position);
        LineRendererRing.SetPosition(2, TrackerHandRightRing3.position);

        LineRendererThumb.SetPosition(0, TrackerHandRightThumb1.position);
        LineRendererThumb.SetPosition(1, TrackerHandRightThumb2.position);
        LineRendererThumb.SetPosition(2, TrackerHandRightThumb3.position);

        LineRendererPinky.SetPosition(0, TrackerHandRightPinky1.position);
        LineRendererPinky.SetPosition(1, TrackerHandRightPinky2.position);
        LineRendererPinky.SetPosition(2, TrackerHandRightPinky3.position);
    }
}

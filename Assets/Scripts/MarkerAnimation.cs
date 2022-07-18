using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    float degreesPerSecond = 60;

    private void Update()
    {
        transform.Rotate(new Vector3(0, degreesPerSecond, 0) * Time.deltaTime);
    }
}

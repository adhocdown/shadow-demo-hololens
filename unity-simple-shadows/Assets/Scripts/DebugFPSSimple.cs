using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFPSSimple : MonoBehaviour {

    // https://answers.unity.com/questions/64331/accurate-frames-per-second-count.html
    TextMesh textMesh;
    int frameCount = 0;
    float dt = 0.0f;
    float fps = 0.0f;
    float updateRate = 4.0f;  // 4 updates per sec.

    // Use this for initialization
    void Start()
    {
        textMesh = gameObject.GetComponentInChildren<TextMesh>();
    }

    // Update is called once per frame
    void Update () {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0 / updateRate)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;
        }
        textMesh.text = "FPS: " + (int) fps;

    }
}

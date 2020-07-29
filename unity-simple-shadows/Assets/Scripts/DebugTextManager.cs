using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class DebugTextManager : MonoBehaviour
{
    TextMesh textMesh;

    // https://forums.hololens.com/discussion/708/how-can-i-see-the-unity-debug-log-output-from-a-running-app-on-the-device
    // Use this for initialization
    void Awake()
    {
        textMesh = gameObject.GetComponentInChildren<TextMesh>();
    }

    void OnEnable()
    {
        Application.logMessageReceived += LogMessage;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= LogMessage;
    }

    public void LogMessage(string message, string stackTrace, LogType type)
    {
        if (textMesh.text.Length > 300)
        {
            textMesh.text = message + "\n";
        }
        else
        {
            textMesh.text += message + "\n";
        }
    }
}
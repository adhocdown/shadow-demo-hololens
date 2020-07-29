using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using UnityEngine.XR.WSA.Input;


public class HandTrackedObject4Experiment : MonoBehaviour //, INavigationHandler, ISourceStateHandler, IInputHandler, ISpeechHandler
{
    // GameObject held in hand
    // https://forums.hololens.com/discussion/comment/6049/#Comment_6049 

    // On HoloLens 2 , hands detected fires whenever hands are visible (not just when a finger is pointing up).
    // https://docs.microsoft.com/en-us/windows/mixed-reality/holograms-211

    // https://github.com/ritchielozada/HoloLensHandTracking/blob/master/Assets/Scripts/HandsTrackingController.cs 
    Camera m_MainCamera;
    public bool is_holding_object;

    /// <summary>
    /// HandDetected tracks the hand detected state.
    /// Returns true if the list of tracked hands is not empty.
    /// </summary>
    public bool HandDetected
    {
        get { return trackedHands.Count > 0; }
    }

    public GameObject TrackingObject;
    //public Shadow ShadowObjVars;
    //public GameObject TargetObject; // lazy. do this better later
    GameObject curObject;

    private HashSet<uint> trackedHands = new HashSet<uint>();
    private Dictionary<uint, GameObject> trackingObject = new Dictionary<uint, GameObject>();
    private GestureRecognizer gestureRecognizer;
    private uint activeId;

    // Use this for initialization
    void Start()
    {
        //This gets the Main Camera from the Scene
        m_MainCamera = Camera.main;
        is_holding_object = false;
        //ShadowObjVars = GameObject.Find("Shadow Cube").GetComponent<Shadow>();

        // Register for hand and finger events to know where your hand
        // is being tracked and what state it is in.
        InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
        InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;
        InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;
    }

    public void SetHoldingObjectBool(bool is_holding)
    {
        is_holding_object = is_holding;
    }

   

    private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs args)
    {
        if (!is_holding_object) return;
        uint id = args.state.source.id;
        // Check to see that the source is a hand.
        if (args.state.source.kind != InteractionSourceKind.Hand)
        {
            return;
        }

        trackedHands.Add(id);
        activeId = id;

        var obj = Instantiate(curObject) as GameObject;
        Vector3 pos;

        if (args.state.sourcePose.TryGetPosition(out pos))
        {
            //obj.transform.position = pos + (m_MainCamera.transform.forward) * 0.5f;
            obj.transform.localPosition = pos; // - m_MainCamera.transform.right * 2.5f; // left
            //+ (m_MainCamera.transform.forward) * 0.1f

        }

        trackingObject.Add(id, obj);
    }

    private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs args)
    {
        if (!is_holding_object) return;

        uint id = args.state.source.id;
        Vector3 pos;
        Quaternion rot;

        if (args.state.source.kind == InteractionSourceKind.Hand)
        {
            if (trackingObject.ContainsKey(id))
            {
                if (args.state.sourcePose.TryGetPosition(out pos))
                {
                    trackingObject[id].transform.position = pos
                        + (m_MainCamera.transform.forward) * 0.1f
                        - (m_MainCamera.transform.right) * 0.1f; //m_MainCamera.transform.right * 0.2f;
                }

                if (args.state.sourcePose.TryGetRotation(out rot))
                {
                    trackingObject[id].transform.rotation = rot;
                }

            }
        }
    }

    private void InteractionManager_InteractionSourceLost(InteractionSourceLostEventArgs args)
    {
        uint id = args.state.source.id;
        // Check to see that the source is a hand.
        if (args.state.source.kind != InteractionSourceKind.Hand)
        {
            return;
        }

        if (trackedHands.Contains(id))
        {
            trackedHands.Remove(id);
        }

        if (trackingObject.ContainsKey(id))
        {
            var obj = trackingObject[id];
            trackingObject.Remove(id);
            Destroy(obj);
        }
        if (trackedHands.Count > 0)
        {
            activeId = trackedHands.First();
        }
    }
}

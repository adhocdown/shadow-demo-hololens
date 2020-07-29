using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;



public class SetWorldAnchor4Experiment : MonoBehaviour
{

    ShadowManager4Experiment shadowManager;
    public GameObject mySceneObject; //Shadow Cube
    public GameObject mySceneUI;
    public static bool ischangedText;
    public static GameObject SceneObject;
    public static GameObject SceneUI;

    public static bool active_toggle;

    void Start()
    {
        SceneObject = mySceneObject;
        shadowManager = mySceneObject.GetComponent<ShadowManager4Experiment>();

        SceneUI = mySceneUI;
        active_toggle = false;
        SetAnchor();
    }

    // on click - set world anchor and dependencies  
    // public virtual void OnInputClicked(InputClickedEventData eventData) 
    public void SetAnchor()
    {        
        active_toggle = !active_toggle;
        Debug.Log("is Setting Anchor Bool: " + active_toggle); 
        transform.GetComponent<HoloToolkit.Unity.SpatialMapping.TapToPlace4Experiment>().enabled = active_toggle;
        transform.GetComponentInChildren<MeshRenderer>().enabled = active_toggle;

        UpdateUITransform();
        UpdateObjectsTransform();
    }

    // set position and orientation of UI relative to anchor 
    // SceneUI is the parent to the actual Canvas element
    // Called in TapToPlace.cs by 'My Anchor Manager' object
    public void UpdateUITransform()
    {
        SceneUI.transform.position = transform.position;
        SceneUI.transform.rotation = transform.rotation;
    }

    // Show scene objects (shadow cubes) when active toggle is true 
    // set position and orientation of NEW spatial anchor relative to first world anchor (set at origin) 
    // the NEW spatial anchor is a distance out from origin based on UI settings 
    // and the shadow cubes are displayed at this new spatial anchor. 
    // Using a 2nd spatial anchor stabilizes the shadow cubes GOs. 
    public void UpdateObjectsTransform()
    {
        SceneObject.GetComponent<ShadowManager4Experiment>().UpdatePrimaryAnchorTransform(transform);
        SceneObject.transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
        SceneObject.transform.rotation = transform.rotation; //transform.position.y + shadowManager.GetSurfaceHeight()

        SceneObject.GetComponent<ShadowManager4Experiment>().UpdateDistance();
        SceneObject.GetComponent<ShadowManager4Experiment>().UpdateIntermediateAnchors(!active_toggle);
    }


    // Called in Shadow Manager's ShadowManager4Experiment script 
    public void PositionAnchorCube(float surfaceHeight)
    { 
        transform.localPosition = new Vector3(0, surfaceHeight, 0);         
    }

}

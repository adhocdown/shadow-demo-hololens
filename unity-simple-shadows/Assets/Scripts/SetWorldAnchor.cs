using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;



public class SetWorldAnchor : MonoBehaviour { 

    public GameObject mySceneObjects; //Shadow Cube
    public GameObject mySceneUI;
    public static bool ischangedText; 
    public static GameObject SceneObjects;
    public static GameObject SceneUI;

    public static bool active_toggle;

    void Start()
    {
        SceneObjects = mySceneObjects;
        SceneUI = mySceneUI;
        active_toggle = false;
        SetAnchor();
    }

    // on click - set world anchor and dependencies  
    // public virtual void OnInputClicked(InputClickedEventData eventData) 
    public void SetAnchor()
    {
        active_toggle = !active_toggle;
        transform.GetComponent<HoloToolkit.Unity.SpatialMapping.TapToPlace>().enabled = active_toggle;
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
        SceneObjects.GetComponent<ShadowManager>().UpdatePrimaryAnchorTransform(transform);
        SceneObjects.transform.position = transform.position;
        SceneObjects.transform.rotation = transform.rotation;

        SceneObjects.GetComponent<ShadowManager>().UpdateDistance();
        SceneObjects.GetComponent<ShadowManager>().UpdateIntermediateAnchors(!active_toggle);
        SceneObjects.SetActive(!active_toggle);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;


public class ToggleWorldAnchor : MonoBehaviour, IInputClickHandler {

	public GameObject AnchorManager;
    public GameObject mySceneObjects;
    public static bool ischangedText; 
    public static GameObject SceneObjects; 
    public static bool active_toggle;

    void Start()
    {
        active_toggle = true;
        SceneObjects = mySceneObjects;
        //SceneObjects = GameObject.Find("Madison Cubes");
        //SceneObjects.SetActive(!active_toggle); 
        SetText(); 
        

    }

    // on click - set world anchor and dependencies
    // toggle 
    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        active_toggle = !active_toggle;
        AnchorManager.GetComponent<HoloToolkit.Unity.SpatialMapping.TapToPlace>().enabled = active_toggle;
        AnchorManager.GetComponentInChildren<MeshRenderer>().enabled = active_toggle;

        SceneObjects.SetActive(!active_toggle); 
        SceneObjects.transform.position = AnchorManager.transform.position;
        SceneObjects.transform.rotation = AnchorManager.transform.rotation;
        GetComponent<TextMesh>().text = "Active: " + mySceneObjects.name;
        SetText(); 
    }

    void Update()
    {
        if (ischangedText)
        {
            SetText();
            ischangedText = !ischangedText; 
        }                   
    }

    public void SetText()
    {
        if (active_toggle)
        {
            GetComponent<TextMesh>().text = "Set Anchor OR Select Different Shadow \n[Active: " + SceneObjects.name +"]";
            GetComponent<TextMesh>().color = new Color32(255, 200, 30, 255);
        }
        else
        {
            GetComponent<TextMesh>().text = "Look at shadows! \n[Active: " + SceneObjects.name + "]";
            GetComponent<TextMesh>().color = new Color32(30, 255, 100, 255);

        }
    }

    

}

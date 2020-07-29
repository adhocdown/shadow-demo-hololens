using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;


public class ToggleActiveObjects : MonoBehaviour, IInputClickHandler
{

    public GameObject mySceneObjects;

    void Start() {
        mySceneObjects.SetActive(false); 
    }

    // reassign SceneObjects for ToggleWorldAnchor script 
    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        if (ToggleWorldAnchor.active_toggle == true)
        {     
            ToggleWorldAnchor.SceneObjects = mySceneObjects;
            ToggleWorldAnchor.ischangedText = !ToggleWorldAnchor.ischangedText; // bool used to trigger text update
        }
    }

}

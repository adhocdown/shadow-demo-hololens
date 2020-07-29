using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;


public class ToggleGroundContact : MonoBehaviour, IInputClickHandler
{
    public bool isOnGround;
    public GameObject[] cubes;

    // Use this for initialization
    void Awake () {
        isOnGround = true;
        cubes = GameObject.FindGameObjectsWithTag("cube");
    }

    // toggle on or off the ground 
    // 0.0525 on ground
    // 0.1525 above ground 
    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        isOnGround = !isOnGround;       
        foreach (GameObject cube in cubes)
        {
            if (isOnGround)
            {
                cube.transform.localPosition = new Vector3(0f, 0.0525f, 0f);
                GetComponent<TextMesh>().text = "On Ground";
            }
            else
            {
                cube.transform.localPosition = new Vector3(0f, 0.1525f, 0f);
                GetComponent<TextMesh>().text = "Above Ground";
            }      
        }           
    }  
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMeLater : MonoBehaviour {


    public Camera myCamera;
        public float sightlength = 100.0f;
    public GameObject selectedObj;

    void Update()
    {
        RaycastHit hit;
        //Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
        Ray ray = new Ray(transform.position, transform.forward);

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        //Debug.DrawRay(transform.position, forward, Color.green);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Transform objectHit = hit.transform;
            ButtonRaycastHit();

            // Do something with the object that was hit by the raycast.
        }
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);


    }

    

    public void ButtonRaycastHit()
    {
        Debug.Log("Button HIIIIIIT !!");

    }

    public void ButtonPressed()
    {
        Debug.Log("Button Pressed!!");
    }
}

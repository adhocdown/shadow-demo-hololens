using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedFramePos : MonoBehaviour {

    public Transform[] positions; 

    private void Start()
    {
      
    }

    public void SetFramePosition(int index)
    {
        transform.position = positions[index].position; 
    }

}

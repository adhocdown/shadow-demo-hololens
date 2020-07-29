using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderManager : MonoBehaviour {

    public List<Transform> Sliders; 

    void Start()
    {
        foreach (Transform child in transform)        
            Sliders.Add(child);
        
        SelectSlider(1);                 
    }

    public void SelectSlider(int index)
    {
        foreach (Transform slider in Sliders)       
            slider.gameObject.SetActive(false);

        Sliders[index].gameObject.SetActive(true);
    }



}

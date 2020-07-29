using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To manage the dark shadow prefabs: create an empty game object to manage them, 
// attach this script, then set all the "Negative Shadow Cube" prefabs as children.

public class LargeNegativeShadowManager : MonoBehaviour {
   
    
    public Material shadowMaterial;
    public Color32 activeColor;

    Color32 darkColor; 
    Color32 lightColor; 

    // Get "Neg Shadow Plane" material reference
    // And assign preset color values
    void Awake () {
        
        darkColor = new Color32(30, 30, 30, 255);
        lightColor = new Color32(255, 255, 255, 255);
        activeColor = darkColor;

        shadowMaterial = transform.GetChild(0).GetComponent<Renderer>().material;
        shadowMaterial.SetColor("_ColorA", activeColor);
    }

    // Set activeColors for Dark Shadow Cube prefab 
    public void toggleActiveColors(bool isbright)
    {
        if (isbright) 
            activeColor = lightColor;
        else
            activeColor = darkColor;
        SetMaterialColor();
    } 

    // Assign color values to material 
    private void SetMaterialColor() 
    {
        shadowMaterial.SetColor("_ColorA", activeColor);
    }

    // For editor execution: when a color value changes in inspector
    // the change is updated to the dark shadow cube gos
    void OnValidate()
    {
        SetMaterialColor(); 
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To manage the dark shadow prefabs: create an empty game object to manage them, 
// attach this script, then set all the "Negative Shadow Cube" prefabs as children.

public class NegativeShadowManager : MonoBehaviour {
   
    public List<Material> shadowMaterials;
    public List<Color32> activeColors;

    List<Color32> darkColorList = new List<Color32>()
    {
        new Color32(30, 30, 30, 255),
        new Color32(40, 40, 40, 255),
        new Color32(60, 60, 60, 255),
        new Color32(80, 80, 80, 255),
    };  

    List<Color32> lightColorList = new List<Color32>()
    {
        new Color32(100, 100, 100, 255),
        new Color32(150, 150, 150, 255),
        new Color32(200, 200, 200, 255),
        new Color32(255, 255, 255, 255),
    };

    // Get "Neg Shadow Plane" material reference
    // And assign preset color values
    void Awake () {
        GetShadowMaterials();
        SetColors(darkColorList);
    }

    // Find "Neg Shadow Plane". Save instanced material references
    private void GetShadowMaterials()
    {
        foreach (Transform child in transform)
        {
            shadowMaterials.Add(child.GetChild(2).transform.GetComponent<Renderer>().material); // save reference
        }
    }

    // Assign preset color values to active color list 
    private void SetColors(List<Color32> myColorList)
    {
        activeColors.Clear();
        foreach (Color32 col in myColorList)
        {
            activeColors.Add(col);
        }
        SetMaterialColors(); 
    }

    // Assign color values to materials 
    private void SetMaterialColors()
    {
        for (int i=0; i < activeColors.Count; i++) {
            shadowMaterials[i].SetColor("_ColorA", activeColors[i]);
        }
    }

    // Set activeColors for Dark Shadow Cube prefab 
    public void toggleActiveColors(bool isbright)
    {
        if (isbright)
            SetColors(lightColorList);
        else
            SetColors(darkColorList);
    }

    // For editor execution: when a color value changes in inspector
    // the change is updated to the dark shadow cube gos
    /*void OnValidate()
    {
        SetMaterialColors(); 
    }
    */


}

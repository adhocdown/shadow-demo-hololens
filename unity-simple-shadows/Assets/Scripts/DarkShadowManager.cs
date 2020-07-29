using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To manage the dark shadow prefabs: create an empty game object to manage them, 
// attach this script, then set all the "Dark Shadow Cube" prefabs as children.

public class DarkShadowManager : MonoBehaviour {
   
    public List<Material> shadowMaterials;
    public List<Color32> activeColors;

    List<Color32> darkColorList = new List<Color32>()
    {
        new Color32(5, 5, 5, 255),
        new Color32(10, 10, 10, 255),
        new Color32(15, 15, 15, 255),
        new Color32(20, 20, 20, 255),
    };

    List<Color32> lightColorList = new List<Color32>()
    {
        new Color32(30, 30, 30, 255),
        new Color32(50, 50, 50, 255),
        new Color32(70, 70, 70, 255),
        new Color32(90, 90, 90, 255),
    };

    // Get Shadow Plane material reference
    // And assign preset color values
    void Awake () {
        GetShadowMaterials();
        SetColors(darkColorList);
    }

    // Find "Shadow Plane". Save instanced material references
    private void GetShadowMaterials()
    {
        foreach (Transform child in transform)
        {
            shadowMaterials.Add(child.GetChild(1).transform.GetComponent<Renderer>().material); // save reference
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
            shadowMaterials[i].SetColor("_Color", activeColors[i]);
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

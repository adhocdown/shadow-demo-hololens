using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadisonShadowManager : MonoBehaviour {

    // These shadow scripts are fine for the demo, but
    // they should be reorganized for efficiency / stability 

    public List<Material> shadowMaterials;
    public List<Color32> activeColors;

    List<Color32> darkColorList = new List<Color32>()
    {
        new Color32(30, 30, 30, 255), // dark shadow  
        new Color32(30, 30, 30, 255), // negative shadow       
    };

    List<Color32> lightColorList = new List<Color32>()
    {
        new Color32(80, 80, 80, 255), // dark shadow
        new Color32(180, 180, 180, 255), // negative shadow
    };


    // Get "Shadow Plane" material reference from Dark Shadow Cube
    // And get "Neg Shadow Plane" material reference from Negative Shadow Cube 
    // And assign preset color values
    void Awake () {
        GetShadowMaterials();

        SetColors(darkColorList);
    }

    // Find "Neg Shadow Plane". Save instanced material references
    private void GetShadowMaterials()
    {
        Transform DarkShadowCube = transform.GetChild(2);
        Transform NegShadowCube = transform.GetChild(3);
        shadowMaterials.Clear(); 
        shadowMaterials.Add(DarkShadowCube.GetChild(1).transform.GetComponent<Renderer>().material); // dark shadow cube
        shadowMaterials.Add(NegShadowCube.GetChild(2).transform.GetComponent<Renderer>().material); // negative shadow cube      
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
        shadowMaterials[0].SetColor("_Color", activeColors[0]);
        shadowMaterials[1].SetColor("_ColorA", activeColors[1]);       
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

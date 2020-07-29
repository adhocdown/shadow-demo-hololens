using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;


public class ShadowManager : MonoBehaviour {

    // Assign in inspector - "My Anchor Manager"
    public Transform anchorTransform;
        
    // Replace some values with this? 
    public Shadow ShadowObjVars;
    bool isShadowAnchored; 

    // Manpulate at runtime 
    private Transform objectTransform;
    private Transform intermediateAnchorsParent;
    public GameObject intermediateAnchor;
    private Material shadowMaterial;
    public Material groundMaterial;

    // Private float objectHeight;         // 0.105f 
    public float maxHeight;
    public float maxDistance; // for WorldAnchor breadcrumb trail
    public int intervalDistance; // distance between intermediate WorldAnchors
    private int numAnchors;  // number of intermediate WorldAnchors 
    

    public Light myLight;
    private Object[] materials;
    public List<Transform> objects;


    // Use this for initialization
    void Start () {        

        ShadowObjVars = transform.GetComponent<Shadow>();
        objectTransform = transform.GetChild(0);    // parent of all objects
        Transform shadowTransform = transform.GetChild(1);   
        Transform groundTransform = transform.GetChild(2);
        intermediateAnchorsParent = anchorTransform.GetChild(2); // parent for intermediate WorldAnchors from origin to target obj


        shadowMaterial = shadowTransform.transform.GetComponent<Renderer>().material;
        groundMaterial = groundTransform.transform.GetComponent<Renderer>().material;
        isShadowAnchored = false;

        //objectHeight = 0.105f; // only cube 
        //maxDistance = 0.20f; // og slider = 20f. but now slider works for decimals  
        maxHeight = 0.1f;
        intervalDistance = 3; 
        numAnchors = 0; 

        Light[] lights = FindObjectsOfType(typeof(Light)) as Light[];
        foreach (Light light in lights)
        {
            if (light.name == "Directional Light")
                myLight = light; 
        }
        materials = Resources.LoadAll("Materials", typeof(Material));
         
        foreach (Transform child in objectTransform)
            objects.Add(child);
        SelectShape(0); 

	}

    // called when button clicked in UI 
    // [ translate=0, object=1, shadow=2, or light mode=3 ]
    public void GetCurrentSetting(int value)
    {
        ShadowObjVars.current_setting = value;
    }
   

    /* 
     * 
     * Distance and Height Functions
     * Called by Slider1 and Slider2, respectively  
     * 
     */   
     
    public void AddToDistance()
    {
        ShadowObjVars.distance_whole_unit += 1; 
        UpdateDistance();
    }
    public void SubtractFromDistance()
    {
        ShadowObjVars.distance_whole_unit -= 1;
        UpdateDistance();
    }

    public void SetDistanceDecimalNumbers(float distance)
    {
        ShadowObjVars.distance_decimal = distance;
        UpdateDistance();
    } 
    
    public void UpdatePrimaryAnchorTransform(Transform primaryAnchorTransform)
    {
        anchorTransform = primaryAnchorTransform;
    }

    public void UpdateDistance()
    {
        RemoveAnchor();

        if (ShadowObjVars.distance_whole_unit >= 0)
            ShadowObjVars.distance = ShadowObjVars.distance_whole_unit + ShadowObjVars.distance_decimal;
        else
            ShadowObjVars.distance = ShadowObjVars.distance_whole_unit - ShadowObjVars.distance_decimal;

        transform.position = anchorTransform.position + anchorTransform.forward * ShadowObjVars.distance;
        AttachAnchor();                
    }


    // check: only place new anchor when current max distance is greater
    // than the num of anchors * an interval distance (default 3m) 
    public void UpdateIntermediateAnchors(bool active_toggle)
    {
        numAnchors = intermediateAnchorsParent.childCount;

        // setting world anchor in progress. Destory old anchors. 
        if (!active_toggle && numAnchors != 0)
        {
            RemoveIntermediateAnchors();
            LogAllAnchors();
        }
        // if active toggle and not enough WorldAnchors, then add as needed 
        else if (active_toggle && (numAnchors + 1) <= (ShadowObjVars.distance / intervalDistance)) {
            while ((numAnchors + 1) <= (ShadowObjVars.distance / intervalDistance))
            {
                AddIntermediateAnchors();
            }
            LogAllAnchors();
        }
    }

    public void LogAllAnchors()
    {
        transform.GetComponent<HoloToolkit.Unity.SpatialMapping.ShadowSpatialAnchor>().LogAnchorsInfo(); 
    }

    public void RemoveIntermediateAnchors()
    {
        foreach (Transform child in intermediateAnchorsParent)  
        {
            Destroy(child.gameObject); 
        }
    }

    public void AddIntermediateAnchors()
    {
        var go = Instantiate(intermediateAnchor, anchorTransform.position, Quaternion.identity);
        go.transform.localPosition = new Vector3(0, 0, (numAnchors + 1) * intervalDistance);
        go.transform.SetParent(intermediateAnchorsParent, false);
        go.AddComponent<WorldAnchor>();
        numAnchors = intermediateAnchorsParent.childCount;
    }

    private void RemoveAnchor()
    {
        // remove spatial anchor 
        if (isShadowAnchored)
        {
            transform.GetComponent<HoloToolkit.Unity.SpatialMapping.ShadowSpatialAnchor>().RemoveWorldAnchor();
            isShadowAnchored = false; 
        }
    } 
    private void AttachAnchor()
    {
        // attach spatial anchor 
        if (!isShadowAnchored)
        {
            transform.GetComponent<HoloToolkit.Unity.SpatialMapping.ShadowSpatialAnchor>().AttachWorldAnchor();
            isShadowAnchored = true; 
        }
    }
    

    public void SetHeight(float value)
    {
        ShadowObjVars.height = (value * maxHeight);
        objectTransform.localPosition = new Vector3(0f, ShadowObjVars.height, 0f);

        // constrain light angle (x) 
        ShadowObjVars.AdjustLightXAngleRange();        
        SetLightXAngle(ShadowObjVars.raw_x_angle);
    }

    

       /* 
        * 
        * Shadow & Ground plane Functions 
        * Called by Slider1 and Slider2, respectively  
        * 
        */

    public void SetShadowIntensity(float intensity)
    { 
        ShadowObjVars.shadow_intensity = (int) (intensity * 255f);
        byte col = (byte)(intensity * 255);
        shadowMaterial.SetColor("_Color", new Color32(col, col, col, 255));
    }
    public void SetGroundIntensity(float intensity)
    {
        ShadowObjVars.ground_intensity = (int) (intensity * 255f);
        byte col = (byte)(intensity * 255);
        groundMaterial.SetColor("_ColorA", new Color32(col, col, col, 255));

        // Add noise if value > 0 to prevent ringing artifacts in radial gradient 
        if (intensity == 0)
            groundMaterial.SetFloat("_Noise", 0.0f);
        else
            groundMaterial.SetFloat("_Noise", 0.015f);
    }

    /* 
     * 
     * Shadow & Ground plane Functions 
     * Called by Slider1 and Slider2, respectively  
     * x angle [20, 160]
     * y angle [0, 360] 
     */

    public void SetLightXAngle(float angle)
    {
        ShadowObjVars.raw_x_angle = angle; 
        ShadowObjVars.light_x_angle = ShadowObjVars.min_x_angle + angle* ShadowObjVars.max_x_angle;
        myLight.transform.rotation = Quaternion.Euler(ShadowObjVars.light_x_angle, ShadowObjVars.light_y_angle, 0);
    }
    public void SetLightYAngle(float angle)
    {
        ShadowObjVars.light_y_angle = angle * 360f;
        myLight.transform.rotation = Quaternion.Euler(ShadowObjVars.light_x_angle, ShadowObjVars.light_y_angle, 0);
    }

    /* 
     * 
     * Object & Texture Functions
     * Called by Slider1 and Slider2, respectively  
     * x angle [20, 160]
     * y angle [0, 360] 
     */

    public void SetObjectShape(float index)
    {    
        ShadowObjVars.shape_index = (int) (index*6f);
        SelectShape(ShadowObjVars.shape_index);
        // update texture & light angle x range, too
        SetObjectTexture(ShadowObjVars.texture_index);
        ShadowObjVars.AdjustLightXAngleRange();

    }
    public void SetObjectTexture(float index)
    {
        int myindex; 
        ShadowObjVars.texture_index = (int) index;
        
        if (ShadowObjVars.texture_index == 1)
            myindex = ShadowObjVars.shape_index;
        else
            myindex = 7; // simple texture index 

        objectTransform.GetChild(ShadowObjVars.shape_index)
            .GetComponent<Renderer>().material = materials[myindex] as Material;           
    }

    public void SelectShape(int index)
    {
        foreach (Transform obj in objects)
            obj.gameObject.SetActive(false);
        objects[index].gameObject.SetActive(true);        
    }

}

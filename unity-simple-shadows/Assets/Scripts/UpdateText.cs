using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateText : MonoBehaviour {

    public string myText;
    public bool isAnchored; 


    public Shadow shadow; // consider changing this for consistency across scripts 
    public ShadowManager shadowManager; // consider changing this for consistency across scripts 


    // Use this for initialization
    void Awake()
    {
        myText = transform.GetChild(0).transform.GetComponent<Text>().text;

        GameObject shadowGO = GameObject.Find("Shadow Cube");
        shadow = shadowGO.transform.GetComponent<Shadow>();
        shadowManager = shadowGO.transform.GetComponent<ShadowManager>();

    }


    // Text for Translation Slider 
    // Distance of object from anchor  
    public void SetDistanceText()
    {
        var dString = string.Format("{0:N2}", shadow.distance); 
        myText = "Distance\t| " + dString;
        UpdateMyText();
        UpdateMySlider(shadow.distance_decimal);
    }
    // Text for Translation Slider 
    // Distance of object from anchor
    public void SetDistanceTextNumberOnly()
    {
        var dString = string.Format("{0:N2}", shadow.distance);
        myText = dString;
        UpdateMyText();
    }
    // Height of object from anchor
    public void SetHeightText()
    {
        var dString = string.Format("{0:N3}", shadow.height);
        myText = "Height\t\t| " + dString;
        UpdateMyText();
        UpdateMySlider(shadow.height/ shadowManager.maxHeight);
    }

    /* 
     * 
     * Shadow & Ground Text UI Functions 
     * Called by Slider1 and Slider2, respectively  
     * x angle [20, 120]
     * y angle [0, 360] 
     */

    public void SetLightXText()
    {
        var dString = (shadow.light_x_angle).ToString("#"); // returns "" when decimalVar == 0
        myText = "X Angle\t| " + dString;
        UpdateMyText();
        UpdateMySlider((shadow.light_x_angle- shadow.min_x_angle) / shadow.max_x_angle); 
    }
    public void SetLightYText()
    {
        var dString = (shadow.light_y_angle).ToString("#"); // returns "" when decimalVar == 0
        myText = "Y Angle\t| " + dString;
        UpdateMyText();
        UpdateMySlider(shadow.light_y_angle/360f);
    }


    // Text for Object Slider  
    // object shape selection   
    public void SetShapeText()
    {
        myText = "Shape\t\t| " + shadow.shape_index;
        UpdateMyText();
        UpdateMySlider((float) shadow.shape_index/6f); 
    }
    // texture for object
    public void SetTextureText()
    {
        myText = "Texture\t| " + shadow.texture_index;
        UpdateMyText();
        UpdateMySlider((float) shadow.texture_index);
    }


    /* 
     * 
     * Shadow & Ground Text UI Functions 
     * Called by Slider1 and Slider2, respectively  
     * [0, 255]
     * 
     */

    public void SetShadowIntensityText()
    {
        myText = "Shadow\t| " + shadow.shadow_intensity;
        UpdateMyText();
        UpdateMySlider((float) shadow.shadow_intensity / 255f);
    }
    // ground (radial gradient)  intensity [0, 255] 
    public void SetGroundIntensityText()
    {
        myText = "Ground\t| " + shadow.ground_intensity;
        UpdateMyText();
        UpdateMySlider((float) shadow.ground_intensity / 255f);
    }


    // Text for Start/Anchor
    public void SetAnchorText(float distance)
    {
        isAnchored = !isAnchored;
        if (isAnchored)
            myText = "Look at Shadows!";
        else
            myText = "Set Anchor";
        UpdateMyText();
    }


    // Update the UI !   called on button clicks 
    void UpdateMyText()
    {
        transform.GetChild(0).transform.GetComponent<Text>().text = myText;
    }
    public void UpdateMySlider(float val) 
    {        
        transform.GetComponent<Scrollbar>().value = val; 
    }
    

}

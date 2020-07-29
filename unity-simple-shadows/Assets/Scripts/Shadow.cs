using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour {

    public int current_setting;

    public int   distance_whole_unit;
    public float distance_decimal;
    public float distance;
    public float height;

    public float light_x_angle;
    public float light_y_angle;
    public float max_x_angle;  // 120, 140 defaults
    public float min_x_angle;  // 120, 140 defaults
    public float raw_x_angle; 

    public int shadow_intensity;
    public int ground_intensity;

    public int shape_index;
    public int texture_index;


    void Start()
    {
        ResetValues(); 
    }


    public void ResetValues()
    {
        current_setting = 0;

        distance_whole_unit = 0;
        distance_decimal = 0f;
        distance = 0f;
        height = 0f;

        light_x_angle = 20f;
        light_y_angle = 20f;
        min_x_angle = 20f; 
        max_x_angle = 140f;
        raw_x_angle = 0f; 

        shadow_intensity = 0;
        ground_intensity = 100;
        shape_index = 0;
        texture_index = 1; 
    }

    public void AdjustLightXAngleRange()
    {
        // object on ground
        if (height < 0.035)
        {
            switch(shape_index)
            {
                case 0:
                    min_x_angle = 20f; 
                    max_x_angle = 140f; // 160
                    break;
                case 1:
                    min_x_angle = 15f;
                    max_x_angle = 150f; // 165
                    break;
                case 2:
                    min_x_angle = 60f;
                    max_x_angle = 75f; // 135
                    break;
                case 3:
                    min_x_angle = 75f;
                    max_x_angle = 30f; // 105
                    break;
                default:
                    min_x_angle = 15f;
                    max_x_angle = 150f; // 165
                    break;
            }
        }
        else // object floating 
        {
            switch (shape_index)
            {
                case 0:
                    min_x_angle = 25f; 
                    max_x_angle = 130f;  
                    break;
                case 1:
                    min_x_angle = 20f;
                    max_x_angle = 140f; //160
                    break;
                case 2:
                    min_x_angle = 65f;
                    max_x_angle = 65f; // 130
                    break;
                case 3:
                    min_x_angle = 75f;
                    max_x_angle = 30f; // 105
                    break;
                default:
                    min_x_angle = 20f;
                    max_x_angle = 140f; //160
                    break;
            }
        }
    }


    // update stored values when slider is updated
    // in: current button
    // in: value 
    // write func here. call in shadow manager 

    // button pressed -> msg to shadow manager
    // slider updated -> 
    // also need to perform action in shadow manager 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowExperimentManager : MonoBehaviour
{

    bool isOnGround;
    int shadowIndex;

    private Transform cubeTransform;
    Transform shadowTransform;
    Transform groundTransform;
    private Material shadowMaterial;
    private Material groundMaterial;

    public Color darkColor;
    public Color whiteColor;

    // Use this for initialization
    void Start()
    {
        isOnGround = true;  // default: on ground
        shadowIndex = 3;    // default: sim contrast shadow 

        cubeTransform = transform.GetChild(0);
        shadowTransform = transform.GetChild(1);
        groundTransform = transform.GetChild(2);

        shadowMaterial = shadowTransform.transform.GetComponent<Renderer>().material;
        groundMaterial = groundTransform.transform.GetComponent<Renderer>().material;
    }

    public void SetCubeHeight(int _isOnGround_Index, int cur_block)
    {
        if (_isOnGround_Index == 1) // on ground
        {
            isOnGround = true;
            cubeTransform.localPosition = new Vector3(cubeTransform.localPosition.x, 0.0f, cubeTransform.localPosition.z);
        }
        else // above ground 
        {
            isOnGround = false;
            if (cur_block == 1)          // 1: table 1m   --->   lift 
                cubeTransform.localPosition = new Vector3(cubeTransform.localPosition.x, 0.0029968586998179183f, cubeTransform.localPosition.z);
            else if (cur_block == 2)     // 2: ground 1m   --->   lift 
                cubeTransform.localPosition = new Vector3(cubeTransform.localPosition.x, 0.011226663618038691f, cubeTransform.localPosition.z);
            else                        // 3: ground 3m   --->   lift 
                cubeTransform.localPosition = new Vector3(cubeTransform.localPosition.x, 0.012051115920044823f, cubeTransform.localPosition.z);



        }
    }

    public void SetShadowMaterials(int _ShadowMat_Index)
    {
        MeshRenderer sh_r = shadowTransform.GetComponent<MeshRenderer>();

        switch (_ShadowMat_Index)
        {
            case 0:                                             // no shadow 
                sh_r.enabled = false;
                sh_r.receiveShadows = true;
                groundTransform.GetComponent<MeshRenderer>().enabled = false;
                break;
            case 1:                                             // gray shadow
                sh_r.enabled = true;
                sh_r.receiveShadows = true;
                shadowMaterial.SetColor("_Color", darkColor);
                groundTransform.GetComponent<MeshRenderer>().enabled = false;
                break;
            case 2:                                             // white shadow
                sh_r.enabled = true;
                sh_r.receiveShadows = true;
                shadowMaterial.SetColor("_Color", whiteColor);
                groundTransform.GetComponent<MeshRenderer>().enabled = false;
                break;
            case 3:                                             // contrast shadow                 
                sh_r.enabled = true;
                sh_r.receiveShadows = false; // avoid drawing shadows twice 
                                             //shadowMaterial.SetColor("_ColorA", gradientColor);
                groundTransform.GetComponent<MeshRenderer>().enabled = true;
                break;
            default:
                break;
        }
    }
}
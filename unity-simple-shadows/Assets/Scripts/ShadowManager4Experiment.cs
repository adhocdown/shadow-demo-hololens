using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;


public class ShadowManager4Experiment : MonoBehaviour {

    // Assign in inspector - "My Anchor Manager"
    public Transform anchorTransform;
    //public Shadow ShadowObjVars;
    public int targetDistance;
    float surfaceHeight; 

    bool isShadowAnchored;
    private Transform objectTransform;
    private Transform intermediateAnchorsParent;
    public GameObject intermediateAnchor;

    public float maxDistance; // for WorldAnchor breadcrumb trail
    public int intervalDistance; // distance between intermediate WorldAnchors
    private int numAnchors;  // number of intermediate WorldAnchors 
    private float z_offset; 

    void Awake()
    {

        objectTransform = transform.GetChild(0);    // parent of all objects
        //Transform shadowTransform = transform.GetChild(1);
        //Transform groundTransform = transform.GetChild(2);
        intermediateAnchorsParent = anchorTransform.GetChild(2); // parent for intermediate WorldAnchors from origin to target obj
        intervalDistance = 3;
        numAnchors = 0;
        z_offset = 0.5f;
        surfaceHeight = 0f;
    }

    public void SetPositionRelativeToAnchor(int cur_block)
    {
        if (cur_block == 3) z_offset = 3.0f; // set 3m away in block 3
        else { z_offset = 0.5f; } // else set 0.5m     
    }

    public void AddToDistance()
    {
        targetDistance += 1;
        UpdateDistance();
    }
    public void SubtractFromDistance()
    {
        targetDistance -= 1;
        UpdateDistance();
    }

    public void UpdateDistance()
    {
        RemoveAnchor();
        transform.localPosition = new Vector3(anchorTransform.localPosition.x, anchorTransform.localPosition.y, anchorTransform.localPosition.z);
        transform.localPosition += transform.forward * z_offset; 
        AttachAnchor();
    }

    public void UpdatePrimaryAnchorTransform(Transform primaryAnchorTransform)
    {
        anchorTransform = primaryAnchorTransform;
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
        else if (active_toggle && (numAnchors + 1) <= (targetDistance / intervalDistance))
        {
            while ((numAnchors + 1) <= (targetDistance / intervalDistance))
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


    // Set surfaceHeight() to move anchor cube and displace position of spatial anchor
    public void SetSurfaceHeight(float value)
    {
        // variable moves Shadow Manger
        surfaceHeight = (value * 0.2f) - 0.1f;
        // but also need to move anchor manager > (1) anchor cube 
        anchorTransform.GetComponent<SetWorldAnchor4Experiment>().PositionAnchorCube(surfaceHeight);
    }
    
    public float GetSurfaceHeight()
    {
        return surfaceHeight;
    }
}

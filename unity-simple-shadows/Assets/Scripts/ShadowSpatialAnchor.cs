using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_2017_2_OR_NEWER
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Persistence;
#else
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;
#endif
#if !UNITY_EDITOR
using HoloToolkit.Unity.SpatialMapping;
#endif

namespace HoloToolkit.Unity.SpatialMapping
{
    /// This script is used in conjunction with WorldAnchorManager and SpatialMappingManager.
    /// When gameobject is moved in UI, update the world anchor 
    /// Check for updates in ShadowManager script 
    public class ShadowSpatialAnchor : MonoBehaviour
    {
        WorldAnchor anchor; 

        public void UpdateWorldAnchor()
        {
            RemoveWorldAnchor();
            AttachWorldAnchor();
        }

        // Add world anchor when object placement is done.
        public void AttachWorldAnchor()
        {
            if (WorldAnchorManager.Instance != null)
            {
                Debug.Log("[ShadowSpatialAnchor] Attach anchor " + this.gameObject.name + " @ " + this.gameObject.transform.position);
                //WorldAnchorManager.Instance.AttachAnchor(this.gameObject, this.gameObject.name);                
                anchor = this.gameObject.AddComponent<WorldAnchor>(); 
            }
            LogAnchorsInfo();
        }

        // Removes existing world anchor if any exist.
        public void RemoveWorldAnchor()
        {
            if (WorldAnchorManager.Instance != null)
            {
                Debug.Log("[ShadowSpatialAnchor] Remove anchor " + this.gameObject.name + " @ " + this.gameObject.transform.position);
                //WorldAnchorManager.Instance.RemoveAnchor(this.gameObject.name);
                DestroyImmediate(anchor); 
            }
            LogAnchorsInfo(); 
        }

        // Debug message for WorldAnchor objects/components in scene 
        public void LogAnchorsInfo()
        {
            var anchors = FindObjectsOfType<WorldAnchor>();
            Debug.Log("[ShadowSpatialAnchor] Total num anchors: " + anchors.Length);
            string ids = ""; 
            for (int i=0; i < anchors.Length; i++ ) { 
                ids += anchors[i].name + ", ";
                if (i % 5 == 0) ids += "\n";
            }
            Debug.Log("[ShadowSpatialAnchor] Anchor names: " + ids);
        }
    }
}

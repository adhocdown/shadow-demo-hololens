using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;



public class ToggleIntensityAll : MonoBehaviour, IInputClickHandler
{
    public GameObject Cubes; 
    public bool isbright;   

    public MadisonShadowManager madsm;
    public DarkShadowManager darksm;
    public NegativeShadowManager negsm;
    public LargeNegativeShadowManager largensm; 


    void Awake()
    {   
        isbright = false;
        Cubes = GameObject.FindGameObjectWithTag("bossShadowManager");        

        print(Cubes.transform.GetChild(2).name); 
        madsm = Cubes.transform.GetChild(0).GetComponent<MadisonShadowManager>();
        darksm = Cubes.transform.GetChild(1).GetComponent<DarkShadowManager>();
        negsm = Cubes.transform.GetChild(2).GetComponent<NegativeShadowManager>();
        largensm = Cubes.transform.GetChild(3).GetComponent<LargeNegativeShadowManager>();

    }

    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        isbright = !isbright;
        madsm.toggleActiveColors(isbright);
        darksm.toggleActiveColors(isbright);
        negsm.toggleActiveColors(isbright);
        largensm.toggleActiveColors(isbright);

        if (isbright)
            GetComponent<TextMesh>().text = "High Intensity";
        else
            GetComponent<TextMesh>().text = "Low Intensity";

    }   

   
}

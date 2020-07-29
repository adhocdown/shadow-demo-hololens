using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UpdateText4Experiment : MonoBehaviour {

    public string myText;
    public ShadowManager4Experiment shadowManager; // consider changing this for consistency across scripts 
    public DataManager dataManager;
    int subjectIdNum;


    // Use this for initialization
    void Awake () {
        myText = transform.GetChild(0).transform.GetComponent<Text>().text;
        subjectIdNum = 0;
    }

    // Text for Subject ID Buttons 
    // < and > buttons to select number for subject id
    public void SetSubjectId()
    {
        var dString = string.Format("{0}", subjectIdNum);
        myText = dString;
        UpdateMyText();
    }
    public void AddToSubjectIdNum()
    {
        subjectIdNum += 1;
        SetSubjectId(); 
    }
    public void SubtractFromSubjectIdNum()
    {
        subjectIdNum -= 1;
        SetSubjectId(); 
    }

    // Dropdown menu is handled by Unity's Event Sytsem

    // Height of object from anchor origin
    public void SetSurfaceHeightText()
    {        
        string dString = string.Format("{0:N3}", shadowManager.GetSurfaceHeight());
        myText = "Surf Height | " + dString;
        UpdateMyText();
        UpdateMySlider((shadowManager.GetSurfaceHeight() + 0.1f) / 0.2f);

    }

    // Text for Start/Anchor
    public void SetStartButtonText(float distance)
    {
        UpdateMyText();
    }

    public void UpdateMySlider(float val)
    {
        transform.GetComponent<Scrollbar>().value = val;
    }
    
    // Break Text Functions 
    public void StartBreakText(int trial)
    {
        myText = "Are you ready to begin?";
        UpdateMyText();
    }

    public void UpdateBreakText(int trial)
    {
        myText = "[ Trial  "+ trial +" completed ] \n"
            + "Are you ready to continue?"; 
        UpdateMyText(); 
    }


    // Misc Functions 

    public void UpdateInstructionText(int prevBlockNum, int nextBlockNum)
    {
        myText = "GOOD JOB! You completed EXP Block " + prevBlockNum
            + "\nGive your HoloLens to Haley \nfor EXP Block " + nextBlockNum;
        UpdateMyText();
    }

    public void SetFinalText()
    {
        myText = "CONGRATULATIONS! \nYOU'RE DONE!";
        UpdateMyText();
    }

    public void UpdateTrialText(int blockNum, int trial, int subTrial)
    {
        myText = "  BLOCK  " + blockNum + "  \t"
            + "[ Trial : " + trial + "/24\t\t"
            + "SubTrial : " + subTrial + "/8 ]";
        UpdateMyText();
    }

    // Update the UI !   called on button clicks 
    void UpdateMyText()
    {
        transform.GetChild(0).transform.GetComponent<Text>().text = myText;
    }

}

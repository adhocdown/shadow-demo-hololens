using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManagement4Experiment : MonoBehaviour {


    public List<Transform> MainMenuChildren;
    public SetWorldAnchor4Experiment WorldAnchor;
    private bool isInitialInput;

    // UI Manager
    //      -- (0) Menu
    //      -- (1) User Input
    //      -- (2) Break

    void Start()
    {

        foreach (Transform child in transform.GetChild(0))
            MainMenuChildren.Add(child);

        InitialSetup();
    }


    // Called on Start button & in Data Manager (between each EXP block) 
    // if isExperimentOngoing (user input = active & main menu = inactive)
    // if !isExperimentOngoing (user input = inactive & main menu = active) 
    public void ToggleMenu(bool isExperimentOngoing)
    {
        transform.GetChild(0).gameObject.SetActive(!isExperimentOngoing);
        transform.GetChild(1).gameObject.SetActive(isExperimentOngoing);
        transform.GetChild(2).gameObject.SetActive(false);
    }

    public void ResetAnchorMenu()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        ShowAnchorResetUI(); 
    }

    public void BreakMenu()
    {
        transform.GetChild(0).gameObject.SetActive(false); // (0) Main 
        transform.GetChild(1).gameObject.SetActive(false); // (1) User Input
        transform.GetChild(2).gameObject.SetActive(true);  // (2) Break
    }

    // Called on Start 
    private void InitialSetup()
    {
        MainMenuChildren[1].gameObject.SetActive(false); // hide (1) Set Anchor Button
        MainMenuChildren[2].gameObject.SetActive(false); // hide (2) Instruction Text 

        ToggleMenu(false);
        isInitialInput = true;
    }



    // Hide Initial UI Manager GO after Experiment has started
    // Show Instructions & Anchor Reset between EXP blocks
    public void ShowAnchorResetUI() 
    {
        MainMenuChildren[0].gameObject.SetActive(false); // hide (0) Initial UI Manager
        MainMenuChildren[1].gameObject.SetActive(true); // show (1) Set Anchor Button
        MainMenuChildren[2].gameObject.SetActive(true); // show (2) Instruction Text
        WorldAnchor.SetAnchor(); // unset anchor 
    }

    public void SetFinalInstruction()
    {
        BreakMenu();
        transform.GetChild(2).GetChild(0).gameObject.SetActive(false); // Break -> (0) Start Trial Button


        /*foreach (Transform UIChild in MainMenuChildren)
            UIChild.gameObject.SetActive(false);
        MainMenuChildren[0].gameObject.SetActive(true); // Subtrail counter
        MainMenuChildren[4].gameObject.SetActive(true); // Subtrail counter
        MainMenuChildren[4].transform.GetComponent<Text>().text = "CONGRATULATIONS! \nYOU'RE DONE!";
        */
    }
}

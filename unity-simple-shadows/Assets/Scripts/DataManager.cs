using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using System.IO;
using System;

public class DataManager : MonoBehaviour
{

    public UpdateText4Experiment trialText;
    public UpdateText4Experiment breakText;
    public UpdateText4Experiment instructText;

    public UIManagement4Experiment UIManager;
    public AudioManager audioManager;

    // UI / Filewriter vars
    public string filename;
    //public GameObject StartUI;
    public List<GameObject> inputFields;
    //private StreamWriter sw; 

    // Subject vars to write 
    public string subjectId;
    string deviceId;
    //string height;
    //string age;
    //string gender;
    public string blockOrder;


    public int[,] randomized_vars;
    public string[,] recorded_vars;

    private int prev_block; // meh
    public int cur_block;
    public int cur_trial;
    public int cur_subtrial;

    bool isExperimentStarted;
    bool isNextTrial;
    private float time;

    public GameObject MyShadowManagerGO; // assign in inspector *shrug*
    ShadowExperimentManager MyShadowManager;
    ShadowManager4Experiment MyShadowManager2; // wow. terrible names for scripts, past self. 


    void Start()
    {

        subjectId = "";
        deviceId = "";
        /*age = "0";
        height = "";
        gender = "";
        */
        blockOrder = "";

        // Get UI gameojbect reference
        //StartUI = GameObject.FindGameObjectWithTag("UI");
        MyShadowManager = MyShadowManagerGO.transform.GetComponent<ShadowExperimentManager>();
        MyShadowManager2 = MyShadowManagerGO.transform.GetComponent<ShadowManager4Experiment>();
        MyShadowManagerGO.SetActive(false); 
        isExperimentStarted = false;

        prev_block = 0; 
        cur_subtrial = 0; // max = 8
        cur_trial = 0; // max = 240                         


    }


    // Set filename and path.
    // Create file if it doesn't exist.
    // Toss error if file does exist. 
    // ughh what a train wreck. this does more than create a file at this point. 
    public void CreateFile()
    {
        if (!isExperimentStarted)
        {
            SetFields();
            filename = Path.Combine(Application.persistentDataPath, deviceId + "_" + subjectId + "_" + blockOrder + ".txt");
            // just  for debugging on computer vv 
            //filename = Application.dataPath + "/Data/contact_" + deviceId + "_" + subjectId + "_" + blockOrder + ".txt"; // use subj & exp ids

            Debug.Log(filename);

            Debug.Log("Start the Experiment.");
            // Create a file to write to 
            using (StreamWriter sw = File.CreateText(filename))
            {
                // Experiment Information 
                sw.WriteLine("SubjectID, DeviceId, BlockOrder");
                sw.WriteLine(subjectId + ", " + deviceId + ", " + blockOrder);

                // Data Labels 
                sw.WriteLine("Block, RepetitionIndex, RandomizedTrials [[shadow_condition, height_condition, likert_rating, elapsed_time], ...]");
                //sw.Dispose();
            }
        }
        StartBlockBreak(); 
    }



    public void StartBlockBreak()
    {
        isNextTrial = false;
        MyShadowManagerGO.SetActive(false);
        audioManager.MuteBackgroundMusic(false);

        trialText.UpdateTrialText(cur_block, cur_trial, cur_subtrial+1);        
        breakText.StartBreakText(cur_trial);

        //UIManager.ToggleMenu(false); 
        if (cur_trial > 1)
        {
            if (cur_trial > 24)
                EndExperiment();
            else
            {
                CheckForNextExperimentBlock(cur_trial);
                instructText.UpdateInstructionText(prev_block, cur_block);
                UIManager.ResetAnchorMenu(); // show anchor reset menu for next EXP block 
                Debug.Log("RESET MY FUKIN ANCHOR");
            }
        }
        //CheckForNextExperimentBlock(cur_trial);
       
    }

    public void StartTrialBreak()
    {
        isNextTrial = false;
        MyShadowManagerGO.SetActive(false); 

        trialText.UpdateTrialText(cur_block, cur_trial, cur_subtrial+1);
        breakText.UpdateBreakText(cur_trial);

        Debug.Log("TRIAL BREAK"); 
        UIManager.BreakMenu();
        //CheckForNextExperimentBlock(cur_trial);

    }

    // manually assign inputFields in inspector (lame, i know)
    public void SetFields()
    {
        subjectId = inputFields[0].transform.GetChild(0).GetComponent<Text>().text;
        blockOrder = inputFields[1].transform.GetChild(0).GetComponent<Text>().text;
        deviceId = "HoloLens";

        cur_block = blockOrder[0] - '0';
        RandomizeTrials();
    }

    public void RandomizeTrials()
    {
        var ar_combinations = new int[,] {
            {0,0},{1,0},{2,0},{3,0},
            {0,1},{1,1},{2,1},{3,1}
        };
        recorded_vars = new string[,] {
            {"0","0"},{"0","0"},{"0","0"},{"0","0"},
            {"0","0"},{"0","0"},{"0","0"},{"0","0"}
        };

        //ArrayPrintCheck("before");
        Shuffle_Matrix(ar_combinations);
        Shuffle_Matrix(ar_combinations);
        randomized_vars = ar_combinations;        
        //ArrayPrintCheck("after"); 
    }

    public void ArrayPrintCheck(string label)
    {
        var array_str = "";
        for (int i = 0; i < 8; i++)
        {
            array_str += "[" + randomized_vars[i, 0] + "," + randomized_vars[i, 1] + "] ";
        }
        Debug.Log(label + ": " + array_str);
    }


    public static void Shuffle_Matrix(int[,] array)
    {
        int number = 8;
        for (int i = 0; i < number; i++)
        {
            int j = UnityEngine.Random.Range(i, number);

            List<int> temp = new List<int> { array[i, 0], array[i, 1] };
            array[i, 0] = array[j, 0];
            array[i, 1] = array[j, 1];
            array[j, 0] = temp[0];
            array[j, 1] = temp[1];
        }
    }

   void Update()
    {
        time += Time.deltaTime; // reset for every sub trial
        if (Input.anyKeyDown && cur_trial > 24)
        {
            EndExperiment(); 
            return; 
        }/*
        else if (Input.anyKeyDown && isExperimentStarted)
        {
            if (Input.GetKeyDown("n") && isNextTrial)
            {
                NextTrial(); // remove in final for HoloLens
            }
            else if (Input.GetKeyDown("r") && !isNextTrial)
            {
                ResetTrials(); // remove in final for HoloLens
            }
            else if (!isNextTrial && (cur_trial) % 8 == 0)  // if any other key is pressed down (except r) 
            {
                CheckForNextExperimentBlock(cur_trial);
            }


        }*/

    }

    void  EndExperiment()
    {
        isExperimentStarted = false;
        isNextTrial = false; 

        Debug.Log("You're all done!");
        audioManager.MuteBackgroundMusic(false);
        breakText.SetFinalText();
        UIManager.SetFinalInstruction(); 
    }


    public void ResetTrials()
    {
        if (isNextTrial)
            return;

        UpdateRecordedValues(99); // idk
        isExperimentStarted = true;


        RandomizeTrials();
        cur_trial += 1;
        isNextTrial = true;
        cur_subtrial = 0;
        time = 0;


        if (cur_trial > 24)
        {
            EndExperiment();
            return;
        }

        Debug.Log("Trial: " + cur_trial + "/24");

        trialText.UpdateTrialText(cur_block, cur_trial, cur_subtrial+1);
        breakText.UpdateBreakText(cur_trial);

        if (cur_subtrial == 0)
        {
            MyShadowManagerGO.SetActive(true);
            UpdateShaders();
        }

    }


    public void NextTrial()
    {
        if (!isNextTrial || !isExperimentStarted)
            return;

        if (cur_subtrial >= 7)
        {
            isNextTrial = false;
            MyShadowManagerGO.SetActive(false);
        }

    }
    

    public void UpdateRecordedValues(int likert_num)
    {
        if (cur_subtrial > 7)
            isNextTrial = false; 

        


        Debug.Log("SubTrial: " + cur_subtrial + "/8");
        Debug.Log(likert_num + " , " + time); 

        recorded_vars[cur_subtrial, 0] = likert_num.ToString();
        recorded_vars[cur_subtrial, 1] = time.ToString();
        time = 0;
        NextTrial();
        cur_subtrial += 1;

        if (cur_subtrial <= 7)
        {
            trialText.UpdateTrialText(cur_block, cur_trial, cur_subtrial+1);
            UpdateShaders();
        }

        if (!isNextTrial && cur_subtrial >= 7)
        {
            WriteTrialInformation();
            Debug.Log("WRITE TRIAL INFORMATION bc subtrial is " + cur_subtrial);
            if (cur_trial >= 24)
            {
                EndExperiment();
                return; 
            }

            if (cur_trial > 0 && (cur_trial % 8 == 0))
            {
                Debug.Log("START NEXT BLOKKKK");
                StartBlockBreak();
            }
            else
                StartTrialBreak();

            cur_subtrial = 0;
            return;
        }
        else if (!isNextTrial)
            return;

    }



    public void CheckForNextExperimentBlock(int next_trial)
    {
        prev_block = cur_block;
        time = 0;
        if (next_trial < 8)         // BLOCK 1
        {           
            cur_block = blockOrder[0] - '0';
        }
        else if (next_trial >= 16)    // BLOCK 3
        {
            cur_block = blockOrder[2] - '0';
        }
        else                        // BLOCK 2
        {                           // else (next_trial > 8 && next_trial <= 16)
            cur_block = blockOrder[1] - '0';
        }

        string position_string;
        switch (cur_block)
        {
            case 1:
                position_string = "TABLE 1m";
                break;
            case 2:
                position_string = "GROUND 1m";
                break;
            case 3:
                position_string = "GROUND 3m";
                break;
            default:
                position_string = "GROUND 1m"; // Should not need use
                break;
        }
        Debug.Log("CHANGE BLOCK [" + cur_block + "] -> MOVE TO " + position_string);
        MyShadowManager2.SetPositionRelativeToAnchor(cur_block);



        using (StreamWriter sw = File.AppendText(filename))
        {
            sw.WriteLine(" ");
        }
    }


    // write randomized trial
    void WriteTrialInformation()
    {
        var array_str = "[";
        for (int i = 0; i < 8; i++)
        {
            array_str += "[" + randomized_vars[i, 0] + "," + randomized_vars[i, 1] + 
                "," + recorded_vars[i, 0] + "," + recorded_vars[i, 1] +
                "], ";
        }
        array_str += "]";

        Debug.Log(array_str); 

        using (StreamWriter sw = File.AppendText(filename))
        {
            sw.WriteLine(cur_block + ", " + cur_trial + ", " + array_str);
            //sw.WriteLine(" ");
            //sw.WriteLine(condition);
        }
        
    }

    // index into array take subtrial - 1
    public void UpdateShaders()
    {
        MyShadowManager.SetCubeHeight(randomized_vars[cur_subtrial, 1], cur_block);
        MyShadowManager.SetShadowMaterials(randomized_vars[cur_subtrial, 0]);
        MyShadowManager2.SetPositionRelativeToAnchor(cur_block);
    }

}
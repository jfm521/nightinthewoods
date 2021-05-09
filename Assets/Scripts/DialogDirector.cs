using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using NPCCharacters; //?
public enum characters
{
    Angus,
    Gregg,
    Bea,
    Frog,
}
public class DialogDirector : MonoBehaviour
{
    // Start is called before the first frame update
    public DialogManager dialogManager;
    public GameObject angusObj;
    public GameObject angusObjTrigger;
    public NPCAngus angusDialog;
    public NPCMove angusMove;
    public GameObject angusObject;
    public bool autoStart;
    public int autoMostDist;

    public bool isTalking = false; // Is talking is set in Dialog Manager.StartTalking() and EndTalking()
    public bool inCutscene = false;
    void Start()
    {
        angusObjTrigger = FindGameObjectInChildWithTag(angusObj,"TalkTrigger");
        angusDialog = GetComponent<NPCAngus>();
        angusMove = angusObject.GetComponent<NPCMove>();   
        if(autoStart)
        {
            //StartCutscene();
            //CheckAutoMove(characters.Angus,0,autoMostDist);
        }
    }
    void FixedUpdate()
    {
        /*Debug.Log(angusDialog.plotProg[plotKey.DialogIndex]);
        if(angusDialog.plotProg[plotKey.SectionIndex] == 0)
        {
            switch(angusDialog.plotProg[plotKey.DialogIndex])
            {
                case(0):
                {
                    if(angusObj.transform.position.x<1)//angusMove.hasArrived)
                    {
                        ProgressPlot(characters.Angus);
                    }
                    break;
                }
                case(1):
                {
                    CheckAutoTalk(characters.Angus,0);
                    break;
                }
                case(2):
                {
                    if(!isTalking)
                    {
                        CheckAutoMove(characters.Angus,0,2,-1);
                        ProgressPlot(characters.Angus);
                    }
                    break;
                }
                case(3):
                {
                    if(!isTalking)
                    {
                        CheckAutoMove(characters.Angus,0,2,-1);
                        ProgressPlot(characters.Angus);
                    }
                    break;
                }
            }
        }*/
    }

    public void StartCutscene()
    {
        inCutscene = true;
        dialogManager.inCutscene = true;
    }
    public void EndCutscene()
    {
        inCutscene = false;
        dialogManager.inCutscene = false;
    }
    public void ProgressPlot(characters character) //This should have more parameters to determine whoes prog
    { 
        Debug.Log("PP in DD");
        switch(character)
            {
                case(characters.Angus): 
                {
                    angusDialog.ProgressPlot();
                    break;
                }
            }
    }
    public void ProgressPlot(characters character, string branch) //This should have more parameters to determine whoes prog
    { 
        Debug.Log("PP in DD (branch)");
        switch(character)
            {
                case(characters.Angus): 
                {
                    angusDialog.ProgressPlot(branch);
                    break;
                }
            }
    }
    public void CheckAutoMove(characters character,int sectionValue,int walkDist) //Check (Section) and Automatically start a conversation
    {
        switch(character){
            case(characters.Angus): {
                if(angusDialog.plotProg[plotKey.SectionIndex] == sectionValue)
                {
                    angusMove.WalkTowards(walkDist);
                }
                break;
            }
        }
    }
    public void CheckAutoMove(characters character,int sectionValue, int dialogValue, int walkDist) //Check (Section) and Automatically start a conversation
    {
        switch(character){
            case(characters.Angus): {
                if(angusDialog.plotProg[plotKey.SectionIndex] == sectionValue
                    &&angusDialog.plotProg[plotKey.DialogIndex] == dialogValue)
                {
                    angusMove.WalkTowards(walkDist);
                }
                break;
            }
        }
    }

    public void AutoTalk(characters character) //Check (Section) and Automatically start a conversation
    {
        if(!isTalking){
            switch(character){
                case(characters.Angus): {
                    dialogManager.AutoTalking(angusObjTrigger);
                    break;
                }
            }
        }
    }
    public void CheckAutoTalk(characters character,int sectionValue) //Check (Section) and Automatically start a conversation
    {
        if(!isTalking){
            switch(character){
                case(characters.Angus): {
                    if(angusDialog.plotProg[plotKey.SectionIndex] == sectionValue)
                    {
                        dialogManager.AutoTalking(angusObjTrigger);
                    }
                    break;
                }
            }
        }
    }
    public void CheckAutoTalk(characters character,int sectionValue,int dialogValue) //Check (Section&Dialog) and Automatically start a conversation
    {
        if(!isTalking){
            switch(character){
                case(characters.Angus): {
                    if(angusDialog.plotProg[plotKey.SectionIndex] == sectionValue
                    &&angusDialog.plotProg[plotKey.DialogIndex] == dialogValue)
                    {
                        dialogManager.AutoTalking(angusObjTrigger);
                    }
                    break;
                }
            }
        }
    }
    public static GameObject FindGameObjectInChildWithTag (GameObject parent, string tag)
     {
         Transform t = parent.transform;
 
         for (int i = 0; i < t.childCount; i++) 
         {
             if(t.GetChild(i).gameObject.tag == tag)
             {
                 return t.GetChild(i).gameObject;
             }
                 
         }
             
         return null;
     }
}

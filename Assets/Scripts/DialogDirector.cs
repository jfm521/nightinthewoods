using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using NPCCharacters; //?
public enum characters
{
    Angus,
    Gregg,
    Bea,
    Frog,
    None
}
public class DialogDirector : MonoBehaviour
{
    // Start is called before the first frame update
    public DialogManager dialogManager;

    [HideInInspector]
    public GameObject angusObjTrigger;

    [HideInInspector]
    public NPCAngus angusDialog;

    [HideInInspector]
    public NPCMove angusMove;
    public GameObject angusObject;
    public bool autoStart;
    public int autoMostDist;

    public bool isTalking = false; // Is talking is set in Dialog Manager.StartTalking() and EndTalking()
    public bool inCutscene = false;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        angusObjTrigger = FindGameObjectInChildWithTag(angusObject,"TalkTrigger");
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
        if(angusDialog.dialogArrSectionName[angusDialog.plotProg[plotKey.SectionIndex]].Substring(0,6)=="<Load>")
        {
            angusObject.GetComponent<NPCMove>().EnterTopDownPosition();
            dialogManager.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            angusDialog.ProgressPlot();
            SceneManager.LoadScene(angusDialog.dialogArr[angusDialog.plotProg[plotKey.SectionIndex],angusDialog.plotProg[plotKey.DialogIndex]]);
        }
            
            /*
            switch(angusDialog.plotProg[plotKey.DialogIndex])
            {
                case(0):
                {
                    CheckAutoTalk(characters.Angus,0);
                    break;
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

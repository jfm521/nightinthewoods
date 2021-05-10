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
    public static DialogManager dialogManager;

    [HideInInspector]
    public static GameObject angusObjTrigger;

    [HideInInspector]
    public NPCAngus angusDialog;

    [HideInInspector]
    public NPCMove angusMove;
    public GameObject angusObject;
    public bool autoStart;
    public int autoMostDist;
    public bool onBranch;

    public static bool isTalking = false; // Is talking is set in Dialog Manager.StartTalking() and EndTalking()
    public static bool inCutscene = false;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Start()
    {
        dialogManager = GameObject.Find("DialogPlayer").GetComponent<DialogManager>()  ;
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
        if(NPCAngus.dialogArrSectionName[NPCAngus.plotProg[plotKey.SectionIndex]].Substring(0,6)=="(Load)")
        {
            if(NPCAngus.plotProg[plotKey.SectionIndex] == 1)
            {
                SceneManager.LoadScene(NPCAngus.dialogArr[NPCAngus.plotProg[plotKey.SectionIndex],NPCAngus.plotProg[plotKey.DialogIndex]]);
            }
        }
        angusObject.GetComponent<Animator>().SetBool("isTalking", isTalking);
        /*if(NPCAngus.plotProg[plotKey.SectionIndex] == 2)
        {
            CameraStars.isCutscene = true;
            CameraStars.GoTo(GameObject.Find("Constellation 1").transform.position);
        }*/
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(NPCAngus.dialogArrSectionName[NPCAngus.plotProg[plotKey.SectionIndex]].Substring(0,6)=="(Load)")
        {
            //Camera.main.GetComponent<CameraFollow>().isCutscene = true;
            angusMove.StartTopDown();
            
            dialogManager.StartTopDown(GameObject.Find("MaePos").transform.position);
            ProgressPlot(characters.Angus);
            AutoTalk(characters.Angus);
            onBranch = true;
        }
    }
    public static void StartCutscene()
    {
        inCutscene = true;
        dialogManager.inCutscene = true;
    }
    public static void EndCutscene()
    {
        inCutscene = false;
        dialogManager.inCutscene = false;
    }
    public static void ProgressPlot(characters character) //This should have more parameters to determine whoes prog
    { 
        Debug.Log("PP in DD");
        switch(character)
            {
                case(characters.Angus): 
                {
                    NPCAngus.ProgressPlot();
                    break;
                }
            }
    }
    public static void ProgressPlot(characters character, string branch) //This should have more parameters to determine whoes prog
    { 
        Debug.Log("PP in DD (branch)");
        switch(character)
            {
                case(characters.Angus): 
                {
                    NPCAngus.ProgressPlot(branch);
                    break;
                }
            }
    }
    public void CheckAutoMove(characters character,int sectionValue,int walkDist) //Check (Section) and Automatically start a conversation
    {
        switch(character){
            case(characters.Angus): {
                if(NPCAngus.plotProg[plotKey.SectionIndex] == sectionValue)
                {
                    angusMove.WalkTowards(walkDist);
                }
                break;
            }
        }
    }
    public void AutoMove(characters character, int walkDest) //Check (Section) and Automatically start a conversation
    {
        switch(character){
            case(characters.Angus): {
                angusMove.WalkTowards(walkDest);
                break;
            }
        }
    }

    public static void AutoTalk(characters character) //Check (Section) and Automatically start a conversation
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
    public static void AutoTalkCam(characters character) //Check (Section) and Automatically start a conversation
    {
        if(!isTalking){
            switch(character){
                case(characters.Angus): {
                    ProgressPlot(characters.Angus);
                    switch(NPCAngus.dialogArr[NPCAngus.plotProg[plotKey.SectionIndex],
                    NPCAngus.plotProg[plotKey.DialogIndex]])
                    {
                        case("(Pope)"):
                        {
                            CameraStars.GoTo(GameObject.Find("Constellation1").transform.position);
                            break;
                        }
                        case("(Whale)"):
                        {
                            CameraStars.GoTo(GameObject.Find("Constellation2").transform.position);
                            break;
                        }
                        case("(Bell)"):
                        {
                            CameraStars.GoTo(GameObject.Find("Constellation3").transform.position);
                            break;
                        }
                        case("(Thief)"):
                        {
                            CameraStars.GoTo(GameObject.Find("Constellation4").transform.position);
                            break;
                        }
                    }
                    ProgressPlot(characters.Angus);
                    dialogManager.AutoTalking(angusObjTrigger);
                    break;
                }
            }
        }
    }
    public static void AutoTalkCam(characters character, Vector3 camPos) //Check (Section) and Automatically start a conversation
    {
        if(!isTalking){
            switch(character){
                case(characters.Angus): {
                    CameraStars.GoTo(camPos);
                    dialogManager.AutoTalking(angusObjTrigger);
                    break;
                }
            }
        }
    }
    public static void AutoTalk(characters character, string branch) //Check (Section) and Automatically start a conversation
    {
        if(!isTalking){
            switch(character){
                case(characters.Angus): {
                    ProgressPlot(characters.Angus,branch);
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
                    if(NPCAngus.plotProg[plotKey.SectionIndex] == sectionValue)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPCCharacters; //?

public class DialogDirector : MonoBehaviour
{
    // Start is called before the first frame update
    public DialogManager dialogManager;
    public GameObject angusObj;
    public NPCAngus angusDialog;

    public NPCMove angusMove;
    public GameObject angusObject;

    public bool isTalking = false; // Is talking is set in Dialog Manager.StartTalking() and EndTalking()
    void Start()
    {
        angusDialog = GetComponent<NPCAngus>();

        angusMove = angusObject.GetComponent<NPCMove>();

        angusMove.WalkTowards(-20);
    }

    // Update is called once per frame
    void FixedUpdate() //I think NPC Walking code should go here
    {
        if(!isTalking){
            // if plot value is 0 angus talk about stuff automatically.
            // passive dialogs are handled through direct interaction between Dialog Manager and NPCTalktive.
            if(angusDialog.plotProg == 0)
            {
                dialogManager.StartTalking(angusObj); // Start Talking will instantly start talking
            }
            // My idea is have a angus object that has both NPCTalktive and a movement code
            // The movement code would have a public function called "WalkTowards(vector2) or something like that"
            // Reference that movement code component here as angusMove (public)
            // and in here you can do angusMove.WalkTowards()
        }
    }

    public void ProgressPlot() //This should have more parameters to determine whoes prog
    { //Right now this method only add plotprog by 1
        Debug.Log("PP");
        if(angusDialog.plotProg == 0)
        {
            Debug.Log("Plot+1");
            angusDialog.plotProg += 1;
        }
    }
}

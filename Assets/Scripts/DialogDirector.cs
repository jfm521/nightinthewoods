using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPCCharacters; //?

public class DialogDirector : MonoBehaviour
{
    // Start is called before the first frame update
    SpeechBubbleManager sbManager;
    GameObject angusPlace;
    NPCAngus angus;
    void Start()
    {
        sbManager = GameObject.Find("MaeDialogPlace").GetComponent<SpeechBubbleManager>();
        angusPlace = GameObject.Find("AngusDialogPlace");
        angus = GetComponent<NPCAngus>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(angus.plotProg == 0)
        {
            sbManager.StartTalking(angusPlace);
        }
    }

    public void ProgressPlot() //This should have more parameters 
    {
        Debug.Log("PP");
        if(angus.plotProg == 0)
        {
            Debug.Log("Plot+1");
            angus.plotProg += 1;
        }
    }
}

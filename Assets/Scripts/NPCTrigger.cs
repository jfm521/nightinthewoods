using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [Header ("Action")]
    public bool stop;
    public bool move;
    public bool talk;
    public bool moveAfterTalk;
    public GameObject moveTo;
    [Header ("Plot Progression")]
    public bool progPlot;
    public bool progPlotBranch;
    public string plotBranch;
    [Header ("Cutscene")]
    public bool startCutscene;
    public bool endCutscene;
    void Start()
    {
        
    }

    // Update is called once per frame
}

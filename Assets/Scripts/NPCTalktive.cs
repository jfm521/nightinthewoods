using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPCCharacters;
public class NPCTalktive : MonoBehaviour
{
    // Start is called before the first frame update
    private string NPCName;
    public enum characters
    {
        Angus,
        Gregg,
        Bea,
        Frog,
    }
    public characters characterSelect = characters.Angus;
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string GetDialogPath()
    {
        Debug.Log("Get Path");
        if(characterSelect == characters.Angus)
        {
            Debug.Log("Get Angus");
            return GameObject.Find("DialogDirector").GetComponent<NPCAngus>().CheckPlotProg();
        }
        return "Error in GetDialogPath";
    }
}
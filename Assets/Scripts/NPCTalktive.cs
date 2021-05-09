using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using NPCCharacters;
public class NPCTalktive : MonoBehaviour
{
    // Start is called before the first frame update
    private string NPCName;
    public bool isTalking;
    public bool talkable;
    public characters characterSelect = characters.Angus; //characters are an Emun defined in DialogDirector

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public string GetDialogPath()
    {
        Debug.Log("Get Path");
        if(characterSelect == characters.Angus)
        {
            Debug.Log("Get Angus");
            return GameObject.Find("DialogDirector").GetComponent<NPCAngus>().GetDialogPath();
        }
        return "Error in GetDialogPath";
    }
}
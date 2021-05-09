using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class DialogManager : MonoBehaviour
{

    //gameobject stuff to turn bubble + text sprites on and off
    public GameObject dialogPrompt, dialogBox;
    private DialogDirector dialogDirector;
    public Vector3 dialogBoxOffSet = new Vector3(0,-10,0);
    public GameObject talkingObj;
    public characters talkingCharacter;
    //dialogBox has to be set in inspector

    //visibility checkers
    bool talkReady = false; //if player is touching npc
    GameObject touchingObj;
    bool canContinue = false; //if text has finished and can continue
    bool isTalking = false;
    public bool inCutscene = false;

    //these are all the text + sentence variables
    public TextMeshProUGUI textDisplay;
    private float typingSpeedNormal = 0.04f;
    private float typingSpeedFast = 0.008f; 
    private float typingSpeed; //typing speed set in start, spaghetti typing speed solution
    private string dialogPath = "None";
    private StreamReader reader;
    string lineText = "-";
    public Color[] colorArr = new Color[4];//Colors of text for characters

    //sounds
    AudioSource talkAudSource;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        talkAudSource = GetComponent<AudioSource>();
        
        dialogDirector = GameObject.Find("DialogDirector").GetComponent<DialogDirector>();
        dialogBox.SetActive(false);
        dialogPrompt.SetActive(false);

        typingSpeed = typingSpeedNormal; //set default typing speed

        colorArr[0] = new Color(0,0,0);
        colorArr[1] = new Color(0.8f,0.3f,0.086f); //Hard Coded colors of text for different charas
    }

    void OnTriggerStay2D(Collider2D other){ 
        if(!inCutscene)
        {
            if(other.tag == "TalkTrigger"){ //if player touches npc collider
                Debug.Log("touching NPC");
                PreTalking(other.gameObject);
            }
            if(other.tag == "PlayerTrigger")
            {
                PlayerTrigger trigger = other.gameObject.GetComponent<PlayerTrigger>();
                if(trigger.talk){
                    switch(trigger.talkTo){
                        case(characters.Angus):
                        {
                            AutoTalking(GameObject.Find("StockAngus").transform.GetChild(0).gameObject); //I didn't have more time to make a character finding system, so be careful when changing Angus'name!
                            //touchingObj = GameObject.Find("StockAngus");
                            break;}
                    }   
                }
                if(trigger.startCutscene)
                    dialogDirector.StartCutscene();
                else if(trigger.endCutscene)
                    dialogDirector.EndCutscene();
            }
        }
    }
    void PreTalking(GameObject obj)
    {
        if(!talkReady){
            dialogPrompt.SetActive(true);
            dialogPrompt.transform.position = obj.transform.position + dialogBoxOffSet;
            
            
            //Prepare to talk
            talkReady = true;
            touchingObj = obj;
            talkingCharacter = touchingObj.GetComponent<NPCTalktive>().characterSelect;
            dialogPath = touchingObj.GetComponent<NPCTalktive>().GetDialogPath();
            //Debug.Log(talkingCharacter);
        }
    }
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.X)){
            if(canContinue){
                NextSentence();
                Debug.Log("Next Sentence");
            }
            else if(talkReady && !isTalking)
                StartTalking();
        }

        if(dialogDirector.isTalking)
            dialogBox.transform.position = talkingObj.transform.position + dialogBoxOffSet;

        if(Input.GetKey(KeyCode.X)) //Lazy method for speeding up text
            typingSpeed = typingSpeedFast;
        else
            typingSpeed = typingSpeedNormal;
    }
    void StartTalking()
    {
        reader = new StreamReader(dialogPath); //Reader read dialog txt
        NextSentence();

        dialogBox.SetActive(true);
        dialogPrompt.SetActive(false);
        
        talkingObj = touchingObj;
        dialogDirector.isTalking = true;
        talkingCharacter = touchingObj.GetComponent<NPCTalktive>().characterSelect;
        dialogDirector.ProgressPlot(talkingCharacter);

        //sound stuff
        talkAudSource.clip = clip;
        talkAudSource.Play();

        isTalking = true;
        //touchingObj.transform.parent.GetComponent<AngusAnimation>().isTalking = true; UNCOMMENT THIS FOR ANGUS AMINE, there are 2 of these
    }
    // If isTalking code here is not working, put this in AngusAnimation's FixedUpdate:
    //isTalking = GameObject.Find("DialogDirector").GetComponent<DialogDirector>().isTalking;

    public void AutoTalking(GameObject obj)
    {
        PreTalking(obj);
        StartTalking();
        Debug.Log("AutoTalking");
    }
    IEnumerator Type(){
        foreach(char letter in lineText.ToCharArray()){ //gets each letter in sentence
            textDisplay.text += letter; //adds each letter to text
        if(textDisplay.text == lineText){ //once text has finished completely
            canContinue = true;
            talkAudSource.Stop();
        }
            yield return new WaitForSeconds(typingSpeed); //sets speed of scroll
        }
    }

    public void NextSentence(){
        canContinue = false; //stops the player from going to the next sentence before its done

        lineText = reader.ReadLine(); //get next sentence
        if(lineText != "<END>"){ 
            if(lineText.Substring(0,2) == "M:") //Mae's line
            {
                talkingObj = gameObject;
                textDisplay.color = colorArr[0];
            }
            else if(lineText.Substring(0,2) == "A:") //Angus's line
            {
                talkingObj = touchingObj; //ANGUS
                textDisplay.color = colorArr[1];
            }
            lineText = lineText.Substring(2);

            textDisplay.text = ""; //set text back to nothing
            StartCoroutine(Type()); //start typing new sentences
            talkAudSource.clip = clip;
            talkAudSource.Play();
        }
        else { //End of dialog
            reader.Close(); //Close reader
            canContinue = false;
            //hide the big bubble too
            dialogBox.SetActive(false);
            EndTalking();
        }
    }


    void OnTriggerExit2D(Collider2D other){ 
        if(!inCutscene){
            if(other.gameObject.tag == "TalkTrigger"){ //if player is no longer touching the npc collider
                touchingObj = null;    
                talkReady = false;
                dialogPrompt.SetActive(false);
                Debug.Log("not touching NPC anymore");
                EndTalking();
            }
        }
    }

    public void EndTalking()
    {
        
            //Reset talk and change plotprog value
            dialogPath = "None";
            
            talkingObj = null;
            isTalking = false;
            //touchingObj.transform.parent.GetComponent<AngusAnimation>().isTalking = false; UNCOMMENT THIS FOR ANGUS AMINE, there are 2 of these
            talkReady = false;

            dialogBox.SetActive(false);
            textDisplay.text = "";
            lineText = "";
            dialogDirector.isTalking = false;
    }
}


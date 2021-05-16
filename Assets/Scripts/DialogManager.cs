using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class DialogManager : MonoBehaviour
{

    //gameobject stuff to turn bubble + text sprites on and off
    public GameObject dialogPrompt, dialogBox, dialogCanvas;
    private DialogDirector dialogDirector;
    [HideInInspector]public Vector3 dialogBoxOffSet = new Vector3(0,5,0); // jackie made this public
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
    private float typingSpeedNormal = 0.044f;
    private float typingSpeedFast = 0.0008f; 
    private float typingSpeed; //typing speed set in start, spaghetti typing speed solution
    private string dialogPath = "None";
    private StreamReader reader;
    string lineText = "-";
    int currChoice = 0;
    string[] choiceArr = new string[1];
    bool isChoosing = false;
    public Color[] colorArr = new Color[4];//Colors of text for characters

    //sounds
    AudioSource talkAudSource;
    public AudioClip clip;
    //Animation stuff
    public Animator animator;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(dialogCanvas);
    }
    
    void Start()
    {
        talkAudSource = GetComponent<AudioSource>();
        
        dialogDirector = GameObject.Find("DialogDirector").GetComponent<DialogDirector>();
        dialogBox.SetActive(false);
        dialogPrompt.SetActive(false);

        typingSpeed = typingSpeedNormal; //set default typing speed

        colorArr[0] = new Color(1,1,1);
        colorArr[1] = new Color(0.8f,0.3f,0.086f); //Hard Coded colors of text for different charas
    }

    void OnTriggerStay2D(Collider2D other){ 
        if(!inCutscene)
        {
            if(other.tag == "TalkTrigger"){ //if player touches npc collider
                Debug.Log("touching NPC");
                if(other.GetComponent<NPCTalktive>().talkable)
                    PreTalking(other.gameObject);
            }
            if(other.tag == "PlayerTrigger")
            {
                PlayerTrigger trigger = other.gameObject.GetComponent<PlayerTrigger>();
                if(trigger.talk){
                    switch(trigger.talkTo){
                        case(characters.Angus):
                        {
                            AutoTalking(DialogDirector.FindGameObjectInChildWithTag(dialogDirector.angusObject,"TalkTrigger")); 
                            //touchingObj = GameObject.Find("StockAngus");
                            break;}
                    }   
                }
                if(trigger.startCutscene)
                    DialogDirector.StartCutscene();
                else if(trigger.endCutscene)
                    DialogDirector.EndCutscene();
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

        if(DialogDirector.isTalking)
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
        DialogDirector.isTalking = true;
        //talkingCharacter = touchingObj.GetComponent<NPCTalktive>().characterSelect;
        

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
                animator.SetBool("IsTalking", true);
                animator.SetBool("AngusTalking", false);

            }
            else if(lineText.Substring(0,2) == "A:") //Angus's line
            {
                talkingObj = touchingObj; //ANGUS
                textDisplay.color = colorArr[1];
                animator.SetBool("IsTalking", false);
                animator.SetBool("AngusTalking", true);
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
            animator.SetBool("IsTalking", false);
            animator.SetBool("AngusTalking", false);
        }
    }


    void OnTriggerExit2D(Collider2D other){ 
        if(!inCutscene){
            if(other.gameObject.tag == "TalkTrigger"){ //if player is no longer touching the npc collider
                touchingObj = null;    
                talkReady = false;
                dialogPrompt.SetActive(false);
                Debug.Log("not touching NPC anymore");
            }
        }
    }
    public void EndTalking()
    {
        
            //Reset talk and change plotprog value
            dialogPath = "None";

            if(!dialogDirector.onBranch)
                DialogDirector.ProgressPlot(talkingCharacter);
            else{
                if(NPCAngus.plotProg[plotKey.SectionIndex]==2)
                    GameObject.Find("Star Manager").GetComponent<manageStars>().checkAllLinked();
            }
            talkingObj = null;
            isTalking = false;
            //touchingObj.transform.parent.GetComponent<AngusAnimation>().isTalking = false; UNCOMMENT THIS FOR ANGUS AMINE, there are 2 of these
            talkReady = false;
            talkingCharacter = characters.None;

            dialogBox.SetActive(false);
            textDisplay.text = "";
            lineText = "";
            DialogDirector.isTalking = false;
    }
    public void StartTopDown(Vector3 position)
    {   
        transform.position = position;
        transform.parent = GameObject.Find("MaePos").transform;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
        GameObject.Find("Right Arm").GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
        GameObject.Find("Left Arm").GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
        GameObject.Find("Legs").GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
        GameObject.Find("Body").GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
        GameObject.Find("Head").GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
        Reset();
    }
    public void Reset()
    {
        talkingObj = null;
        talkingCharacter = characters.None;
        talkReady = false; //if player is touching npc
        touchingObj = null;
        canContinue = false; //if text has finished and can continue
        isTalking = false;
        inCutscene = false;
        dialogPath = "None";
        reader.Close();
        lineText = "-";
    }
    void StartTopDown()
    {

    }
    void OnDisabled()
    {
        reader.Close();
    }
}


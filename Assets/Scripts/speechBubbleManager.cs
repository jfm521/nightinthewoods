using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class speechBubbleManager : MonoBehaviour
{

    //gameobject stuff to turn bubble + text sprites on and off
    public GameObject miniBubble, bigBubble, sentence, dots, NPCtriangle, playerTriangle, dialogBox;

    public Vector3 dialogBoxOffSet = new Vector3(0,1,0);
    public GameObject talkingObj;
    //dialogBox has to be set in inspector

    //visibility checkers
    bool touched = false; //if player is touching npc
    GameObject touchingObj;
    bool speechVisible = false; //if the bubble and text is now visible, ready to start talking
    bool canContinue = true; //if text has finished and can continue

    //these are all the text + sentence variables
    public TextMeshProUGUI textDisplay, playerTextDisplay;
    private float typingSpeedNormal = 0.04f;
    private float typingSpeedFast = 0.008f;
    private float typingSpeed; //typing speed set in start, spaghetti typing speed solution
    private string dialogPath = "Assets/Dialogs/TestDialog.txt";
    private StreamReader reader;
    string lineText = "-";
    public Color[] colorArr = new Color[4];//Colors of text for characters

    // Start is called before the first frame update
    void Start()
    {

        //makes all the speech bubbles + text invisible
        //playerBubble.SetActive(false);
        //playerSpeech.SetActive(false);
        bigBubble.SetActive(false);
        sentence.SetActive(false);
        miniBubble.SetActive(false);
        dots.SetActive(false);

        typingSpeed = typingSpeedNormal; //set default typing speed

        colorArr[0] = new Color(0,0,0);
        colorArr[1] = new Color(0.8f,0.3f,0.086f); //Hard Coded colors of text for different charas
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)){ //if x is pressed and is touching the npc
            if(touched && !speechVisible)
            {
                reader = new StreamReader(dialogPath); //Reader read dialog txt
                //lineText = reader.ReadLine();

                Debug.Log("show player bubble");

                //show the big bubble
                bigBubble.SetActive(true);
                sentence.SetActive(true);

                //DialogBox Position
                //dialogBox.transform.position = talkingObj.transform.position + dialogBoxOffSet;

                //hide the small bubble
                miniBubble.SetActive(false);
                dots.SetActive(false);
                speechVisible = true;
            }
            if(speechVisible && canContinue){
                //DialogBox Position
                NextSentence();
                dialogBox.transform.position = talkingObj.transform.position + dialogBoxOffSet;
                Debug.Log("speaking");

            }
        }
        if(Input.GetKey(KeyCode.X)) //Lazy method for speeding up text
            typingSpeed = typingSpeedFast;
        else
            typingSpeed = typingSpeedNormal;

        if(textDisplay.text == lineText){ //once text has finished completely
            canContinue = true;
        }

    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "TalkTrigger"){ //if player touches npc collider

            Debug.Log("touched NPC");

            //show the mini bubble
            miniBubble.SetActive(true);
            dots.SetActive(true);

            //Prepare to talk
            touched = true;
            touchingObj = other.gameObject;
            dialogPath = touchingObj.GetComponent<NPCTalktive>().GetDialogPath();
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag == "TalkTrigger"){ //if player is no longer touching the npc collider

            Debug.Log("not touching NPC anymore");

            //hide the mini bubble
            miniBubble.SetActive(false);
            dots.SetActive(false);

            //Reset talk and change plotprog value
            touched = false;
            dialogPath = "None";
            touchingObj.GetComponent<NPCTalktive>().ProgressPlot();
            touchingObj = GameObject.Find("objDummy");

            //hide the big bubble too
            bigBubble.SetActive(false);
            sentence.SetActive(false);
        }
    }

    //THIS IS ALL NPC TALKING STUFF
    IEnumerator Type(){
        foreach(char letter in lineText.ToCharArray()){ //gets each letter in sentence
            textDisplay.text += letter; //adds each letter to text
            yield return new WaitForSeconds(typingSpeed); //sets speed of scroll
        }
    }

    public void NextSentence(){

        Debug.Log("canContinue is: " + canContinue);
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
        }
        else { //End of dialog
            reader.Close(); //Close reader

            //hide the big bubble too
            bigBubble.SetActive(false);
            sentence.SetActive(false);
            canContinue = true;
            speechVisible = false;
        }
    }
}

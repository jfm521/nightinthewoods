using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class speechBubbleManager : MonoBehaviour
{

    //gameobject stuff to turn bubble + text sprites on and off
    public GameObject miniBubble, bigBubble, sentence, dots, NPCtriangle, playerTriangle;

    //dialogue states
    /*bool playerSpoke0 = true;
    bool playerSpoke1 = false;
    bool NPCSpoke1 = false;
    bool playerSpoke2 = false;
    bool NPCSpoke2 = false;*/

    //visibility checkers
    bool touched = false; //if player is touching npc
    bool speechVisible = false; //if the bubble and text is now visible, ready to start talking
    bool canContinue = true; //if text has finished and can continue

    //these are all the text + sentence variables
    public TextMeshProUGUI textDisplay, playerTextDisplay;
    public float typingSpeed;
    public string[] sentences;
    public string[] playerSentences;
    private int index;

    private string dialogPath = "Assets/Dialogs/TestDialog.txt";
    private StreamReader reader;
    string lineText;
    public Color[] colorArr = new Color[4];//Colors of text for characters

    // Start is called before the first frame update
    void Start()
    {
        //starts typing the sentences
        //StartCoroutine(Type()); 

        //makes all the speech bubbles + text invisible
        //playerBubble.SetActive(false);
        //playerSpeech.SetActive(false);
        bigBubble.SetActive(false);
        sentence.SetActive(false);
        miniBubble.SetActive(false);
        dots.SetActive(false);

        colorArr[0] = new Color(0,0,0);
        colorArr[1] = new Color(0.8f,0.3f,0.086f); //Hard Coded Angus Color
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)){ //if x is pressed and is touching the npc
            if(touched && !speechVisible)
            {
                reader = new StreamReader(dialogPath); //Reader read dialog txt
                lineText = reader.ReadLine();

                Debug.Log("show player bubble");

                //show the big bubble
                bigBubble.SetActive(true);
                sentence.SetActive(true);

                //show big player bubble
                //playerBubble.SetActive(true);
                //playerSpeech.SetActive(true);

                //hide the small bubble
                miniBubble.SetActive(false);
                dots.SetActive(false);
                speechVisible = true;
            }
            if(speechVisible && canContinue){
                Debug.Log("speaking");
                NextSentence();
            }
        }

        if(textDisplay.text == lineText){ //once text has finished completely
            canContinue = true;
        }
        Debug.Log("canContinue is: " + canContinue);
    }

    void OnTriggerEnter2D(Collider2D other){ 
        if(other.gameObject.name == "showBubbleCollider"){ //if player touches npc collider
            
            Debug.Log("touched NPC");

            //show the mini bubble
            miniBubble.SetActive(true);
            dots.SetActive(true);
            touched = true;
        }
    }

    void OnTriggerExit2D(Collider2D other){ 
        if(other.gameObject.name == "showBubbleCollider"){ //if player is no longer touching the npc collider
            
            Debug.Log("not touching NPC anymore");

            //hide the mini bubble
            miniBubble.SetActive(false);
            dots.SetActive(false);
            touched = false;

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
                textDisplay.color = colorArr[0];
            }
            else if(lineText.Substring(0,2) == "A:") //Angus's line
            {
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


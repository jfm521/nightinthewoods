using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCMove : MonoBehaviour
{
    // The npc's maximum movement speed
    public float speed;

    // The npc's current horizontal velocity
    float hVel;

    // The npc's horizontal acceleration
    public float hAcc;

    // The npc's current vertical velocity
    float vVel;

    // The horizontal direction the npc is moving (-1 for left, 1 for right, 0 for nowhere)
    float moveDir;

    // The npc's rigidbody and box collider (feet)
    Rigidbody2D myBody;
    BoxCollider2D myCollider;

    float goalX;

    public bool hasArrived;
    NPCTalktive npcTalkative;
    DialogDirector dialogDirector;
    GameObject tempMoveTo;
    bool moveAfterTalk;
    public Animator angusAnimator;

    // jackie's attempt at dialog initiation
    bool awaitingInput;
    NPCTrigger trigger;
    GameObject mae;
    public GameObject dialogPrompt;
    public float dialogPromptArea;


    // Called before the first frame update
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody2D>();
        myCollider = gameObject.GetComponent<BoxCollider2D>();

        npcTalkative = FindGameObjectInChildWithTag(gameObject,"TalkTrigger").GetComponent<NPCTalktive>();
        dialogDirector = GameObject.Find("DialogDirector").GetComponent<DialogDirector>();
        mae = GameObject.Find("DialogPlayer");

    }




    // Called once per frame
    void Update()
    {
        LocateGoal();

        //jackie's attempt at dialog initiation
        if (awaitingInput)
        {
            if (!dialogPrompt.activeSelf && hVel == 0)
            {
                dialogPrompt.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.X) && (mae.transform.position.x <= gameObject.transform.position.x + dialogPromptArea && mae.transform.position.x >= gameObject.transform.position.x - dialogPromptArea))
            {
                awaitingInput = false;
                InterpretTrigger();
                dialogPrompt.SetActive(false);

            }

            dialogPrompt.transform.position = gameObject.transform.position + mae.transform.GetComponent<DialogManager>().dialogBoxOffSet;
        }
    }




    // Called at a fixed rate
    void FixedUpdate()
    {
        vVel = myBody.velocity.y;
        if(myBody.velocity.x == 0 && myBody.velocity.y == 0)
            angusAnimator.SetBool("isWalking", false);
        else
            angusAnimator.SetBool("isWalking", true);

        HandleMovement();

        if(moveAfterTalk&&!DialogDirector.isTalking)
            WalkTowards(tempMoveTo.transform.position.x);
    }




    // Checks for keyboard input
    void LocateGoal()
    {
        // Checks left, right, and jump for movement
        if (gameObject.transform.position.x < goalX - .5)
        {
            moveDir = 1;
        }
        else if (gameObject.transform.position.x > goalX + .5)
        {
            moveDir = -1;
        }
        else
        {
            moveDir = 0;
        }
    }




    // Moves the player
    void HandleMovement()
    {
        // Changes the npc's horizontal velocity based on the npc's move direction (as determined by checkKeys)
        if (moveDir == 1)
        {
            if (hVel < speed)
            {
                hVel += hAcc;
            }
        }
        else if (moveDir == -1)
        {
            if (hVel > (-1 * speed))
            {
                hVel -= hAcc;
            }
        }
        else
        {
            if ((.1 > hVel) && (-.1 < hVel))
            {
                hVel = 0;
                angusAnimator.SetBool("isWalking", false);
                hasArrived = true;
            }
            else if (hVel > 0)
            {
                hVel -= hAcc;
            }
            else if (hVel < 0)
            {
                hVel += hAcc;
            }
        }

        // Updates the npc's actual velocity
        myBody.velocity = new Vector3(hVel * speed, myBody.velocity.y, 0);
    }

    // Sets the NPC's target location
    public void WalkTowards(float goalXInput)
    {
        
        hasArrived = false;
        goalX = goalXInput;
    }

    public void StopWalking()
    {
        goalX = transform.position.x;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "NPCTrigger")
        {
            tempMoveTo = null;
            moveAfterTalk = false;

            //jackie's dialog stuff, set npctrigger above so it's referenceable in different scripts
            /*NPCTrigger*/ trigger = other.GetComponent<NPCTrigger>();

            //jackie's attempt at dialog initiation
            if (trigger.autoStart)
            {
                InterpretTrigger();
                /*if (trigger.startCutscene)
                    DialogDirector.StartCutscene();
                else if (trigger.endCutscene)
                    DialogDirector.EndCutscene();

                if (trigger.talk)
                {
                    DialogDirector.AutoTalk(npcTalkative.characterSelect);
                    if (trigger.moveAfterTalk)
                    {
                        moveAfterTalk = true;
                        tempMoveTo = trigger.moveTo;
                    }
                }
                if (trigger.stop)
                    StopWalking();
                else if (trigger.move)
                    WalkTowards(trigger.moveTo.transform.position.x);
                else if (trigger.progPlot)
                    DialogDirector.ProgressPlot(npcTalkative.characterSelect);

                if (trigger.setTalkable)
                {
                    if (trigger.talkable)
                        FindGameObjectInChildWithTag(gameObject, "TalkTrigger").GetComponent<NPCTalktive>().talkable = trigger.talkable;
                }

                if (trigger.load)
                    SceneManager.LoadScene("StarConnect 1");*/
            }
            else
            {
                awaitingInput = true;
            }
        }
    }

    //jackie's attempt at dialog initiation; moved this here from OnTriggerEnter2D
    void InterpretTrigger()
    {
        if (trigger.startCutscene)
            DialogDirector.StartCutscene();
        else if (trigger.endCutscene)
            DialogDirector.EndCutscene();

        if (trigger.talk)
        {
            DialogDirector.AutoTalk(npcTalkative.characterSelect);
            if (trigger.moveAfterTalk)
            {
                moveAfterTalk = true;
                tempMoveTo = trigger.moveTo;
            }
        }
        if (trigger.stop)
            StopWalking();
        else if (trigger.move)
            WalkTowards(trigger.moveTo.transform.position.x);
        else if (trigger.progPlot)
            DialogDirector.ProgressPlot(npcTalkative.characterSelect);

        if (trigger.setTalkable)
        {
            if (trigger.talkable)
                FindGameObjectInChildWithTag(gameObject, "TalkTrigger").GetComponent<NPCTalktive>().talkable = trigger.talkable;
        }

        if (trigger.load)
            SceneManager.LoadScene("StarConnect 1");
    }

    public void StartTopDown()
    {
        moveAfterTalk = false;
        myBody.bodyType = RigidbodyType2D.Static;
        transform.position = GameObject.Find("AngusPos").transform.position;
        transform.parent = GameObject.Find("AngusPos").transform;
        GameObject.Find("Angus Body").GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
        GameObject.Find("Angus Head").GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
        GameObject.Find("AngusLegs").GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
        GameObject.Find("AngusArm1").GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
        GameObject.Find("AngusArm2").GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
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

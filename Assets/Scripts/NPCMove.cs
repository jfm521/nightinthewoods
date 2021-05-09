using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Called before the first frame update
    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody2D>();
        myCollider = gameObject.GetComponent<BoxCollider2D>();

        npcTalkative = FindGameObjectInChildWithTag(gameObject,"TalkTrigger").GetComponent<NPCTalktive>();
        dialogDirector = GameObject.Find("DialogDirector").GetComponent<DialogDirector>();

    }




    // Called once per frame
    void Update()
    {
        LocateGoal();
    }




    // Called at a fixed rate
    void FixedUpdate()
    {
        vVel = myBody.velocity.y;

        HandleMovement();

        if(moveAfterTalk&&!dialogDirector.isTalking)
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
            NPCTrigger trigger = other.GetComponent<NPCTrigger>();
            
            if(trigger.startCutscene)
                dialogDirector.StartCutscene();
            else if(trigger.endCutscene)
                dialogDirector.EndCutscene();

            if(trigger.talk){
                dialogDirector.AutoTalk(npcTalkative.characterSelect);
                if(trigger.moveAfterTalk)
                {
                    moveAfterTalk = true;
                    tempMoveTo = trigger.moveTo;
                }
            }
            if(trigger.stop)
                StopWalking();
            else if(trigger.move)
                WalkTowards(trigger.moveTo.transform.position.x);
            else if(trigger.progPlot)
                dialogDirector.ProgressPlot(npcTalkative.characterSelect);
        }
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

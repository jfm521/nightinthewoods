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




    // Called before the first frame update
    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody2D>();
        myCollider = gameObject.GetComponent<BoxCollider2D>();
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
        goalX = goalXInput;
    }

}

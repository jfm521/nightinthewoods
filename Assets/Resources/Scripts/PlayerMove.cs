using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerMove : MonoBehaviour
{
    // The player's maximum movement speed
    public float speed;

    // The player's current horizontal velocity
    float hVel;

    // The player's horizontal acceleration
    public float hAcc;

    // The player's current vertical velocity
    float vVel;

    // The horizontal direction the player is moving (-1 for left, 1 for right, 0 for nowhere)
    float moveDir;

    // Stores the player's jump input
    bool jump;

    // The player's maximum jump height
    public float jumpHeight;

    // The player's maximum triple jump height

    public float tripleJumpHeight;

    // Stores the player's insantaneous jump velocity
    float jumpVel;

    // Stores whether or not the player's feet are touching the floor
    bool onFloor;

    // Stores the number of times the player has jumped leading up to a triple jump
    float tripleJump;

    // Tracks the time spent on the floor to potentially cancel a triple jump
    float tripleJumpTimer;

    // How much time can pass before a triple jump is canceled
    public float maxTripleJumpTimer;

    // The player's rigidbody and box collider (feet)
    Rigidbody2D myBody;
    BoxCollider2D myCollider;




    // Called before the first frame update
    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody2D>();
        myCollider = gameObject.GetComponent<BoxCollider2D>();
    }




    // Called once per frame
    void Update()
    {  
        CheckKeys(); 
    }




    // Called at a fixed rate
    void FixedUpdate()
    {
        vVel = myBody.velocity.y;

        HandleMovement();
        TimeTripleJump();
        DetectCollisions();
    }




    // Checks for keyboard input
    void CheckKeys()
    {
        // Checks left, right, and jump for movement
        if (Input.GetKey(KeyCode.D))
        {
            moveDir = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveDir = -1;
        }
        else
        {
            moveDir = 0;
            if (tripleJump > 0)
            {
                tripleJump = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        // Cancels the triple jump if the player lets go of a key
        if ((Input.GetKeyUp(KeyCode.D)) || (Input.GetKeyUp(KeyCode.A)))
        {
            tripleJump = 0;
        }
    }




    // Moves the player
    void HandleMovement()
    {
        // Changes the player's horizontal velocity based on the player's move direction (as determined by checkKeys)
        if (moveDir == 1)
        {
            if (hVel < speed){
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

        // Changes the player's vertical velocity when they jump

        jumpVel = myBody.velocity.y;

        if (jump == true)
        {
            if (onFloor == true)
            {
                tripleJump += 1;
                if (tripleJump >= 3)
                {
                    jumpVel += tripleJumpHeight;
                    tripleJump = 0;
                }
                else
                {
                    jumpVel += jumpHeight;
                }
                onFloor = false;
                tripleJumpTimer = 0;
            }
            jump = false;
        }

        // Updates the player's actual velocity
        myBody.velocity = new Vector3(hVel * speed, jumpVel, 0);
    }




    // Sets onFloor to true if the player collides with the floor
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Floor")
        {
            onFloor = true;
        }
    }




    // Cancels the triple jump if the player spends too long on the ground
    void TimeTripleJump()
    {
        if ((tripleJump > 0) && (onFloor == true))
        {
            tripleJumpTimer += 1;
            if (tripleJumpTimer >= maxTripleJumpTimer)
            {
                tripleJump = 0;
                tripleJumpTimer = 0;
            }
        }
    }




    // Detects for collision with the floor
    void DetectCollisions()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(transform.position.x + myBody.velocity.x, transform.position.y + myBody.velocity.y), 1);
        Debug.DrawRay(transform.position, new Vector3(transform.position.x + myBody.velocity.x, transform.position.y + myBody.velocity.y), Color.black);
    }

}

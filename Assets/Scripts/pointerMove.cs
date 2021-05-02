using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointerMove : MonoBehaviour
{

    //Stores whether the player is currently drawing a line
    public bool isConnecting;

    // Box collider
    CircleCollider2D myCollider;

    // Camera
    public Camera cam;

    // Stores the star that the player is currently drawing from
    public GameObject starInHand;

    // Stores the Star Manager obejct
    public GameObject starManager;




    // Start is called before the first frame update
    void Start()
    {
        // Set variables
        isConnecting = false;
        myCollider = gameObject.GetComponent<CircleCollider2D>();
    }




    // Update is called once per frame
    void Update()
    {
       
         if (isConnecting)
        {
            // Cancels isConnecting if the mouse button is clicked
            /*if (Input.GetMouseButtonDown(0))
            {
                isConnecting = false;
            }*/

            Debug.Log("CONNECTING");
        }

    }




    void FixedUpdate()
    {
        // Moves the hand to the mouse's position
        myCollider.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
    }




    // If the player clicks on a star, they start connecting
    void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetMouseButtonDown(0))
            {
            if (collision.tag == "Star")
            {
                if (!isConnecting)
                {
                    isConnecting = true;
                    starInHand = collision.gameObject;
                }
                else
                {
                    isConnecting = false;
                }
            }
        }
    }
}

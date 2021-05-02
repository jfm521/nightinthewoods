using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starConnect : MonoBehaviour
{
    //Stores how many stars each star is connected to
    public int isConnected;

    // Stores which stars can connect to the current star
    public GameObject starFriend1;
    public GameObject starFriend2;

    // Stores which star is connected
    public bool starFriend1Connected;
    public bool starFriend2Connected;

    // Stores the pointer
    public GameObject pointer;

    // Stores the start and end points of the line
    Vector3 startPt;
    Vector3 endPt;

    // Start is called before the first frame update
    void Start()
    {
        isConnected = 0;

        starFriend1Connected = false;
        starFriend2Connected = false;

        startPt = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y);
        endPt = startPt;
    }

    // Update is called once per frame
    void Update()
    {
        //Draws the line (HAVE GIZMOS ON)
        if (pointer.GetComponent<pointerMove>().isConnecting && pointer.GetComponent<pointerMove>().starInHand == gameObject)
        {

        }
        Debug.DrawLine(startPt, endPt);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (collision.tag == "Pointer")
            {
                if (collision.GetComponent<pointerMove>().isConnecting)
                {
                    if (collision.GetComponent<pointerMove>().starInHand == starFriend1 && !starFriend1Connected)
                    {
                        isConnected += 1;
                        starFriend1Connected = true;
                    }
                    else if (collision.GetComponent<pointerMove>().starInHand == starFriend2 && !starFriend2Connected)
                    {
                        isConnected += 1;
                        starFriend2Connected = true;
                    }
                }
            }
        }
    }
}

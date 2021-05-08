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

    // Line renderer
    public LineRenderer myLine;
    public LineRenderer connectedLine;

    // Camera (for mouse pos)
    public Camera cam;

    // Debug
    public bool debugOn;

    // Start is called before the first frame update
    void Start()
    {
        isConnected = 0;

        starFriend1Connected = false;
        starFriend2Connected = false;
    }

    // Update is called once per frame
    void Update()
    {

        //Detects if star friends are connected from a different star
        if (starFriend1Connected == false && starFriend1.GetComponent<starConnect>().starFriend1Connected && starFriend1.GetComponent<starConnect>().starFriend1 == gameObject)
        {
            starFriend1Connected = true;
        }
        else if (starFriend1Connected == false && starFriend1.GetComponent<starConnect>().starFriend2Connected && starFriend1.GetComponent<starConnect>().starFriend2 == gameObject)
        {
            starFriend1Connected = true;
        }
        if (starFriend2Connected == false && starFriend2.GetComponent<starConnect>().starFriend1Connected && starFriend2.GetComponent<starConnect>().starFriend1 == gameObject)
        {
            starFriend2Connected = true;
        }
        else if (starFriend2Connected == false && starFriend2.GetComponent<starConnect>().starFriend2Connected && starFriend2.GetComponent<starConnect>().starFriend2 == gameObject)
        {
            starFriend2Connected = true;
        }

        //Draws the line if not yet connected
        if (!starFriend1Connected || !starFriend2Connected)
        {
            if (pointer.GetComponent<pointerMove>().isConnecting && pointer.GetComponent<pointerMove>().starInHand == gameObject)
            {
                float mousex = cam.ScreenToWorldPoint(Input.mousePosition).x;
                float mousey = cam.ScreenToWorldPoint(Input.mousePosition).y;
                myLine.SetPosition(1, new Vector3(mousex - gameObject.transform.position.x, mousey - gameObject.transform.position.y));
                if (debugOn)
                {
                    Debug.Log("Drawing line");
                }
            }
            else
            {
                myLine.SetPosition(1, new Vector3(0, 0));
            }
        }
        else
        {
            myLine.SetPosition(1, new Vector3(0, 0));
        }

        // Draws the line if connected
        if (starFriend1Connected)
        {
            connectedLine.SetPosition(1, new Vector3(starFriend1.gameObject.transform.position.x - gameObject.transform.position.x, starFriend1.gameObject.transform.position.y - gameObject.transform.position.y));
        }
    }

    // When mouse is clicked while touching star, connect or cancel
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

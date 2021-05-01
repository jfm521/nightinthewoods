using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manageStars : MonoBehaviour
{

    // Array of stars
    public GameObject[] cons1;
    public GameObject[] cons2;
    public GameObject[] cons3;
    public GameObject[] cons4;

    // Counts how many stars are finished
    int starcount;

    // Keeps track of which constellations are done
    bool cons1Done;
    bool cons2Done;
    bool cons3Done;
    bool cons4Done;

    // Start is called before the first frame update
    void Start()
    {
        cons1Done = false;
        cons2Done = false;
        cons3Done = false;
        cons4Done = false;
    }

    // Update is called once per frame
    void Update()
    {

        // Detects if constellation 1 is finished
        if (!cons1Done)
        {
            for (int i = 0; i < cons1.Length; i++)
            {
                if (cons1[i].GetComponent<starConnect>().starFriend1Connected)
                {
                    starcount += 1;
                }
            }

            if (starcount == cons1.Length)
            {
                cons1Done = true;
                Debug.Log("CONSTELLATION 1 FINISHED");
            }

            starcount = 0;
        }


        // Detects if constellation 2 is finished
        if (!cons2Done)
        {
            for (int i = 0; i < cons2.Length; i++)
            {
                if (cons2[i].GetComponent<starConnect>().starFriend1Connected)
                {
                    starcount += 1;
                }
            }

            if (starcount == cons2.Length)
            {
                cons2Done = true;
                Debug.Log("CONSTELLATION 2 FINISHED");
            }

            starcount = 0;
        }


        // Detects if constellation 3 is finished
        if (!cons3Done)
        {
            for (int i = 0; i < cons3.Length; i++)
            {
                if (cons3[i].GetComponent<starConnect>().starFriend1Connected)
                {
                    starcount += 1;
                }
            }

            if (starcount == cons3.Length)
            {
                cons3Done = true;
                Debug.Log("CONSTELLATION 3 FINISHED");
            }

            starcount = 0;
        }


        // Detects if constellation 4 is finished
        if (!cons4Done)
        {
            for (int i = 0; i < cons4.Length; i++)
            {
                if (cons4[i].GetComponent<starConnect>().starFriend1Connected)
                {
                    starcount += 1;
                }
            }

            if (starcount == cons4.Length)
            {
                cons4Done = true;
                Debug.Log("CONSTELLATION 4 FINISHED");
            }

            starcount = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manageStars : MonoBehaviour
{

    // Array of stars
    public GameObject[] consA;

    int starcount;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // Detects if the constellation is finished
        for (int i = 0; i < consA.Length; i++)
        {
            if (consA[i].GetComponent<starConnect>().starFriend1Connected)
            {
                starcount += 1;
            }
            if (consA[i].GetComponent<starConnect>().starFriend2Connected)
            {
                starcount += 1;
            }
        }

        if (starcount == consA.Length)
        {
            Debug.Log("CONSTELLATION FINISHED");
        }

        starcount = 0;
    }
}

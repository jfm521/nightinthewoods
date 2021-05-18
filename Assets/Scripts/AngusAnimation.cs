using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngusAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator Angusanimator;
    Vector3 lastPos;
    Vector3 currentPosition;

    void Start()
    {
        Angusanimator.SetBool("isTalking", false);
    }

    // Update is called once per frame
    void Update()
    {
        /*currentPosition = transform.position;
        if (currentPosition == lastPos){
            
        } else if (currentPosition != lastPos){
            Angusanimator.SetBool("isWalking", true);
        }
        lastPos = currentPosition;*/
    } 
    
}

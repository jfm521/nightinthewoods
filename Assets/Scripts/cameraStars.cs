using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStars : CameraFollow
{
    public static float camX;
    public static float camY;
    bool isMovable;
    void Update()
    {
        Debug.Log(isMovable);
    }

    void FixedUpdate()
    {   
        if(!isCutscene)
        {
            camY = Mathf.Clamp(followTransform.position.y, yMin + camSize, yMax - camSize);
            camX = Mathf.Clamp(followTransform.position.x, xMin + camSize, xMax - camSize);
        }
        if (isMovable)
        {
            smoothPos = Vector3.Lerp(gameObject.transform.position, new Vector3(camX, camY, gameObject.transform.position.z), smoothRate);
        }
        else
        {
            smoothPos = gameObject.transform.position;
        }
        
        gameObject.transform.position = smoothPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pointer")
        {
            isMovable = false;
        }
        Debug.Log("Enter collision");
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pointer")
        {
            isMovable = true;
        }
        Debug.Log("Exit collision");
    }
}

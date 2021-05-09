using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraStars : MonoBehaviour
{

    public Transform followTransform;
    public BoxCollider2D worldBounds;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    float camX;
    float camY;

    float camRatio;
    float camSize;

    bool isMovable;

    Camera mainCam;

    Vector3 smoothPos;

    public float smoothRate;

    // Start is called before the first frame update
    void Start()
    {
        xMin = worldBounds.bounds.min.x;
        xMax = worldBounds.bounds.max.x;
        yMin = worldBounds.bounds.min.y;
        yMax = worldBounds.bounds.max.y;

        mainCam = gameObject.GetComponent<Camera>();

        camSize = mainCam.orthographicSize;
        camRatio = (xMax + camSize) / 8.0f;
    }

    //lerp, 2 positions, moving from one to other

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isMovable);
    }

    void FixedUpdate()
    {
        camY = Mathf.Clamp(followTransform.position.y, yMin + camSize, yMax - camSize);
        camX = Mathf.Clamp(followTransform.position.x, xMin + camSize, xMax - camSize);

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

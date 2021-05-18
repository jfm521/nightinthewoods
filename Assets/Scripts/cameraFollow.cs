using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform followTransform;
    public BoxCollider2D worldBounds;

    protected float xMin;
    protected float xMax;
    protected float yMin;
    protected float yMax;

    static float camX;
    static float camY;

    protected float camRatio;
    protected float camSize;

    protected Camera mainCam;

    protected Vector3 smoothPos;

    public float smoothRate; 
    public static bool isCutscene;

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
        
    }

    void FixedUpdate(){
    if(!isCutscene)
    {
        camY = Mathf.Clamp(followTransform.position.y, yMin + camSize, yMax - camSize);
        camX = Mathf.Clamp(followTransform.position.x, xMin + camSize, xMax - camSize);
    }
        smoothPos = Vector3.Lerp(gameObject.transform.position, new Vector3(camX, camY, gameObject.transform.position.z), smoothRate);
        
        gameObject.transform.position = smoothPos;
    }
    public static void GoTo(Vector3 position)
    {
        camX = position.x;
        camY = position.y;
   }
}

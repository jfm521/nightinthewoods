using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class sceneTransition : MonoBehaviour
{

    public bool fading = false;
    public SpriteRenderer blackScreen;
    private Color fadeColor;
    public float alpha = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (fading == true){
            Debug.Log("fading"); //if its fading
            
            if (alpha < 1){ //if slowly adding color
                alpha += .01f; //ad more
            } else { //if not and completely solid
                SceneManager.LoadScene("star gazing"); //change screen
            }
        } else {
            if (alpha > 0){ //at start of scene
                alpha -= .01f; //slowly fade dark screen away
            }
        }

        fadeColor = new Color(0, 0, 0, alpha);
        blackScreen.color = fadeColor;
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.name == "transitionTriggerBox"){
            //gameObject.GetComponent<SpriteRenderer>().enabled = false; makes player disappear
            fading = true; //sets baclk screen to strat fading
        }
    }
}

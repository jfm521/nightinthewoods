using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCCharacters // not useful for now
{   public class NPCAngus : MonoBehaviour
    {
        // Start is called before the first frame update
        public float plotProg = 0; //Plot Progression value determines which dialog to send
        public string[] dialogArr = new string[5];
        //string NPCName = "Angus";
        void Awake()
        {
            dialogArr[0] = "Assets/Dialogs/Angus/AngusDialogGetInCar.txt";
            dialogArr[1] = "Assets/Dialogs/Angus/AngusDialogDriveCar.txt";
        }
        public string CheckPlotProg()
        {
            //plotProg = GameObject.Find("GameManager").GetComponent<NPCAngus>().GetAngusPlotProg();
            //Debug.Log("Got plotProg");
            Debug.Log(plotProg);
            if(plotProg == 0)
                return dialogArr[0];
            else if(plotProg == 1)
                return dialogArr[1];
            else return "Error in CheckPlotProg";
        }
    }
    public class NPCAngusStars : NPCAngus //Seperate Class for Ease of Testing
    {
        // Start is called before the first frame update
        void Awake()
        {
            dialogArr[0] = "Assets/Dialogs/Angus/Stars/AngusStarsDialog1.txt";
        }
    }
}
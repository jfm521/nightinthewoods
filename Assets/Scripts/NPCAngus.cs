using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

        public enum plotKey
        {
            SectionIndex,
            DialogIndex
        }
    public class NPCAngus : MonoBehaviour
    {
        //Plot Progression value determines which dialog to send
        //First array is scetion of plaot, second is dialog

        public static Dictionary<plotKey,int> plotProg = new Dictionary<plotKey, int>(); 
        public static string[,] dialogArr = new string[5,17];
        public static string[] dialogArrSectionName = new string[5];

        public Text debugText;
        private string debugStr;
        public bool debugMode;
        private static string sectionEnd = "ENDOFSECTION";
        private static string cutsceneSpot = "CUTSCENESPOT";
        //string NPCName = "Angus";
        void Awake()
        {
            dialogArrSectionName[0] = "Graveyard";
            dialogArr[0,0] = "Assets/Resources/Dialogs/Angus/"+dialogArrSectionName[0]+"/AngusDialogGraveyard1.txt";
            dialogArr[0,1] = "Assets/Resources/Dialogs/Angus/"+dialogArrSectionName[0]+"/AngusDialogGraveyard2.txt";
            dialogArr[0,2] = "Assets/Resources/Dialogs/Angus/"+dialogArrSectionName[0]+"/AngusDialogGraveyard3.txt";
            dialogArr[0,3] = sectionEnd;
            //Don't need rest
            dialogArrSectionName[1] = "(Load)StarGazing";
            dialogArr[1,0] = "StarConnect"; //Scene name
            dialogArr[1,1] = sectionEnd;
            dialogArrSectionName[2] = "(Brch)StarGazing";
            dialogArr[2,0] = "Assets/Resources/Dialogs/Angus/"+dialogArrSectionName[2]+"/AngusDialogStarsStart.txt";
            dialogArr[2,1] = "Assets/Resources/Dialogs/Angus/"+dialogArrSectionName[2]+"/AngusDialogStarsPope.txt";
            dialogArr[2,2] = "Assets/Resources/Dialogs/Angus/"+dialogArrSectionName[2]+"/AngusDialogStarsWhale.txt";
            dialogArr[2,3] = "Assets/Resources/Dialogs/Angus/"+dialogArrSectionName[2]+"/AngusDialogStarsBell.txt";
            dialogArr[2,4] = "Assets/Resources/Dialogs/Angus/"+dialogArrSectionName[2]+"/AngusDialogStarsThief.txt";
            dialogArrSectionName[3] = "(Load)EndingScene";
            dialogArr[3,0] = "EndScene";
            dialogArr[3,1] = sectionEnd;
            plotProg.Add(plotKey.SectionIndex,0);
            plotProg.Add(plotKey.DialogIndex,0);
        }
        public string GetDialogPath()
        {

            return dialogArr[plotProg[plotKey.SectionIndex],plotProg[plotKey.DialogIndex]];
            
        }
        public static void ProgressPlot()
        {
            if(dialogArrSectionName[plotProg[plotKey.SectionIndex]].Substring(0,6)=="(Brch)")
                ProgressPlotBranch("");
            else
                ProgressPlotNormal();
        }
        public static void ProgressPlot(string branch)
        {
            if(dialogArrSectionName[plotProg[plotKey.SectionIndex]].Substring(0,6)=="(Brch)")
                ProgressPlotBranch(branch);
            else
                ProgressPlotNormal();
        }
        static void ProgressPlotNormal()
        {
            if(plotProg[plotKey.SectionIndex] <= 1) //If it is in the graveyard
            {
                plotProg[plotKey.DialogIndex]+=1;
                Debug.Log("Progressed to Section: "+dialogArrSectionName[plotProg[plotKey.SectionIndex]]+plotProg[plotKey.DialogIndex]);
                if(dialogArr[plotProg[plotKey.SectionIndex],plotProg[plotKey.DialogIndex]] == sectionEnd)
                {
                    plotProg[plotKey.SectionIndex]+=1;
                    plotProg[plotKey.DialogIndex] = 0;
                    Debug.Log("Entering "+dialogArrSectionName[plotProg[plotKey.SectionIndex]]);
                }
            }
        }
        static void ProgressPlotBranch(string branch)//a method spcifically for choosing star
        {
            if(plotProg[plotKey.SectionIndex] == 2) //If star gazing
            {
                switch(branch)
                {
                    case("Bell"):
                    {
                        plotProg[plotKey.DialogIndex] = 3;
                        Debug.Log("Progressed to Section: "+dialogArrSectionName[plotProg[plotKey.SectionIndex]]+plotProg[plotKey.DialogIndex]);
                        break;
                    }
                    case("Whale"):
                    {
                        plotProg[plotKey.DialogIndex] = 2;
                        Debug.Log("Progressed to Section: "+dialogArrSectionName[plotProg[plotKey.SectionIndex]]+plotProg[plotKey.DialogIndex]);
                        break;
                    }
                    case("Pope"):
                    {
                        plotProg[plotKey.DialogIndex] = 1;
                        Debug.Log("Progressed to Section: "+dialogArrSectionName[plotProg[plotKey.SectionIndex]]+plotProg[plotKey.DialogIndex]);
                        break;
                    }
                    case("Thief"):
                    {
                        plotProg[plotKey.DialogIndex] = 4;
                        Debug.Log("Progressed to Section: "+dialogArrSectionName[plotProg[plotKey.SectionIndex]]+plotProg[plotKey.DialogIndex]);
                        break;
                    }
                    case("<END>"):
                    {
                        plotProg[plotKey.SectionIndex]+=1;
                        plotProg[plotKey.DialogIndex] = 0;
                        Debug.Log("Entering "+dialogArrSectionName[plotProg[plotKey.SectionIndex]]);
                        break;
                    }
                    default:
                    {
                        Debug.Log("Error in ProgressPlot(Branch): bad branch name");
                        break;
                    }
                }
            }
        }
        void Update(){
            debugStr = "Current: "+dialogArrSectionName[plotProg[plotKey.SectionIndex]]+plotProg[plotKey.DialogIndex];
            if(debugMode)
                debugText.text = "plotProg:["+plotProg[plotKey.SectionIndex]+","+plotProg[plotKey.DialogIndex];
        }
}
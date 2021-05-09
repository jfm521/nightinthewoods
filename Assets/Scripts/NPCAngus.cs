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

        public Dictionary<plotKey,int> plotProg = new Dictionary<plotKey, int>(); 
        public string[,] dialogArr = new string[5,7];
        public string[] dialogArrSectionName = new string[5];

        public Text debugText;
        public bool debugMode;
        private string sectionEnd = "ENDOFSECTION";
        private string cutsceneSpot = "CUTSCENESPOT";
        //string NPCName = "Angus";
        void Awake()
        {
            dialogArrSectionName[0] = "Graveyard";
            dialogArr[0,0] = "Assets/Resources/Dialogs/Angus/"+dialogArrSectionName[0]+"/AngusDialogGraveyard1.txt";
            dialogArr[0,1] = "Assets/Resources/Dialogs/Angus/"+dialogArrSectionName[0]+"/AngusDialogGraveyard2.txt";
            dialogArr[0,2] = "Assets/Resources/Dialogs/Angus/"+dialogArrSectionName[0]+"/AngusDialogGraveyard3.txt";
            dialogArr[0,3] = sectionEnd;
            //Don't need rest
            dialogArrSectionName[1] = "<Loop>StarGazing";
            dialogArr[1,0] = "Assets/Resources/Dialogs/Angus/"+dialogArrSectionName[0]+"/AngusDialogGraveyard3.txt";
            dialogArr[0,3] = sectionEnd;
            dialogArrSectionName[2] = "<Brch>StarGazing";
            dialogArr[2,0] = "Assets/Resources/Dialogs/Angus/"+dialogArrSectionName[1]+"/AngusDialogStarsStart.txt";
            dialogArr[2,1] = "Assets/Resources/Dialogs/Angus/"+dialogArrSectionName[1]+"/AngusDialogStarsBell.txt";
            dialogArr[2,2] = "Assets/Resources/Dialogs/Angus/"+dialogArrSectionName[1]+"/AngusDialogStarsWhale.txt";
            dialogArr[2,3] = "Assets/Resources/Dialogs/Angus/"+dialogArrSectionName[1]+"/AngusDialogStarsEnd.txt";
            dialogArr[2,4] = sectionEnd;
            plotProg.Add(plotKey.SectionIndex,0);
            plotProg.Add(plotKey.DialogIndex,0);
        }
        public string GetDialogPath()
        {

            return dialogArr[plotProg[plotKey.SectionIndex],plotProg[plotKey.DialogIndex]];
            
        }
        public void ProgressPlot()
        {
            if(dialogArrSectionName[plotProg[plotKey.SectionIndex]].Substring(0,6)=="<Brch>")
                ProgressPlotBranch("");
            else
                ProgressPlotNormal();
        }
        public void ProgressPlot(string branch)
        {
            if(dialogArrSectionName[plotProg[plotKey.SectionIndex]].Substring(0,6)=="<Brch>")
                ProgressPlotBranch(branch);
            else
                ProgressPlotNormal();
        }
        void ProgressPlotNormal()
        {
            if(plotProg[plotKey.SectionIndex] == 0) //If it is in the graveyard
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
        void ProgressPlotBranch(string branch)//a method spcifically for choosing star
        {
            if(plotProg[plotKey.SectionIndex] == 1) //If star gazing
            {
                switch(branch)
                {
                    case("Bell"):
                    {
                        Debug.Log("Progressed to Section: "+dialogArrSectionName[plotProg[plotKey.SectionIndex]]+plotProg[plotKey.DialogIndex]);
                        plotProg[plotKey.DialogIndex] = 1;
                        break;
                    }
                    case("Whale"):
                    {
                        Debug.Log("Progressed to Section: "+dialogArrSectionName[plotProg[plotKey.SectionIndex]]+plotProg[plotKey.DialogIndex]);
                        plotProg[plotKey.DialogIndex] = 2;
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
        void LateUpdate(){
            if(debugMode)
                debugText.text = "Current: "+dialogArrSectionName[plotProg[plotKey.SectionIndex]]+plotProg[plotKey.DialogIndex];
        }
}
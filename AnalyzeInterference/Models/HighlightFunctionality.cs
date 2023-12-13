using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalyzeInterference.Common;
using Inventor;

namespace AnalyzeInterference.Models
{
    internal class HighlightFunctionality
    {
        private static HighlightFunctionality _instance;
        public static HighlightFunctionality Instance => _instance ?? (_instance = new HighlightFunctionality());

        public HighlightSet highlightRed ;
        public HighlightSet highlightGreen ;
        public HighlightSet highlightBlue ;

        public HighlightFunctionality() {
            highlightRed= Globals.ActiveInvDoc.CreateHighlightSet();
            highlightGreen = Globals.ActiveInvDoc.CreateHighlightSet();
            highlightBlue = Globals.ActiveInvDoc.CreateHighlightSet();


            highlightRed.Color = Globals.InvApp.TransientObjects.CreateColor(255, 0, 0);
            highlightGreen.Color = Globals.InvApp.TransientObjects.CreateColor(0,255,0);
            highlightBlue.Color = Globals.InvApp.TransientObjects.CreateColor(0,0,255);

            //oHS2.Color = invapp.TransientObjects.CreateColor(0, 255, 0) '緑
            //oHS3.Color = invapp.TransientObjects.CreateColor(10, 0, 255) '青
            //oHS4.Color = invapp.TransientObjects.CreateColor(255, 255, 0) '黄色
            //oHS5.Color = invapp.TransientObjects.CreateColor(0, 255, 255) 'シアン
            //oHS6.Color = invapp.TransientObjects.CreateColor(255, 255, 255) '白
            //oHS_InterferenceBody.Color = invapp.TransientObjects.CreateColor(255, 0, 0) '赤
        }

        public void ComponentHighlight(List<ComponentData> InterferenceResultsList)
        {
            foreach(var item in InterferenceResultsList)
            {
                highlightRed.AddItem(item.ComponentOccurrence);
            }
        }
    }
}

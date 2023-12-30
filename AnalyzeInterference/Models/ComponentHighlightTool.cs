using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalyzeInterference.Common;
using Inventor;

namespace AnalyzeInterference.Models
{
    internal class ComponentHighlightTool
    {
        private static ComponentHighlightTool _instance;
        public static ComponentHighlightTool Instance => _instance ?? (_instance = new ComponentHighlightTool());

        public HighlightSet highlightRed ;
        public HighlightSet highlightGreen ;
        public HighlightSet highlightBlue ;

        public ComponentHighlightTool() {
            highlightRed= Globals.ActiveInvDoc.CreateHighlightSet();
            highlightGreen = Globals.ActiveInvDoc.CreateHighlightSet();
            highlightBlue = Globals.ActiveInvDoc.CreateHighlightSet();


            highlightRed.Color = Globals.InvApp.TransientObjects.CreateColor(255, 0, 0);
            highlightGreen.Color = Globals.InvApp.TransientObjects.CreateColor(0,255,0);
            highlightBlue.Color = Globals.InvApp.TransientObjects.CreateColor(0,0,255);
        }

        public void ApplyToAll(List<ComponentData> InterferenceResultsList)
        {
            foreach(var item in InterferenceResultsList)
            {
                highlightRed.AddItem(item.ComponentOccurrence);
            }
        }
    }
}

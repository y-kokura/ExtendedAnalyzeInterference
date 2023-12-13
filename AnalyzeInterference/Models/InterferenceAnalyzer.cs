using Inventor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalyzeInterference.Models;
using AnalyzeInterference.Common;

namespace AnalyzeInterference.Models
{
    internal class InterferenceAnalyzer
    {
        private static InterferenceAnalyzer _instance;
        public static InterferenceAnalyzer Instance => _instance ?? (_instance = new InterferenceAnalyzer());

        public (InterferenceResults AnalyzeResultsBoth, InterferenceResults AnalyzeResultsScrew) InterferenceAnalysisExecute(ObjectCollection ScrewComponentCollection,ObjectCollection NonScrewComponentCollection)
        {
            AssemblyComponentDefinition assemblyCompDef = Globals.ActiveInvDoc.ComponentDefinition;
            InterferenceResults AnalyzeResultsBoth =null;
            InterferenceResults AnalyzeResultsScrew=null;
            //InterferenceResults resultsNonScrew=null;

            if (ScrewComponentCollection.Count >= 1 && NonScrewComponentCollection.Count >= 1)
            {
                AnalyzeResultsBoth = assemblyCompDef.AnalyzeInterference(ScrewComponentCollection, NonScrewComponentCollection);
                AnalyzeResultsScrew = assemblyCompDef.AnalyzeInterference(ScrewComponentCollection);
                //assemblyCompDef.AnalyzeInterference(NonScrewComponentCollection);
            }
            else if (ScrewComponentCollection.Count >= 1 && NonScrewComponentCollection.Count == 0)
            {
                AnalyzeResultsScrew = assemblyCompDef.AnalyzeInterference(ScrewComponentCollection);
            }

            return (AnalyzeResultsBoth, AnalyzeResultsScrew);
        }
    }

}

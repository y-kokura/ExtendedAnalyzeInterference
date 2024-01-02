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
    internal class OccurrenceCollectionInterferenceAnalysis
    {
        private static OccurrenceCollectionInterferenceAnalysis _instance;
        public static OccurrenceCollectionInterferenceAnalysis Instance => _instance ?? (_instance = new OccurrenceCollectionInterferenceAnalysis());

        public (InterferenceResults AnalyzeResultsBoth, InterferenceResults AnalyzeResultsScrew) PerformInterferenceAnalysis(ObjectCollection ScrewComponentCollection,ObjectCollection NonScrewComponentCollection)
        {
            AssemblyComponentDefinition assemblyCompDef = Globals.ActiveInvDoc.ComponentDefinition;
            InterferenceResults AnalyzeResultsBoth =null;
            InterferenceResults AnalyzeResultsScrew=null;
            //InterferenceResults resultsNonScrew=null;

            if(ScrewComponentCollection==0)return(null,null);
            
            AnalyzeResultsScrew = assemblyCompDef.AnalyzeInterference(ScrewComponentCollection);
            if(NonScrewComponentCollection>=1)AnalyzeResultsBoth = assemblyCompDef.AnalyzeInterference(ScrewComponentCollection, NonScrewComponentCollection);
                        
            return (AnalyzeResultsBoth, AnalyzeResultsScrew);

        }
    }

}

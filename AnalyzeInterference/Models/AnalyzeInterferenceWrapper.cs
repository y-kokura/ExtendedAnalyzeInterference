using AnalyzeInterference.ViewModels;
using Inventor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnalyzeInterference.Models
{
    internal class AnalyzeInterferenceWrapper
    {
        public AnalyzeInterferenceWrapper()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public void ExecuteAnalysis()
        {
            //アセンブリドキュメント内のComponentOccurrenceを分類する。
            (ObjectCollection ScrewComponentCollection, ObjectCollection NonScrewComponentCollection) =OccurrenceCategorizer.Instance.StartCategorize();
            (InterferenceResults AnalyzeResultsBoth, InterferenceResults AnalyzeResultsScrew) =InterferenceAnalyzer.Instance.InterferenceAnalysisExcute(ScrewComponentCollection, NonScrewComponentCollection);

        }
    }
}

using AnalyzeInterference.ViewModels;
using AnalyzeInterference.Views;
using Inventor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static AnalyzeInterference.Common.AnalyzeInterferenceModule;

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
            //アセンブリドキュメント内のComponentOccurrenceを分類する(ネジを含む部品か含まない部品か)。
            (ObjectCollection ScrewComponentCollection, ObjectCollection NonScrewComponentCollection, List<ComponentData> InterferenceResultsList) =OccurrenceCategorizer.Instance.StartCategorize();
            (InterferenceResults AnalyzeResultsBoth, InterferenceResults AnalyzeResultsScrew) =InterferenceAnalyzer.Instance.InterferenceAnalysisExecute(ScrewComponentCollection, NonScrewComponentCollection);

            Test.test1(AnalyzeResultsScrew);
            Test.test1(AnalyzeResultsBoth);
            InterferenceResultAggregator.Instance.AggregateResults(InterferenceResultsList, AnalyzeResultsBoth );
            InterferenceResultAggregator.Instance.AggregateResults(InterferenceResultsList, AnalyzeResultsScrew);

            HighlightFunctionality.Instance.ComponentHighlight(InterferenceResultsList);

            var resultWindowViewModel = new ResultWindowViewModel(new ObservableCollection<ComponentData>(InterferenceResultsList));
            var resultWindow = new ResultWindow();
            resultWindow.DataContext = resultWindowViewModel; 
            resultWindow.Show();



        }
    }
}

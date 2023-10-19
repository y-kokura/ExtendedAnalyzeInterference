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
            //アセンブリドキュメント内のComponentOccurrenceを分類する。
            (ObjectCollection ScrewComponentCollection, ObjectCollection NonScrewComponentCollection, List<ComponentData> InterferenceResultsList) =OccurrenceCategorizer.Instance.StartCategorize();
            (InterferenceResults AnalyzeResultsBoth, InterferenceResults AnalyzeResultsScrew) =InterferenceAnalyzer.Instance.InterferenceAnalysisExcute(ScrewComponentCollection, NonScrewComponentCollection);
            

            InterferenceResultAggregator.Instance.AggregateResults(InterferenceResultsList, AnalyzeResultsBoth );
            InterferenceResultAggregator.Instance.AggregateResults(InterferenceResultsList, AnalyzeResultsScrew);

            var resultWindowViewModel = new ResultWindowViewModel(new ObservableCollection<ComponentData>(InterferenceResultsList));
            var resultWindow = new ResultWindow();
            resultWindow.DataContext = resultWindowViewModel;  // この行を追加
            resultWindow.Show();



        }
    }
}

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

namespace AnalyzeInterference.Models
{
    internal class ThreadInterferenceAnalysisWorkFlow
    {
        public ThreadInterferenceAnalysisWorkFlow()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public void RunInterferenceAnalysis()
        {
            //アセンブリドキュメント内のComponentOccurrenceを分類する(ネジを含む部品か含まない部品か)。
            (ObjectCollection ScrewComponentCollection, ObjectCollection NonScrewComponentCollection, List<ComponentData> InterferenceResultsList) =ComponentOccurrenceCollection.Instance.StartCategorize();

            //ネジを含む部品と含まない部品のそれぞれに対して干渉解析を実行する。
            (InterferenceResults AnalyzeResultsBoth, InterferenceResults AnalyzeResultsScrew) = OccurrenceCollectionInterferenceAnalysis.Instance.PerformInterferenceAnalysis(ScrewComponentCollection, NonScrewComponentCollection);

            //干渉解析の結果を集計する。
            InterferenceResultAggregatorTool.Instance.AggregateResults(InterferenceResultsList, AnalyzeResultsBoth );
            InterferenceResultAggregatorTool.Instance.AggregateResults(InterferenceResultsList, AnalyzeResultsScrew);

            //干渉解析の結果をハイライトする。
            ComponentHighlightTool.Instance.ApplyToAll(InterferenceResultsList);

            //干渉解析の結果を表示する。
            var resultWindowViewModel = new ResultWindowViewModel(new ObservableCollection<ComponentData>(InterferenceResultsList));
            var resultWindow = new ResultWindow();
            resultWindow.DataContext = resultWindowViewModel; 
            resultWindow.Show();



        }
    }
}

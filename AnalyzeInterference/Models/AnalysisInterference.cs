using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalyzeInterference;
using AnalyzeInterference.ViewModels;
using Inventor;
using System.Windows;
using System.Runtime.InteropServices;

namespace AnalyzeInterference.Models
{
     #region "InterferenceResults Class"
    internal interface IAnalysis
    {
        void AddCollection(ObjectCollection oCollection);
        InterferenceResults Analyze();
    }

    internal class ScrewInterference : IAnalysis
    {
        public void AddCollection(ObjectCollection oCollection)
        {
            // ここで具体的な解析コードを書く
        }
        public InterferenceResults Analyze()
        {
            InterferenceResults results = null;
            return results;
        }
    }

    internal class NonScrewInterference : IAnalysis
    {
        public void AddCollection(ObjectCollection oCollection)
        {
            // ここで具体的な解析コードを書く
        }
        public InterferenceResults Analyze()
        {
            InterferenceResults results = null;
            return results;
        }
    }
    internal class AllInterference : IAnalysis
    {
        public void AddCollection(ObjectCollection oCollection)
        {
            // ここで具体的な解析コードを書く
        }
        public InterferenceResults Analyze()
        {
            InterferenceResults results = null;
            return results;
        }
    }
    #endregion

        
    class AnalyzeInterference
    {
        // シングルトンのインスタンスを作成
        private static AnalyzeInterference _instance;
        public static AnalyzeInterference Instance => _instance ?? (_instance = new AnalyzeInterference());



        public static void Main()
        {
            if (Globals.InvApp.ActiveDocumentType != DocumentTypeEnum.kAssemblyDocumentObject)
            {
                MessageBox.Show("アセンブリドキュメントで実行してください。");
                return ;
            }

            Globals.ActiveInvDoc=Globals.InvApp.ActiveDocument;





            // ObjectCollectionを作成
            ObjectCollection oCollection = Globals.InvApp.TransientObjects.CreateObjectCollection();


            //// InterferenceAnalysisのインスタンスを作成して解析を実行
            //IAnalysis analysis1 = new ScrewInterference();
            //analysis1.AddCollection(oCollection);

            //// AnotherAnalysisのインスタンスを作成して解析を実行
            //IAnalysis analysis2 = new NonScrewInterference();
            //analysis2.AddCollection(oCollection);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventor;

namespace AlanaysisMainModule
{
    internal interface IAnalysis
    {
        void AddCollection(ObjectCollection oCollection);
         InterferenceResults  Analyze();
    }

    internal class ScrewInterference : IAnalysis
    {
        public void AddCollection(ObjectCollection oCollection)
        {
            // ここで具体的な解析コードを書く
        }
        public InterferenceResults Analyze()
        {
            InterferenceResults results =null; 
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






    class Program
    {
        static void Main(string[] args)
        {
            // InventorのApplicationオブジェクトを取得するコード（ダミー）
            Inventor.Application invApp = null;

            // ObjectCollectionを作成
            ObjectCollection oCollection = invApp.TransientObjects.CreateObjectCollection();








            // InterferenceAnalysisのインスタンスを作成して解析を実行
            IAnalysis analysis1 = new ScrewInterference();
            analysis1.AddCollection(oCollection);

            // AnotherAnalysisのインスタンスを作成して解析を実行
            IAnalysis analysis2 = new NonScrewInterference();
            analysis2.AddCollection(oCollection);
        }
    }
}
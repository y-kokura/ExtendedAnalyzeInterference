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
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Documents;

namespace AnalyzeInterference.Models
{

    public interface IAnalyzeTargets
    {
        ObjectCollection AllComponentCollection { get; set; }
        ObjectCollection ScrewComponentCollection { get; set; }
        ObjectCollection NonScrewComponentCollection { get; set; }
        List<ComponentData> InterferenceResultsList { get; set; }

        void Targets();
        void CategorizeOccurrence(ComponentOccurrence occurrence);
        bool LoopContinueCheck(ComponentOccurrence occurrence);
        void AddCollection(ComponentOccurrence occurrence, List<ComponentData> list );
        void AddSubOccurrence(ComponentOccurrence occurrence, ComponentData componentData);

    }

    public abstract class BaseAnalyzeComponent : IAnalyzeTargets
    {
        public ObjectCollection AllComponentCollection { get; set; }
        public ObjectCollection ScrewComponentCollection { get; set; }
        public ObjectCollection NonScrewComponentCollection { get; set; }
        // フィールドとして保持
        public List<ComponentData> _interferenceResultsList = new List<ComponentData>();

        // IAnalyzeTargets の契約を守るために、ゲッターとセッターの両方を提供
        public List<ComponentData> InterferenceResultsList
        {
            get { return _interferenceResultsList; }
            set { _interferenceResultsList = value; }
        }


        public BaseAnalyzeComponent()
        {
            AllComponentCollection = Globals.InvApp.TransientObjects.CreateObjectCollection();
            ScrewComponentCollection = Globals.InvApp.TransientObjects.CreateObjectCollection();
            NonScrewComponentCollection = Globals.InvApp.TransientObjects.CreateObjectCollection();
        }

        public abstract void Targets();

        public void CategorizeOccurrence(ComponentOccurrence occurrence)
        {
            if (LoopContinueCheck(occurrence)) return;
            int ThreadCount = FeaturePropertyCheck.ThreadFeatureCounter(occurrence);


            if (ThreadCount > 0)
            {
                ScrewComponentCollection.Add(occurrence);

                AddCollection(occurrence, InterferenceResultsList);

            }
            else if (ThreadCount <= 0)
            {
                if (occurrence.SubOccurrences.Count > 0)
                {
                    foreach (ComponentOccurrence subOccurrence in occurrence.SubOccurrences)
                    {
                        CategorizeOccurrence(subOccurrence);
                    }
                }
                else
                {
                    NonScrewComponentCollection.Add(occurrence);
                }
            }


        }
        public bool LoopContinueCheck(ComponentOccurrence occurrence)
        {
            // Check for kReferenceBOMStructure
            if (AnalyzeLogic.Instance.kReferenceBOM && occurrence.BOMStructure == BOMStructureEnum.kReferenceBOMStructure)
            {
                return true;
            }

            // Check for kPhantomBOMStructure
            if (AnalyzeLogic.Instance.kPhantomBOM && occurrence.BOMStructure == BOMStructureEnum.kPhantomBOMStructure)
            {
                return true;
            }

            // Check for Disabled Component
            if (AnalyzeLogic.Instance.Disable && !occurrence.Enabled)
            {
                return true;
            }

            // Check for Hidden Component
            if (AnalyzeLogic.Instance.Hidden && !occurrence.Visible)
            {
                return true;
            }

            // None of the conditions met
            return false;
        }


        //public abstract void AddCollection(ComponentOccurrence occurrence, List<ComponentData> list);
        public void AddCollection(ComponentOccurrence occurrence, List<ComponentData> list)
        {
            System.Array referenceKey = null;
            occurrence.GetReferenceKey(ref referenceKey);
            byte[] byteReferenceKey = (byte[])referenceKey;


            //string bitReferenceKey = BitConverter.ToString(referenceKey);

            int threadCount = FeaturePropertyCheck.ThreadFeatureCounter(occurrence);

            var foundItem = list.FirstOrDefault(t => ((byte[])t.ReferenceKey).SequenceEqual(byteReferenceKey));


            if (foundItem == null)
            {
                // Create a new ComponentData object
                ComponentData newComponentData = new ComponentData
                {
                    ComponentOccurrence = occurrence,
                    ReferenceKey = byteReferenceKey,
                    ThreadCount = threadCount,
                    TappdCount = 0,
                    InterferenceCount = 0,
                    InterferenceBodies = Globals.InvApp.TransientObjects.CreateObjectCollection(), // Assuming invapp is available globally
                    InterferenceOccurrences1 = new List<ComponentOccurrence>(),
                    InterferenceOccurrences2 = new List<ComponentOccurrence>(),
                    SubOccurrences = Globals.InvApp.TransientObjects.CreateObjectCollection(),
                    SubOccurrencesKey = new List<byte[]>(),
                    InterferenceCountType = "干渉数 : 0"
                };

                // Add the new data to the list
                list.Add(newComponentData);

                AddSubOccurrence(occurrence, newComponentData); // Assuming AddSubOccurrence is another method you have 
            }
        }


        public void AddSubOccurrence(ComponentOccurrence occurrence, ComponentData componentData)
        {
            if (LoopContinueCheck(occurrence))
                return;

            if (occurrence.SubOccurrences.Count > 0)
            {
                foreach (ComponentOccurrence subOccurrence in occurrence.SubOccurrences)
                {
                    AddSubOccurrence(subOccurrence, componentData);
                }
            }
            else
            {
                System.Array referenceKey = null;
                occurrence.GetReferenceKey(ref referenceKey);
                byte[] byteReferenceKey = (byte[])referenceKey;

                long tappedCount = FeaturePropertyCheck.TappedFeatureCounter(occurrence);

                componentData.TappdCount += tappedCount;
                componentData.SubOccurrences.Add(occurrence);
                componentData.SubOccurrencesKey.Add(byteReferenceKey);
            }
        }
    }


    public class AllComponent : BaseAnalyzeComponent
    {
        public override void Targets()
        {
            if (Globals.ActiveInvDoc.ComponentDefinition.Occurrences.Count <= 1) return;
            foreach (ComponentOccurrence occurrence in Globals.ActiveInvDoc.ComponentDefinition.Occurrences)
            {
                //AllComponentCollection.Add(occurrence);
                CategorizeOccurrence(occurrence);
            }
        
        }

    }


    public class SelectedComponent : BaseAnalyzeComponent
    {
        public override void Targets()
        {
            if (Globals.ActiveInvDoc.SelectSet.Count <= 1) return;

            foreach (ComponentOccurrence occurrence in Globals.ActiveInvDoc.SelectSet)
            {
                //AllComponentCollection.Add(occurrence);
                CategorizeOccurrence(occurrence);
            }

        }
    }



    public static class AnalyzeFactory
    {
        public static IAnalyzeTargets CreateAnalyzeInstance(bool allComponent, bool selectedComponent)
        {
            if (allComponent)
            {
                return new AllComponent();
            }
            else if (selectedComponent)
            {
                return new SelectedComponent();
            }
            throw new ArgumentException("解析対象の選択が不正です。");
        }
    }



    public class AnalyzeLogic
    {
        // シングルトンのインスタンスを作成
        private static AnalyzeLogic _instance;
        public static AnalyzeLogic Instance => _instance ?? (_instance = new AnalyzeLogic());

        #region"Propertys"
        public bool AllComponent { get; set; }
        public  bool SelectedComponent { get; set; }

        public  bool kReferenceBOM { get; set; }
        public  bool kPhantomBOM { get; set; }
        public  bool Disable { get; set; }
        public  bool Hidden { get; set; }
        #endregion


        public void RunAnalysis()
        {
            if (Globals.InvApp.ActiveDocumentType == DocumentTypeEnum.kAssemblyDocumentObject)
            {
                //MessageBox.Show("アセンブリドキュメントで実行してください。");
                return;
            }
            else
            {

            }

            //アクティブドキュメントを取得
            Globals.ActiveInvDoc = (Inventor.AssemblyDocument)Globals.InvApp.ActiveDocument;


            IAnalyzeTargets analysisInstance = AnalyzeFactory.CreateAnalyzeInstance(AllComponent, SelectedComponent);

            // 解析処理の実行
            analysisInstance.Targets();



        }
    }




    public class ComponentData
    {
        public ComponentOccurrence ComponentOccurrence { get; set; }

        public  byte[]  ReferenceKey{ get; set; }
        
        //public string BitReferenceKey { get; set; }
        public long ThreadCount { get; set; }
        public long TappdCount { get; set; }
        public long InterferenceCount { get; set; }
        public ObjectCollection InterferenceBodies { get; set; }
        public List<ComponentOccurrence> InterferenceOccurrences1 { get; set; }
        public List<ComponentOccurrence> InterferenceOccurrences2 { get; set; }
        public ObjectCollection SubOccurrences { get; set; }
        public List<byte[]> SubOccurrencesKey { get; set; }
        public string InterferenceCountType { get; set; }
    }


}
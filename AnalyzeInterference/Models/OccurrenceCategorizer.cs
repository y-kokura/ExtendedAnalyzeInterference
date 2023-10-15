using AnalyzeInterference.Common;
using Inventor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnalyzeInterference.Models
{
    internal class OccurrenceCategorizer
    {
        // シングルトンのインスタンスを作成
        private static OccurrenceCategorizer _instance;
        public static OccurrenceCategorizer Instance => _instance ?? (_instance = new OccurrenceCategorizer());

        #region"Propertys"
        public bool AllComponent { get; set; }
        public bool SelectedComponent { get; set; }

        public bool kReferenceBOM { get; set; }
        public bool kPhantomBOM { get; set; }
        public bool Disable { get; set; }
        public bool Hidden { get; set; }
        #endregion


        /// <summary>
        /// 干渉解析実行のために、解析対象のComponentOccurrenceをObjectCollectionにネジとネジ以外で分類します。
        /// </summary>
        public (ObjectCollection ScrewComponentCollection, ObjectCollection NonScrewComponentCollection) StartCategorize()
        {
            if (Globals.InvApp.ActiveDocumentType != DocumentTypeEnum.kAssemblyDocumentObject)
            {
                MessageBox.Show("アセンブリドキュメントで実行してください。");
                return (null,null);
            }

            //アクティブドキュメントを取得
            Globals.ActiveInvDoc = (Inventor.AssemblyDocument)Globals.InvApp.ActiveDocument;


            IAnalyzeTargets analysisInstance = CreateAnalyzeInstance(AllComponent, SelectedComponent);

            // 解析処理の実行
             return analysisInstance.ComponentOccurrenceCateforize();

        }


        /// <summary>
        /// 解析のターゲットとして必要な操作やプロパティを定義したインターフェース。
        /// </summary>
        public interface IAnalyzeTargets
        {
            ObjectCollection ScrewComponentCollection { get; set; }
            ObjectCollection NonScrewComponentCollection { get; set; }
            List<ComponentData> InterferenceResultsList { get; set; }

            (ObjectCollection ScrewComponentCollection, ObjectCollection NonScrewComponentCollection) ComponentOccurrenceCateforize();

        }

        /// <summary>
        /// IAnalyzeTargetsの基本的な実装を提供する抽象クラス。
        /// </summary>
        public abstract class BaseAnalyzeComponent : IAnalyzeTargets
        {
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
                ScrewComponentCollection = Globals.InvApp.TransientObjects.CreateObjectCollection();
                NonScrewComponentCollection = Globals.InvApp.TransientObjects.CreateObjectCollection();
            }


            public abstract (ObjectCollection ScrewComponentCollection, ObjectCollection NonScrewComponentCollection) ComponentOccurrenceCateforize();

        }

        /// <summary>
        /// 全てのコンポーネントに対して解析を行うクラス。
        /// </summary>
        public class AllAnalysisTarget : BaseAnalyzeComponent
        {
            public override (ObjectCollection ScrewComponentCollection, ObjectCollection NonScrewComponentCollection) ComponentOccurrenceCateforize()
            {
                if (Globals.ActiveInvDoc.ComponentDefinition.Occurrences.Count <= 1) return (ScrewComponentCollection, NonScrewComponentCollection); ;
                foreach (ComponentOccurrence occurrence in Globals.ActiveInvDoc.ComponentDefinition.Occurrences)
                {
                    OccurrenceCategorizer occurrenceCategorizer = new OccurrenceCategorizer();
                    occurrenceCategorizer.CategorizeOccurrence(occurrence, ScrewComponentCollection, NonScrewComponentCollection, InterferenceResultsList);
                }
                return (ScrewComponentCollection, NonScrewComponentCollection);
            }
        }


        /// <summary>
        /// 選択されたコンポーネントに対して解析を行うクラス。
        /// </summary>
        public class SelectedAnalysisTarget : BaseAnalyzeComponent
        {
            public override (ObjectCollection ScrewComponentCollection, ObjectCollection NonScrewComponentCollection) ComponentOccurrenceCateforize()
            {
                if (Globals.ActiveInvDoc.SelectSet.Count <= 1) return (ScrewComponentCollection, NonScrewComponentCollection); ;

                foreach (ComponentOccurrence occurrence in Globals.ActiveInvDoc.SelectSet)
                {
                    OccurrenceCategorizer occurrenceCategorizer = new OccurrenceCategorizer();
                    occurrenceCategorizer.CategorizeOccurrence(occurrence, ScrewComponentCollection, NonScrewComponentCollection, InterferenceResultsList);

                }
                return (ScrewComponentCollection, NonScrewComponentCollection);
            }
        }



        
            /// <summary>
            /// RadioButtonのユーザの選択に基づき、解析対象を生成する。
            /// </summary>
            /// <param name="allComponent">すべてのコンポーネントを解析する場合はtrue、それ以外はfalse。</param>
            /// <param name="selectedComponent">選択されたコンポーネントのみを解析する場合はtrue、それ以外はfalse。</param>
            /// <returns>解析対象のインスタンス。</returns>
            /// <exception cref="ArgumentException">適切な解析対象が選択されなかった場合にスローされます。</exception>
            public static IAnalyzeTargets CreateAnalyzeInstance(bool allComponent, bool selectedComponent)
            {
                if (allComponent)
                {
                    return new AllAnalysisTarget();
                }
                else if (selectedComponent)
                {
                    return new SelectedAnalysisTarget();
                }
                throw new ArgumentException("解析対象の選択が不正です。");
            }
 



        /// <summary>
        /// ComponentOccurrenceをネジとネジ以外に分類する    
        /// </summary>
        /// <param name="occurrence">分類対象のComponentOccurrence</param>
        /// <param name="ScrewComponentCollection">ネジのObjectCollection</param>
        /// <param name="NonScrewComponentCollection">ネジ以外のObjectCollection</param>
        /// <param name="InterferenceResultsList"></param>
        public void CategorizeOccurrence(ComponentOccurrence occurrence, ObjectCollection ScrewComponentCollection, ObjectCollection NonScrewComponentCollection, List<ComponentData> InterferenceResultsList)
        {
            if (LoopContinueCheck(occurrence)) return;
            int ThreadCount = FeaturePropertyChecker.ThreadFeatureCounter(occurrence);


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
                        CategorizeOccurrence(occurrence, ScrewComponentCollection, NonScrewComponentCollection, InterferenceResultsList);
                    }
                }
                else
                {
                    NonScrewComponentCollection.Add(occurrence);
                }
            }


        }


        /// <summary>
        /// コンポーネントが解析対象かどうかを判定する
        /// </summary> 
        /// <param name="occurrence">解析対象となるComponentOccurrence</param>
        /// <returns>true : 非解析対象　false : 解析対象</returns>
        private bool LoopContinueCheck(ComponentOccurrence occurrence)
        {
            //BOMstructureの状態判定
            if (kReferenceBOM && occurrence.BOMStructure == BOMStructureEnum.kReferenceBOMStructure) return true;
            if (kPhantomBOM && occurrence.BOMStructure == BOMStructureEnum.kPhantomBOMStructure) return true;

            //表示・有効の状態判定
            if (Disable && !occurrence.Enabled) return true;
            if (Hidden && !occurrence.Visible) return true;

            //どれにも該当しない場合は、false(解析対象)
            return false;
        }


        /// <summary>
        /// コンポーネントの情報を元に、指定されたリストに新しいComponentDataオブジェクトを追加します。既にリストに同じReferenceKeyを持つComponentDataが存在する場合は追加しません。
        /// </summary>
        /// <param name="occurrence">追加または検証されるComponentOccurrenceオブジェクト。</param>
        /// <param name="list">新しいComponentDataオブジェクトを追加する対象のリスト。</param>
        private void AddCollection(ComponentOccurrence occurrence, List<ComponentData> list)
        {
            byte[] referenceKey = new byte[] { };
            occurrence.GetReferenceKey(referenceKey);


            int threadCount = FeaturePropertyChecker.ThreadFeatureCounter(occurrence);
            var foundItem = list.FirstOrDefault(t => t.ReferenceKey.SequenceEqual(referenceKey));


            if (foundItem == null)
            {
                ComponentData newComponentData = new ComponentData
                {
                    ComponentOccurrence = occurrence,
                    ReferenceKey = referenceKey,
                    ThreadCount = threadCount,
                    TappdCount = 0,
                    InterferenceCount = 0,
                    InterferenceBodies = Globals.InvApp.TransientObjects.CreateObjectCollection(),
                    InterferenceOccurrences1 = new List<ComponentOccurrence>(),
                    InterferenceOccurrences2 = new List<ComponentOccurrence>(),
                    SubOccurrences = Globals.InvApp.TransientObjects.CreateObjectCollection(),
                    SubOccurrencesKey = new List<byte[]>(),
                };

                // Add the new data to the list
                list.Add(newComponentData);

                AddSubOccurrence(occurrence, newComponentData);
            }
        }


        /// <summary>
        /// 指定されたComponentOccurrenceのサブオカレンスを再帰的に処理し、関連する情報をComponentDataオブジェクトに追加します。
        /// </summary>
        /// <param name="occurrence">処理するComponentOccurrenceオブジェクト。</param>
        /// <param name="componentData">関連する情報を追加するComponentDataオブジェクト。</param>
        private void AddSubOccurrence(ComponentOccurrence occurrence, ComponentData componentData)
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
                byte[] referenceKey = new byte[] { };
                occurrence.GetReferenceKey(referenceKey);

                long tappedCount = FeaturePropertyChecker.TappedFeatureCounter(occurrence);

                componentData.TappdCount += tappedCount;
                componentData.SubOccurrences.Add(occurrence);
                componentData.SubOccurrencesKey.Add(referenceKey);
            }
        }

    }

}

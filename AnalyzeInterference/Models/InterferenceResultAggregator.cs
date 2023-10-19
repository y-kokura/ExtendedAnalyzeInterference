using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Documents;
using Inventor;

namespace AnalyzeInterference.Models
{
    internal class InterferenceResultAggregator
    {
        private static InterferenceResultAggregator _instance;
        public static InterferenceResultAggregator Instance => _instance ?? (_instance = new InterferenceResultAggregator());

        /// <summary>
        /// 与えられたInterferenceResultsから集計を行い、resultDataListを更新します。
        /// </summary>
        /// <param name="resultDataList">集計用のリスト</param>
        /// <param name="interferenceResults">解析対象の干渉結果</param>
        public void AggregateResults(List<ComponentData> resultDataList, InterferenceResults interferenceResults)
        {
            if (interferenceResults == null)
            {
                return;
            }

            foreach (InterferenceResult result in interferenceResults)
            {
                ComponentOccurrence firstOccurrence = result.OccurrenceOne;
                ComponentOccurrence secondOccurrence = result.OccurrenceTwo;

                AddOrUpdateResult(firstOccurrence, secondOccurrence, result.InterferenceBody, resultDataList);
            }
        }

        /// <summary>
        /// 与えられたComponentOccurrenceとSurfaceBodyを用いて、resultDataListを更新または追加します。
        /// </summary>
        /// <param name="firstOccurrence">InterferenceResult.OccurrenceOne</param>
        /// <param name="secondOccurrence">InterferenceResult.OccurrenceTwo</param>
        /// <param name="surfaceBody">InterferenceResult.InterferenceBody</param>
        /// <param name="resultDataList">集計用のリスト</param>
        private void AddOrUpdateResult(ComponentOccurrence firstOccurrence, ComponentOccurrence secondOccurrence, SurfaceBody surfaceBody, List<ComponentData> resultDataList)
        {
            var foundItem1 = FindItemByReferenceKey(firstOccurrence, resultDataList);
            var foundItem2 = FindItemByReferenceKey(secondOccurrence, resultDataList);

            if (foundItem1 != null && foundItem2 != null && ReferenceEquals(foundItem1, foundItem2))
            {
                UpdateInterferenceData(foundItem1, firstOccurrence, secondOccurrence, surfaceBody);
                return;
            }
            else
            {
                UpdateInterferenceData(foundItem1, firstOccurrence, secondOccurrence, surfaceBody);
                UpdateInterferenceData(foundItem2, firstOccurrence, secondOccurrence, surfaceBody);
            }
        }


        /// <summary>
        /// 与えられたComponentOccurrenceに対応するComponentDataをresultDataListから検索します。
        /// </summary>
        /// <param name="occurrence">検索対象のComponentOccurrence</param>
        /// <param name="componentDataList">集計用のリスト</param>
        /// <returns>見つかった場合はComponentData、見つからない場合はnull。</returns>
        private ComponentData FindItemByReferenceKey(ComponentOccurrence occurrence, List<ComponentData> componentDataList)
        {
            //byte[] referenceKey = new byte[] { };
            //occurrence.GetReferenceKey(referenceKey);

            System.Array tempArray = new byte[1];  // 一時的な System.Array
            occurrence.GetReferenceKey(ref tempArray);  // ref キーワードを使用
            byte[] referenceKey = (byte[])tempArray;  // System.Array を byte[] にキャスト

            Debug.Print(occurrence.Name);
            Debug.Print(BitConverter.ToString(referenceKey));



            var foundItem = componentDataList.FirstOrDefault(t => t.ReferenceKey.SequenceEqual(referenceKey));            
            return foundItem ?? componentDataList.FirstOrDefault(t => t.SubOccurrencesKey != null && t.SubOccurrencesKey.Any(arr => arr.SequenceEqual(referenceKey)));
            
            //var foundItem = componentDataList.FirstOrDefault(t => t.ReferenceKey == referenceKey);
            //return foundItem ?? componentDataList.FirstOrDefault(t => t.SubOccurrencesKey.Contains(referenceKey));
        }


        /// <summary>
        /// 与えられたComponentDataとComponentOccurrences、SurfaceBodyを用いて、ComponentDataの各フィールドを更新します。
        /// </summary>
        /// <param name="item">更新されるComponentData。</param>
        /// <param name="firstOccurrence">InterferenceResult.OccurrenceOne</param>
        /// <param name="secondOccurrence">InterferenceResult.OccurrenceTwo</param>
        /// <param name="surfaceBody">InterferenceResult.InterferenceBody</param>
        private void UpdateInterferenceData(ComponentData item, ComponentOccurrence firstOccurrence, ComponentOccurrence secondOccurrence, SurfaceBody surfaceBody)
        {
            if (item == null) return;

            item.InterferenceCount++;
            item.InterferenceBodies.Add(surfaceBody);
            item.InterferenceOccurrences1.Add(firstOccurrence);
            item.InterferenceOccurrences2.Add(secondOccurrence);
        }

    }
}

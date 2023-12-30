using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Documents;
using Inventor;

namespace AnalyzeInterference.Models
{
    internal class InterferenceResultAggregatorTool
    {
        private static InterferenceResultAggregatorTool _instance;
        public static InterferenceResultAggregatorTool Instance => _instance ?? (_instance = new InterferenceResultAggregatorTool());

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

            foreach (InterferenceResult interferenceResult in interferenceResults)
            {
                DistributeResults(interferenceResult, resultDataList);
            }
        }

        /// <summary>
        /// 与えられたComponentOccurrenceとSurfaceBodyを用いて、resultDataListを更新または追加します。
        /// </summary>
        /// <param name="firstOccurrence">InterferenceResult.OccurrenceOne</param>
        /// <param name="secondOccurrence">InterferenceResult.OccurrenceTwo</param>
        /// <param name="surfaceBody">InterferenceResult.InterferenceBody</param>
        /// <param name="resultDataList">集計用のリスト</param>
        private void DistributeResults(InterferenceResult interferenceResult, List<ComponentData> resultDataList)
        {
            ComponentOccurrence firstOccurrence = interferenceResult.OccurrenceOne;
            ComponentOccurrence secondOccurrence = interferenceResult.OccurrenceTwo;
            SurfaceBody surfaceBody = interferenceResult.InterferenceBody;


            var foundItem1 = FindItemByReferenceKey(firstOccurrence, resultDataList);
            var foundItem2 = FindItemByReferenceKey(secondOccurrence, resultDataList);

            if (foundItem1 != null && foundItem2 != null && ReferenceEquals(foundItem1, foundItem2))
            {
                AddComponentData(foundItem1, interferenceResult);
                return;
            }
            else
            {
                AddComponentData(foundItem1, interferenceResult);
                AddComponentData(foundItem2, interferenceResult);
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

            //Debug.Print(occurrence.Name);
            //Debug.Print(BitConverter.ToString(referenceKey));



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
        private void AddComponentData(ComponentData item, InterferenceResult interferenceResult)
        {
            if (item == null) return;

            ComponentOccurrence firstOccurrence = interferenceResult.OccurrenceOne;
            ComponentOccurrence secondOccurrence = interferenceResult.OccurrenceTwo;
            SurfaceBody surfaceBody = interferenceResult.InterferenceBody;
            double volume=interferenceResult.Volume;

            if (IsThreadTypeInterference(surfaceBody, volume)) item.ThreadTypeInterferenceCount++;
            item.InterferenceCount++;
            item.InterferenceBodies.Add(surfaceBody);
            item.InterferenceOccurrences1.Add(firstOccurrence);
            item.InterferenceOccurrences2.Add(secondOccurrence);
        }

        private bool IsThreadTypeInterference(SurfaceBody surfaceBody , Double interferenceVolume)
        {
            OrientedBox orientedBox = surfaceBody.OrientedMinimumRangeBox;
            double lengthOne = orientedBox.DirectionOne.Length;
            double lengthTwo= orientedBox.DirectionTwo.Length;
            double lengthThree=orientedBox.DirectionThree.Length;

            if (!AreTwoValuesAlmostEqual(lengthOne, lengthTwo, lengthThree)) return false;

            var biggestVolume = Math.PI* lengthOne* lengthTwo* lengthThree / 4 ;
            var volumeRatio = interferenceVolume / biggestVolume;

            return volumeRatio >= 0.2 && volumeRatio <= 0.4;
        }

        private bool AreTwoValuesAlmostEqual(double a, double b, double c, double tolerance = 1e-6)
        {
            return Math.Abs(a - b) < tolerance || Math.Abs(a - c) < tolerance || Math.Abs(b - c) < tolerance;
        }

    }
}

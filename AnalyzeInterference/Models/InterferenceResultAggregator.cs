using System;
using System.Collections.Generic;
using System.Linq;
using Inventor;

namespace AnalyzeInterference.Models
{
    internal class InterferenceResultAggregator
    {
        public void AggregateResults(List<ComponentData> componentDataList, InterferenceResults interferenceResults)
        {
            if (interferenceResults == null)
            {
                return;
            }

            foreach (var result in interferenceResults)
            {
                var firstOccurrence = result.OccurrenceOne;
                var secondOccurrence = result.OccurrenceTwo;

                AddOrUpdateResult(firstOccurrence, secondOccurrence, result.InterferenceBody, componentDataList);
            }
        }

        private void AddOrUpdateResult(ComponentOccurrence firstOccurrence, ComponentOccurrence secondOccurrence, SurfaceBody surfaceBody, List<ComponentData> componentDataList)
        {
            var foundItem1 = FindItemByReferenceKey(firstOccurrence, componentDataList);
            var foundItem2 = FindItemByReferenceKey(secondOccurrence, componentDataList);

            if (foundItem1 != null && foundItem2 != null && ReferenceEquals(foundItem1, foundItem2))
            {
                UpdateInterferenceData(foundItem1, firstOccurrence, secondOccurrence, surfaceBody);
                return;
            }

            UpdateInterferenceData(foundItem1, firstOccurrence, secondOccurrence, surfaceBody);
            UpdateInterferenceData(foundItem2, firstOccurrence, secondOccurrence, surfaceBody);
        }

        private ComponentData FindItemByReferenceKey(ComponentOccurrence occurrence, List<ComponentData> componentDataList)
        {
            byte[] referenceKey = GetReferenceKey(occurrence);
            string bitReferenceKey = BitConverter.ToString(referenceKey);

            var foundItem = componentDataList.FirstOrDefault(t => t.BitReferenceKey == bitReferenceKey);
            return foundItem ?? componentDataList.FirstOrDefault(t => t.SubOccurrencesKey.Contains(bitReferenceKey));
        }

        private byte[] GetReferenceKey(ComponentOccurrence occurrence)
        {
            // Assuming this method fetches the reference key for the given occurrence.
            // Replace this with actual code to get the reference key.
            return new byte[0];
        }

        private void UpdateInterferenceData(ComponentData item, ComponentOccurrence firstOccurrence, ComponentOccurrence secondOccurrence, SurfaceBody surfaceBody)
        {
            if (item == null) return;

            item.InterferenceCount++;
            item.InterferenceBodies.Add(surfaceBody);
            item.InterferenceOccurrences1.Add(firstOccurrence);
            item.InterferenceOccurrences2.Add(secondOccurrence);
            item.InterferenceCountType = DetermineInterferenceCountType(item);
        }

        private string DetermineInterferenceCountType(ComponentData item)
        {
            // Implementation of CheckInterferenceCountType logic here.
            return "SomeStringBasedOnLogic";
        }
    }
}

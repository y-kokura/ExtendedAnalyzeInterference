using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventor;

namespace AnalyzeInterference.Models
{
    internal class HoleFeatureCountTool
    {

        /// <summary>
        /// ComponentOccurrenceに含まれるの雄ネジの要素数をカウントします。
        /// </summary>
        /// <param name="occurrence">カウント対象のComponentOccurrence</param>
        /// <returns>雄ネジの要素数を返します。</returns>
        public static int ThreadFeatures(ComponentOccurrence occurrence)
        {
            int ThreadCount = 0;
            ComponentDefinition compDef = occurrence.Definition; 

            if (compDef is AssemblyComponentDefinition assemblyDef)
            {
                return assemblyDef.Features.ThreadFeatures.Count;

            }
            else if (compDef is PartComponentDefinition partDef)
            {
                return partDef.Features.ThreadFeatures.Count;
            }
            else
            {
                return ThreadCount;
            }
            
        }

        /// <summary>
        /// ComponentOccurrenceに含まれるの雌ネジの要素数をカウントします。
        /// </summary>
        /// <param name="occurrence">カウント対象のComponentOccurrence</param>
        /// <returns>雌ネジの要素数を返します。</returns>
        public static int TappedFeatures(ComponentOccurrence occurrence)
        {
            int HoleCount = 0;
            ComponentDefinition compDef = occurrence.Definition;
            
            if(compDef is AssemblyComponentDefinition assemblyDef)
            {
                return assemblyDef.Features.HoleFeatures.Count;
            }
            else if(compDef is PartComponentDefinition partDef)
            {
                return partDef.Features.HoleFeatures.Count;
            }
            else
            {
                return HoleCount;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventor;

namespace AnalyzeInterference.Models
{
    internal class FeaturePropertyCheck
    {
        public static int ThreadFeatureCounter(ComponentOccurrence occurrence)
        {
            int ThreadCount = 0;
            ComponentDefinition compDef = occurrence.Definition; 

            if (compDef is AssemblyComponentDefinition assemblyDef)
            {
                return assemblyDef.Features.ThreadFeatures.Count;
                //if (assemblyDef.Features.ThreadFeatures.Count > 0)
                //{
                //    foreach (ThreadFeature threadFeature in assemblyDef.Features.ThreadFeatures)
                //    {
                //        threadFeature.
                //        if (oThread.ThreadType == ThreadTypeEnum.kThreadGeneral)
                //        {
                //            ThreadCount += 1;
                //        }
                //    }
                //}
            }
            else if (compDef is PartComponentDefinition partDef)
            {
                return partDef.Features.ThreadFeatures.Count;
                //if (partDef.Features.ThreadFeatures.Count > 0)
                //{
                //    foreach (ThreadFeature threadFeature in partDef.Features.ThreadFeatures)
                //    {
                //        if (oThread.ThreadType == ThreadTypeEnum.kThreadGeneral)
                //        {
                //            ThreadCount += 1;
                //        }
                //    }
                //}
            }
            else
            {
                return ThreadCount;
            }
            
        }
        public static int TappedFeatureCounter(ComponentOccurrence occurrence)
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



            //foreach (HoleFeature holeFeature in occurrence.Definition.Features.HoleFeatures){

            //}





            //if (oOcc.HoleFeatures.Count > 0)
            //{
            //    foreach (HoleFeature oHole in oOcc.HoleFeatures)
            //    {
            //        if (oHole.ThreadType == ThreadTypeEnum.kThreadGeneral)
            //        {
            //            HoleCount += 1;
            //        }
            //    }
            //}
            //return HoleCount;

        }
    }
}

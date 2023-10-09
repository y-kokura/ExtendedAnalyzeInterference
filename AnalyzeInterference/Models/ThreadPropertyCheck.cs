using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventor;

namespace AnalyzeInterference.Models
{
    internal class FeaturePropertyCheck
    {
        public static int ThreadFeatureCounter(ComponentOccurrence oOcc)
        {
            int ThreadCount = 0;
            if (oOcc.Definition.featureThreadFeatures.Count > 0)
            {
                foreach (ThreadFeature oThread in oOcc.ThreadFeatures)
                {
                    if (oThread.ThreadType == ThreadTypeEnum.kThreadGeneral)
                    {
                        ThreadCount += 1;
                    }
                }
            }
            return ThreadCount;
        }
        public static int TappedFeatureCounter(ComponentOccurrence oOcc)
        {
            int HoleCount = 0;
            if (oOcc.HoleFeatures.Count > 0)
            {
                foreach (HoleFeature oHole in oOcc.HoleFeatures)
                {
                    if (oHole.ThreadType == ThreadTypeEnum.kThreadGeneral)
                    {
                        HoleCount += 1;
                    }
                }
            }
            return HoleCount;
        }
    }
}

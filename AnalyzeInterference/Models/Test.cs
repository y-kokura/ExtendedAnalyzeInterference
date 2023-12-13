using Inventor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzeInterference.Models
{
    public class Test
    {
        public static void test1(InterferenceResults results)
        {
            if (results == null) return;
            foreach (InterferenceResult item in results)
            {
               var rangeBox = item.InterferenceBody.OrientedMinimumRangeBox;
                var sumVolume = rangeBox.DirectionOne.Length * rangeBox.DirectionTwo.Length * rangeBox.DirectionThree.Length / 4 * Math.PI;
               Debug.Print("Name:{0}{1}{2}", item.OccurrenceOne.Name,
                   System.Environment.NewLine,item.Volume/ sumVolume );
            }
        }
    }
}

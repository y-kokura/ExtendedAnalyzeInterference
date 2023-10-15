using Inventor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzeInterference.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class ComponentData
    {
        public ComponentOccurrence ComponentOccurrence { get; set; }
        public byte[] ReferenceKey { get; set; }
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

    public class ResultData
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<string> DataList { get; set; }

        public ResultData()
        {
            DataList = new List<string>();
        }

        public void AddData(string newData)
        {
            DataList.Add(newData);
        }
    }

}

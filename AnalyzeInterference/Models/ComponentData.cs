using Inventor;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
        public long TappedCount { get; set; }
        public long InterferenceCount { get; set; }
        public long ThreadTypeInterferenceCount { get;set; }
        public ObjectCollection InterferenceBodies { get; set; }
        public List<ComponentOccurrence> InterferenceOccurrences1 { get; set; }
        public List<ComponentOccurrence> InterferenceOccurrences2 { get; set; }
        public ObjectCollection SubOccurrences { get; set; }
        public List<byte[]> SubOccurrencesKey { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.Common
{
    public class CompareReport
    {
        public List<string> PropertiesAdded { get; set; }
        public List<string> PropertiesRemoved { get; set; }
        public Dictionary<string, string[]> PropertiesChanged { get; set; }

        public CompareReport()
        {
            PropertiesAdded = new List<string>();
            PropertiesChanged = new Dictionary<string, string[]>();
            PropertiesRemoved = new List<string>();
        }
    }
}

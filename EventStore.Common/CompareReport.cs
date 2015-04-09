using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.Common
{
    public class CompareReport
    {
        public Dictionary<string, object[]> PropertiesChanged { get; set; }

        public CompareReport()
        {
            PropertiesChanged = new Dictionary<string, object[]>();
        }
    }
}

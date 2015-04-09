using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.Common
{
    public class Hospital : AbstractEventObject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string CountyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
        public string Ownership { get; set; }
        public bool EmergencyServices { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventStore.Common;

namespace EventStore.Web.Models
{
    public class ViewHistoryModel
    {
        public string HospitalId { get; set; }
        public string CollectionName { get; set; }
        public Hospital Current { get; set; }
        public List<Hospital> HospitalHistory { get; set; } 
    }
}
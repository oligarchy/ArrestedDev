using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventStore.Common;

namespace EventStore.Web.Models
{
    public class ViewEntityDetailsModel
    {
        public string CollectionName { get; set; }
        public Hospital HospitalDetails { get; set; }
    }
}
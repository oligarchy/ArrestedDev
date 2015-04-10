using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventStore.Common;

namespace EventStore.Web.Models
{
    public class ViewEntitiesModel
    {
        public int PageNumber { get; set; }
        public string CollectionName { get; set; }
        public List<Hospital> Hospitals { get; set; }
    }
}
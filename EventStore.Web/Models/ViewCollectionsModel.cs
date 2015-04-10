using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventStore.Web.Models
{
    public class ViewCollectionsModel
    {
        public List<string> Collections { get; set; }

        public ViewCollectionsModel()
        {
            Collections = new List<string>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventStore.Common;

namespace EventStore.Web.Models
{
    public class LoginHistoryModel
    {
        public List<Login> Logins { get; set; }
    }
}
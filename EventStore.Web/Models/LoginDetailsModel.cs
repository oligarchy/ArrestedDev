using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventStore.Common;

namespace EventStore.Web.Models
{
    public class LoginDetailsModel
    {
        public string Username { get; set; }
        public List<Login> Logins { get; set; }
    }
}
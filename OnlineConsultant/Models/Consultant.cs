using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineConsultant.Models
{
    public class Consultant
    {
        public UserDetail User { get; set; }

        public UserDetail HandlingUser { get; set; }

        public bool IsFree { get; set; }
    }
}
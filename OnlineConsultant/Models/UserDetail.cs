using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineConsultant.Models
{
    public class UserDetail
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsSpec { get; set; }

        public string ConnectionId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineConsultant.Models
{
    public class QuestionUserModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

        public bool IsOpen { get; set; }

        public string CreateTime { get; set; }
    }
}
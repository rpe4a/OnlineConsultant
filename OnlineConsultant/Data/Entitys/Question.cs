using System;

namespace OnlineConsultant.Data.Entitys
{
    public enum QuestionOfState
    {
        Open,
        Close
    }
    
    public class Question
    {
        public int Id { get; set; }
 
        public string Description { get; set; }

        public DateTime CreateTime { get; set; }

        public QuestionOfState State { get; set; }

        public virtual User User { get; set; }
    }
}
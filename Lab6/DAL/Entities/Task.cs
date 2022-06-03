using System;
using System.Collections.Generic;

namespace DAL
{
    public interface ITask
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ResposibleEmloyeeID { get; set; }
        public DateTime CreationDate { get; set; }
        public TaskState State { get; set; }
        public string Comments { get; set; }
    }

    public class Task : ITask
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public TaskState State { get; set; }

        public string Description { get; set; }

        public string Comments { get; set; }

        public DateTime CreationDate { get; set; }

        public string ResposibleEmloyeeID { get; set; }

        public Task(string id, string name, string description, string resposibleEmloyeeID,
            DateTime creationDate, TaskState state, string comments)
        {
            ID = id;
            Name = name;
            Description = description;
            ResposibleEmloyeeID = resposibleEmloyeeID;
            CreationDate = creationDate;
            State = state;
            Comments = comments;
        }
    }

}

using System;
using System.Collections.Generic;
using BLL;
using DAL;

namespace NEWW
{
    public interface ITaskModel
    {
        public string Name { get; }
        public string Description { get; }
        public string ResposibleEmloyeeID { get; }
        public DateTime CreationDate { get; }
        public TaskState State { get; }
        public string Comments { get; }
    }

    public class TaskModel : ITaskModel
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        public string ResposibleEmloyeeID { get; private set; }

        public DateTime CreationDate { get; private set; }

        public TaskState State { get; private set; }

        public string Comments { get; private set; }

        public TaskModel(string name, string description, string resposibleEmloyee)
        {
            Name = name;
            Description = description;
            ResposibleEmloyeeID = resposibleEmloyee;
        }

        public TaskModel(ITaskDTO task)
        {
            Name = task.Name;
            Description = task.Description;
            ResposibleEmloyeeID = task.ResposibleEmloyeeID;
            CreationDate = task.CreationDate;
            State = task.State;
            Comments = task.Comments;
        }
    }

    

}

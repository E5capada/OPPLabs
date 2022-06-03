using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public interface ITaskDTO
    {
        public string Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string ResposibleEmloyeeID { get; }
        public DateTime CreationDate { get; }
        public TaskState State { get; }
        public string Comments { get; }

        public void ChangeDescription(string description);
        public void ChangeResposible(string responsibleID);
        public void ChangeState(TaskState state);
        public void AddComment(string comment);

    }

    public class TaskDTO : ITaskDTO
    {
        public string Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public string ResposibleEmloyeeID { get; private set; }

        public DateTime CreationDate { get; private set; }

        public TaskState State { get; private set; }

        public String Comments { get; private set; }

        public TaskDTO(string name, string description, string resposibleEmloyee)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Description = description;
            ResposibleEmloyeeID = resposibleEmloyee;
            CreationDate = DateTime.Now;
        }

        public TaskDTO(ITask task)
        {
            Id = task.ID;
            Name = task.Name;
            Description = task.Description;
            ResposibleEmloyeeID = task.ResposibleEmloyeeID;
            CreationDate = task.CreationDate;
            State = task.State;
            Comments = task.Comments;
        }

        public void AddComment(string comment)
        {
            Comments += comment + " ";
        }

        public void ChangeDescription(string description)
        {
            Description = description;
        }

        public void ChangeResposible(string responsibleID)
        {
            ResposibleEmloyeeID = responsibleID;
        }

        public void ChangeState(TaskState state)
        {
            State = state;
        }
    }
}

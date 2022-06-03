using System;
using System.Collections.Generic;
using System.Linq;
using DAL;

namespace BLL
{
    public interface ITaskToDoService
    {
        //ADD
        void AddTask(TaskDTO task);

        //GET
        IEnumerable<ITaskDTO> GetAllTasks();
        ITaskDTO GetTaskByID(string taskID);
        IEnumerable<ITaskDTO> GetTaskByCreationDate(DateTime date);
        IEnumerable<ITaskDTO> GetTaskByResponsible(string responsibleID);
        IEnumerable<ITaskDTO> GetTasksByDate(DateTime date);
        ITaskDTO GetTaskByLastChange();
        IEnumerable<ITaskDTO> GetTasksThisMemberChange(string memberID);
        IEnumerable<ITaskDTO> GetTasksOfEmployees(IEnumerable<IStaffMemberDTO> emplloyees);

        //UPDATE
        void ChangeResponsible(string taskID, string memberID);
        void ChangeState(string taskID, TaskState state);
        void AddComment(string memberID, string taskID, string comment);
        LinkedList<IModificationDTO> GetHistory();
    }

    public class TaskToDoService : ITaskToDoService
    {
        History tasks = new History();

        //IUnitOfWork Database { get; set; }
        private ITaskRepository<Task> Database = new TaskRepository(new Context("default"));

        public TaskToDoService(IUnitOfWork uow)
        {
            //Database = uow;
        }

        public void AddTask(TaskDTO task)
        {
            var newTask = new Task(task.Id, task.Name, task.Description, task.ResposibleEmloyeeID, task.CreationDate, task.State, task.Comments);
            Database.CreateTask(newTask);
            //Database.Save();
        }

        public Task ConvertToDal(TaskDTO task)
        {
            return new Task(task.Id, task.Name, task.Description, task.ResposibleEmloyeeID, task.CreationDate, task.State, task.Comments);
        }

        //GET
        public IEnumerable<ITaskDTO> GetAllTasks()
        {
            var tasks = Database.GetAll();
            var list = new List<ITaskDTO>();

            foreach (var task in tasks)
            {
                list.Add(new TaskDTO(task));
            }

            if (list.Count > 0)
            {
                return list;
            }
            else throw new Exception("We don not find enything :(");
        }

        public ITaskDTO GetTaskByID(string taskID)
        {
            var task = Database.Get(taskID);
            return new TaskDTO(task);
        }

        public IEnumerable<ITaskDTO> GetTaskByCreationDate(DateTime date)
        {
            var tasks = Database.GetAll();
            var list = new List<ITaskDTO>();
            foreach (var task in tasks)
            {
                if (task.CreationDate.Date == date.Date)
                {
                    list.Add(new TaskDTO(task));
                }
            }
            return list;
        }

        public IEnumerable<ITaskDTO> GetTaskByResponsible(string responsibleID)
        {
            var tasks = Database.GetAll();
            var list = new List<ITaskDTO>();
            foreach (var task in tasks)
            {
                if (task.ResposibleEmloyeeID == responsibleID)
                {
                    list.Add(new TaskDTO(task));
                }
            }
            return list;
        }

        public void AddComment(string memberID, string taskID, string comment)
        {
            var task = GetTaskByID(taskID);
            if (task.State != TaskState.Resolved)
            {
                task.AddComment(comment);
                Database.Update(ConvertToDal((TaskDTO)task));
                tasks.modifications.AddLast(new TaskNewCommentDTO(memberID, task.Id, comment));
            }
            else throw new Exception("Task is resolved :(");
        }

        public void ChangeResponsible(string taskID, string memberID)
        {
            var task = GetTaskByID(taskID);
            var prevResponsible = task.ResposibleEmloyeeID;
            task.ChangeResposible(memberID);
            Database.Update(ConvertToDal((TaskDTO)task));
            tasks.modifications.AddLast(new TaskNewResponsibleDTO(task.Id, task.ResposibleEmloyeeID, prevResponsible));
        }

        public void ChangeState(string taskID, TaskState state)
        {
            var task = GetTaskByID(taskID);
            var prevState = task.State;
            task.ChangeState(state);
            Database.Update(ConvertToDal((TaskDTO)task));
            
            tasks.modifications.AddLast(new TaskChangedStateDTO(task.ResposibleEmloyeeID, task.Id, task.State, prevState));
        }

        public IEnumerable<ITaskDTO> GetTasksOfEmployees(IEnumerable<IStaffMemberDTO> emplloyees)
        {
            var result = new List<TaskDTO>();
            foreach (var emp in emplloyees)
            {
                var tasks = GetTaskByResponsible(emp.ID);
                foreach (var task in tasks)
                {
                    result.Add((TaskDTO)task);
                }
            }
            if (result.Count > 0)
            {
                return result;
            }
            else throw new Exception("Your employess do not have tasks");
        }

        public IEnumerable<ITaskDTO> GetTasksByDate(DateTime date)
        {
            var result = new List<TaskDTO>();
            if (tasks.modifications.Count > 0)
            {
                var modOfTasks = tasks.modifications.Where(t => t.Date.Day == date.Day);
                foreach (var mod in modOfTasks)
                {
                    var task = (TaskDTO)GetTaskByID(mod.TaskID);
                    if (result.Find(t => t.Id == task.Id) == null)
                    {
                        result.Add(task);
                    }
                }
                return result;
            }
            else throw new Exception("Tasks were not changed");
        }

        public ITaskDTO GetTaskByLastChange()
        {
            if (tasks.modifications.Count > 0)
            {
                var taskID = tasks.modifications.Last().TaskID;
                var task = GetTaskByID(taskID);
                return task;
            }
            else throw new Exception("Tasks were not changed");
        }

        public IEnumerable<ITaskDTO> GetTasksThisMemberChange(string memberID)
        {
            var result = new List<TaskDTO>();
            if (tasks.modifications.Count > 0)
            {
                var modOftasks = tasks.modifications.Where(t => t.StaffMemberID == memberID);
                foreach (var mod in modOftasks)
                {
                    var task = (TaskDTO)GetTaskByID(mod.TaskID);
                    if (!result.Exists(t => t.Id == task.Id))
                    {
                        result.Add(task);
                    }
                }
                return result;
            }
            else throw new Exception("Tasks were not changed");
        }

        public LinkedList<IModificationDTO> GetHistory()
        {
            return tasks.modifications;
        }
    }
}

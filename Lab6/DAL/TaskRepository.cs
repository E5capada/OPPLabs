using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace DAL
{
    public interface ITaskRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(string Id);

        void CreateTask(T task);
        void Update(T task); //замена

        //void Delete(T task);
    }

    public class TaskRepository : ITaskRepository<Task>
    {
        //private Context db;
        Dictionary<string, Task> db = new Dictionary<string, Task>();

        public TaskRepository(Context db)
        {
            //this.db = db;
        }

        public void CreateTask(Task task)
        {
            //db.Tasks.Add(task);
            if (db.ContainsKey(task.ID))
                throw new Exception("already in base!");

            db.Add(task.ID, task);
        }


        public Task Get(string id)
        {
            if (db.ContainsKey(id))
                return db[id];
            else throw new Exception("Staff are not exist");
        }

        public IEnumerable<Task> GetAll()
        {
            return db.Values;
        }

        public void Update(Task task)
        {
            //db.Entry(task).State = EntityState.Modified;
            db[task.ID] = task;
            
        }
    }
   
}

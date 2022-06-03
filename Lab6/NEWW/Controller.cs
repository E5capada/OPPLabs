using System;
using System.Collections.Generic;
using System.Linq;
using BLL;
using DAL;

namespace NEWW
{
    public class Controller : IController
    {
        private IService service;
        

        public Controller(IService service)
        {
            this.service = service;
        }

        //Add
        public string AddStaffMember(StaffMemberModel member) => service.AddStaffMember(new StaffMemberDTO(member.Name, member.LeaderID, member.State));

        public string AddTask(TaskModel task) => service.AddTask(new TaskDTO(task.Name, task.Description, task.ResposibleEmloyeeID));

        public string AddDayReport(DayReportModel report) => service.AddDayReport(new DayReportDTO(report.ResponsibleID, report.Text));

        public string AddSprintReport(SprintReportModel report) => service.AddSprintReport(new SprintReportDTO(report.ResponsibleID, report.Text));

        //Update
        public void ChangeLeader(string memberID, string leaderID)
        {
            if (String.IsNullOrEmpty(memberID))
                throw new Exception("Id was not input");
            if (String.IsNullOrEmpty(leaderID))
                throw new Exception("Id was not input");

            service.ChangeLeader(memberID, leaderID);
        }

        public void AddTaskComment(string memberID, string taskID, string comment)
        {
            if (String.IsNullOrEmpty(memberID))
                throw new Exception("Id was not input");
            if (String.IsNullOrEmpty(taskID))
                throw new Exception("Id was not input");
            if (String.IsNullOrEmpty(comment))
                throw new Exception("Comment was not input");

            service.AddTaskComment(memberID, taskID, comment);
        }

        public void ChangeTaskState(string memberID, string taskID, TaskState state)
        {
            if (String.IsNullOrEmpty(memberID))
                throw new Exception("Id was not input");
            if (String.IsNullOrEmpty(taskID))
                throw new Exception("Id was not input");
            
            service.ChangeTaskState(memberID, taskID, state);
        }

        public void ChangeResponsible(string taskID, string memberID)
        {
            if (String.IsNullOrEmpty(memberID))
                throw new Exception("Id was not input");
            if (String.IsNullOrEmpty(taskID))
                throw new Exception("Id was not input");
            
            service.ChangeTaskResponsible(taskID, memberID);
        }

        public void RedactDayReport(string memberID, string taskId, string newText)
        {
            if (String.IsNullOrEmpty(memberID))
                throw new Exception("Id was not input");
            if (String.IsNullOrEmpty(taskId))
                throw new Exception("Id was not input");

            service.RedactDayReport(memberID, taskId, newText);
        }

        public void AddEmployee(string leaderID, string memberID)
        {
            if (String.IsNullOrEmpty(memberID))
                throw new Exception("Id was not input");
            if (String.IsNullOrEmpty(leaderID))
                throw new Exception("Id was not input");

            service.AddEmployee(leaderID, memberID);
        }

        public void FinishTask(string memberID, string taskID)
        {
            if (String.IsNullOrEmpty(memberID))
                throw new Exception("Id was not input");
            if (String.IsNullOrEmpty(taskID))
                throw new Exception("Id was not input");

            service.ChangeTaskState(memberID, taskID, TaskState.Resolved);
        }

        //Show
        public void ShowHierarchy()
        {
            var teamLead = service.GetTeamLeader();
            ShowEmployees(teamLead.ID);

        }

        private void ShowEmployees(string id)
        {
            var Lead = service.GetStaffByID(id);
            var k = Lead.Employees.Count();
            if (k > 0) {
                Console.Write("\n" + Lead.Name + " leader of ");
                foreach (var empID in Lead.Employees)
                {
                    var emp = GetStaffByID(empID);
                    Console.Write("\t" + emp.Name + " ");
                }
                
                foreach (var empID in Lead.Employees)
                {
                    ShowEmployees(empID);
                }
               
            }
        }

        public void ShowTaskInfo(string taskID)
        {
            if (String.IsNullOrEmpty(taskID))
                throw new Exception("Id was not input");

            var task = GetTaskByID(taskID);
            Console.WriteLine("\n--------------------------------------------");
            Console.WriteLine("Name: " + task.Name);
            Console.WriteLine("Description: " + task.Description);
            Console.WriteLine("State: " + task.State.ToString());
            Console.WriteLine("Date: " + task.CreationDate);
            Console.WriteLine("Responsible: " + GetStaffByID(task.ResposibleEmloyeeID).Name);

            if (task.Comments != null)
                Console.WriteLine("Comments: " + task.Comments);
            Console.WriteLine("--------------------------------------------");
        }

        public void ShowStaffInfo(string memberID)
        {
            if (String.IsNullOrEmpty(memberID))
                throw new Exception("Id was not input");

            var staff = GetStaffByID(memberID);
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Name: "+staff.Name);
            Console.WriteLine("State: " + staff.State.ToString());
            if (staff.LeaderID != null)
                Console.WriteLine("Leader: " + GetStaffByID(staff.LeaderID).Name);
            if (staff.Employees.Count > 0)
            {
                Console.WriteLine("Employees: ");
                foreach(var emp in staff.Employees)
                {
                    Console.WriteLine("Name: " + GetStaffByID(emp).Name + " " );
                }
            }

            Console.WriteLine("\n--------------------------------------------");
        }

        public void ShowReportInfo(string reportID) {
            if (String.IsNullOrEmpty(reportID))
                throw new Exception("Id was not input");

            var report = service.GetDayReport(reportID);

            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Date: " + report.CreationDate);
            Console.WriteLine("Responsible: " + GetStaffByID(report.ResponsibleID).Name);
            Console.WriteLine("State: " + report.State.ToString());
            Console.WriteLine("\n--------------------------------------------");

            if (report.Text != null)
                Console.WriteLine("Text: " + report.Text);
                
        }

        public void ShowHistory() {
            var history = service.GetHistory();
            foreach (var mod in history)
            {
                mod.ShowInfo();
            }
        }

        //Get
        public ITaskModel GetTaskByID(string taskID)
        {
            if (String.IsNullOrEmpty(taskID))
                throw new Exception("Id was not input");
            var model = new TaskModel(service.GetTaskByID(taskID));
            return model;
        }

        public IStaffMemberModel GetStaffByID(string memberID)
        {
            if (String.IsNullOrEmpty(memberID))
                throw new Exception("Id was not input");
            return new StaffMemberModel((StaffMemberDTO)service.GetStaffByID(memberID));
        }

        public IEnumerable<ITaskModel> GetTasksByDate(DateTime date)
        {
            IEnumerable<ITaskDTO> tasks = service.GetTasksByDate(date);
            List<TaskModel> models = new List<TaskModel>();
            foreach (var task in tasks)
            {
                models.Add(new TaskModel(task));
                ShowTaskInfo(task.Id);
            }

            return models;
        }

        public ITaskModel GetTaskByLastChange()
        {
            var task = service.GetTaskByLastChange();
            ShowTaskInfo(task.Id);
            return new TaskModel(task);
        }

        public IEnumerable<ITaskModel> GetTaskByResponsible(string memberID)
        {
            if (String.IsNullOrEmpty(memberID))
                throw new Exception("Id was not input");
            IEnumerable<ITaskDTO> tasks = service.GetTaskByResponsible(memberID);
            List<TaskModel> models = new List<TaskModel>();
            foreach (var task in tasks)
            {
                models.Add(new TaskModel(task));
                ShowTaskInfo(task.Id);
            }
            return models;
        }

        public IEnumerable<ITaskModel> GetTasksThisMemberChange(string memberID)
        {
            if (String.IsNullOrEmpty(memberID))
                throw new Exception("Id was not input");
            IEnumerable<ITaskDTO> tasks = service.GetTasksThisMemberChange(memberID);
            List<TaskModel> models = new List<TaskModel>();
            foreach (var task in tasks)
            {
                models.Add(new TaskModel(task));
                ShowTaskInfo(task.Id);
            }
            return models;
        }

        // переделай!
        public IEnumerable<ITaskModel> GetTasksOfEmployees(string leaderID)
        {
            if (String.IsNullOrEmpty(leaderID))
                throw new Exception("Id was not input");

            var tasks = service.GetTasksOfEmployees(leaderID);
            List<TaskModel> models = new List<TaskModel>();
            foreach (var task in tasks)
            {
                models.Add(new TaskModel(task));
                ShowTaskInfo(task.Id);
            }
            return models;
        }

        public IEnumerable<DayReportModel> GetReportsOfEmployees(string leaderID)
        {
            if (String.IsNullOrEmpty(leaderID))
                throw new Exception("Id was not input");

            var reports = service.GetReportsOfEmployees(leaderID);
            List<DayReportModel> models =  new List<DayReportModel>();
            
            foreach (var report in reports)
            {
                models.Add(new DayReportModel((DayReportDTO)report));
                ShowReportInfo(report.ID);
            }
            
            return models;
        }

        // sprint Report
        public void WriteSprintReport(string leaderID)
        {
            var TeamLead = service.GetTeamLeader();
            if (leaderID == TeamLead.ID)
            {
                var report = service.WriteSprintReport();
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Date: " + report.CreationDate);
                Console.WriteLine("Responsible: " + TeamLead.Name);
                Console.WriteLine("State: " + report.State.ToString());

                if (report.Text != null)
                    Console.WriteLine("Comments: " + report.Text);
            }
        }
    }
}

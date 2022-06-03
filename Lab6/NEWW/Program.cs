using System;
using BLL;
using DAL;

namespace NEWW
{
    class Program
    {
        static void Main(string[] args)
        {
            var uow = new EFUnitOfWork("DefaultConnection");
            var staffService = new StaffToDoService(uow);
            var taskService = new TaskToDoService(uow);
            var reportService = new ReportToDoService(uow);
            var myService = new Service(staffService, taskService, reportService);
            var controller = new Controller(myService);

            string TeamLeader = controller.AddStaffMember(new StaffMemberModel("Vladimir", Staffstate.TeamLeader));
            string manager1 = controller.AddStaffMember(new StaffMemberModel("Irina", TeamLeader, Staffstate.Leader));
            string manager2 = controller.AddStaffMember(new StaffMemberModel("Roman", TeamLeader, Staffstate.Leader));
            string worker1 = controller.AddStaffMember(new StaffMemberModel("Alex", manager1, Staffstate.SimpleStaff));
            string worker2 = controller.AddStaffMember(new StaffMemberModel("Elena", manager1, Staffstate.SimpleStaff));
            string worker3 = controller.AddStaffMember(new StaffMemberModel("Pavel", manager2, Staffstate.SimpleStaff));

            string task1 = controller.AddTask(new TaskModel("Start project", "some description", manager1));
            string task2 = controller.AddTask(new TaskModel("Write Programm", "some description", manager2));
            string task3 = controller.AddTask(new TaskModel("Make coffee", "very important", worker2));

            //controller.ShowStaffInfo(TeamLeader);
            //controller.ShowStaffInfo(manager1);
            //controller.ShowHierarchy();

            //controller.ChangeLeader(manager1, manager2);
            //controller.ShowHierarchy();

            controller.ShowTaskInfo(task1);
            controller.AddTaskComment(manager1, task1, "new comment");
            //controller.ChangeResponsible(task1, worker3);
            controller.ChangeTaskState(manager1, task1, TaskState.Resolved);
            controller.ShowHistory();
            controller.ShowTaskInfo(task1);


            //Console.WriteLine("Task by last change");
            //controller.GetTaskByLastChange();

            //Console.WriteLine("Task by date");
            //controller.GetTasksByDate(DateTime.Now);

            //Console.WriteLine("Task by Responsible");
            //controller.GetTaskByResponsible(worker2);

            //Console.WriteLine("Task of Employees");
            //controller.GetTasksOfEmployees(TeamLeader);

            /*
            Console.WriteLine("Task the member changed");
            controller.AddTaskComment(manager1, task2, "new comment2");
            controller.GetTasksThisMemberChange(manager1);
            */

            string report1 = controller.AddDayReport(new DayReportModel("Report of manager1", manager1));
            string report2 = controller.AddDayReport(new DayReportModel("Report of manager2", manager2));
            string report3 = controller.AddDayReport(new DayReportModel("Report of worker1", worker1));
            controller.RedactDayReport(manager2, report1, "add some comment");
            controller.RedactDayReport(manager2, report1, "some info");
            controller.ShowReportInfo(report1);

            Console.WriteLine("\nReports of Employess:");
            controller.GetReportsOfEmployees(TeamLeader);

            Console.WriteLine("\nSprint Report:");
            controller.WriteSprintReport(TeamLeader);

        }
    }
}

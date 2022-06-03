using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BLL;
using DAL;

public class Service : IService
{
    private IStaffToDoService staffService;
    private ITaskToDoService taskService;
    private IReportToDoService reportService;

    public Service(IStaffToDoService staffService, ITaskToDoService taskService, IReportToDoService reportService)
    {
        this.staffService = staffService;
        this.taskService = taskService;
        this.reportService = reportService;
    }

    //ADD
    public string AddStaffMember(StaffMemberDTO member)
    {
        staffService.AddStaffMember(member);
        if (member.LeaderID != null)
        {
            AddEmployee(member.LeaderID, member.ID);
        }
        return member.ID;
    }

    public string AddTask(TaskDTO task)
    {
        taskService.AddTask(task);
        return task.Id;
    }

    public string AddDayReport(DayReportDTO report)
    {
        reportService.AddDayReport(report);
        return report.ID;
    }

    public string AddSprintReport(SprintReportDTO report)
    {
        reportService.AddDayReport(report);
        return report.ID;
    }

    //Update
    public void ChangeLeader(string memberID, string leaderID)
    {
        staffService.ChangeLeader(memberID, leaderID);

    }

    public void AddTaskComment(string memberID, string taskID, string comment)
    {
        if (GetStaffByID(memberID) != null)
        {
            var task = taskService.GetTaskByID(taskID);
            if (task.State == TaskState.Open)
            {
                taskService.ChangeState(taskID, TaskState.Active);
            }
            taskService.AddComment(memberID, taskID, comment);
        }
        else throw new Exception("Such member does not exist");
    }

    public void ChangeTaskState(string memberID, string taskID, TaskState state)
    {
        var task = taskService.GetTaskByID(taskID);
        var previousState = task.State;

        taskService.ChangeState(task.Id, state);
        
    }

    public void ChangeTaskResponsible(string taskID, string memberID)
    {
        var task = taskService.GetTaskByID(taskID);
        string previousResponsibleID = task.ResposibleEmloyeeID;
        taskService.ChangeResponsible(taskID, memberID);
    }

    public void RedactDayReport(string memberID, string reportID, string newText)
    {
        var member = staffService.GetStaffMember(memberID);
        if (member != null)
        {
            reportService.RedactDayReport(memberID, reportID, newText);
        }
    }

    public void AddEmployee(string leaderID, string memberID) => staffService.AddEmployee(leaderID, memberID);

    //Get
    public ITaskDTO GetTaskByID(string taskID) => taskService.GetTaskByID(taskID);

    public IReportDTO GetReport(string reportID) => reportService.GetDayReport(reportID);

    public IStaffMemberDTO GetStaffByID(string memberID) => staffService.GetStaffMember(memberID);

    public IEnumerable<ITaskDTO> GetTasksByDate(DateTime date)
    {
        return taskService.GetTasksByDate(date);
    }

    public ITaskDTO GetTaskByLastChange()
    {
        return taskService.GetTaskByLastChange();
    }

    public IEnumerable<ITaskDTO> GetTaskByResponsible(string memberID) => taskService.GetTaskByResponsible(memberID);

    public IEnumerable<ITaskDTO> GetTasksThisMemberChange(string memberID)
    {
        return taskService.GetTasksThisMemberChange(memberID);
    }

    public IEnumerable<ITaskDTO> GetTasksOfEmployees(string leaderID)
    {
        var emplloyees = staffService.GetEmployes(leaderID);
        return taskService.GetTasksOfEmployees(emplloyees);
    }

    public IEnumerable<IReportDTO> GetReportsOfEmployees(string leaderID)
    {
        var leader = GetStaffByID(leaderID);
        return reportService.GetDayReportsOfEmployees(leader.Employees);
    }

    private IEnumerable<IReportDTO> GetDayReportByResponsible(string memberId)
    {
        return reportService.GetDayReportRedactedBy(memberId);
    }

    public SprintReportDTO WriteSprintReport()
    {
        var draft = new DraftReportDTO();
        var members = staffService.GetAllStaff();
        foreach (var member in members)
        {
            var memberTasks = taskService.GetTaskByResponsible(member.ID);
            if (member.Employees.Count > 0)
            {
                var reportsOfEmployees = reportService.GetDayReportsOfEmployees(member.Employees);
            }

            draft.Redact(member.WriteReport());
            Thread.Sleep(20);
        }
        draft.ChangeState(TaskState.Resolved);
        return new SprintReportDTO(draft);
    }

    public IStaffMemberDTO GetTeamLeader()
    {
        var result = staffService.GetAllStaff().Where(s => s.State == Staffstate.TeamLeader);
        return result.First();
    }

    public IReportDTO GetDayReport(string reportID)
    {
        return reportService.GetDayReport(reportID);
    }

    public LinkedList<IModificationDTO> GetHistory()
    {
        return taskService.GetHistory();
    }
}

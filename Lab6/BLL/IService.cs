using System;
using System.Collections.Generic;
using DAL;

namespace BLL
{
    public interface IService
    {
        string AddStaffMember(StaffMemberDTO member);
        string AddTask(TaskDTO task);
        string AddDayReport(DayReportDTO report);
        string AddSprintReport(SprintReportDTO report);

        //Update
        void ChangeLeader(string memberID, string leaderID);
        void AddTaskComment(string memberID, string taskID, string comment);
        void ChangeTaskState(string memberID, string taskID, TaskState state);
        void ChangeTaskResponsible(string taskID, string memberID);
        void AddEmployee(string leaderID, string memberID);

        void RedactDayReport(string memberID, string reportID, string newText);

        //Get
        ITaskDTO GetTaskByID(string staffID);

        IReportDTO GetDayReport(string reportID);

        IStaffMemberDTO GetStaffByID(string meberID);

        IEnumerable<ITaskDTO> GetTasksByDate(DateTime date);

        ITaskDTO GetTaskByLastChange();

        IEnumerable<ITaskDTO> GetTaskByResponsible(string memberID);

        IEnumerable<ITaskDTO> GetTasksThisMemberChange(string memberID);

        IEnumerable<ITaskDTO> GetTasksOfEmployees(string leaderID);

        IEnumerable<IReportDTO> GetReportsOfEmployees(string leaderID);

        SprintReportDTO WriteSprintReport();

        IStaffMemberDTO GetTeamLeader();

        LinkedList<IModificationDTO> GetHistory();
    }
}

using System;
using System.Collections.Generic;
using DAL;

namespace NEWW
{
    public interface IController
    {
        //Add
        string AddStaffMember(StaffMemberModel member);
        
        string AddTask(TaskModel task);
        string AddDayReport(DayReportModel report);
        string AddSprintReport(SprintReportModel report);

        //Update
        void ChangeLeader(string memberID, string leaderID);
        void AddTaskComment(string memberID, string taskID, string comment);
        void ChangeTaskState(string memberID, string taskID, TaskState state);
        void FinishTask(string memberID, string taskID);
        void ChangeResponsible(string taskID, string memberID);

        void AddEmployee(string leaderID, string memberID);

        void RedactDayReport(string memberID, string taskId, string newText);

        //Show
        void ShowHierarchy();
        void ShowTaskInfo(string taskID);
        void ShowStaffInfo(string memberID);
        void ShowReportInfo(string ReportID);
        void ShowHistory();

        //Get
        ITaskModel GetTaskByID(string taskID);

        IStaffMemberModel GetStaffByID(string meberID);

        IEnumerable<ITaskModel> GetTasksByDate(DateTime date);

        ITaskModel GetTaskByLastChange();

        IEnumerable<ITaskModel> GetTaskByResponsible(string memberID);

        IEnumerable<ITaskModel> GetTasksThisMemberChange(string memberID);

        IEnumerable<ITaskModel> GetTasksOfEmployees(string leaderID);

        IEnumerable<DayReportModel> GetReportsOfEmployees(string leaderID);
    }
}

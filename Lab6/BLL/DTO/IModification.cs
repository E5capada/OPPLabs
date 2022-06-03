using System;
using DAL;

namespace BLL
{
    public interface IModificationDTO
    {
        public DateTime Date { get; set; }
        public string TaskID { get; set; }
        public string StaffMemberID { get; set; }

        void ShowInfo();
    }

    public abstract class ReportModificationDTO : IModificationDTO
    {
        public string TaskID { get; set; }
        public DateTime Date { get; set; }
        public string ReportId { get; set; }
        public string StaffMemberID { get; set; }

        public abstract void ShowInfo();
    }

    public class ReportCreateddDTO : ReportModificationDTO
    {
        public string NewText { get; set; }

        public ReportCreateddDTO(string reportId, string staffMemberID, string newText)
        {
            Date = DateTime.Now;
            ReportId = reportId;
            StaffMemberID = staffMemberID;
            this.NewText = newText;
        }

        public override void ShowInfo()
        {
            Console.Write("\nDate " + Date + "\tAdd new Report, " + " text:" + NewText);
        }
    }

    public class ReportRedactedDTO : ReportModificationDTO
    {
        public string NewText { get; set; }

        public ReportRedactedDTO(string reportId, string staffMemberID, string newText)
        {
            Date = DateTime.Now;
            ReportId = reportId;
            StaffMemberID = staffMemberID;
            this.NewText = newText;
        }

        public override void ShowInfo()
        {
            Console.Write("\nDate " + Date + "\tAdd new text: " + NewText);
        }
    }

    public class ReportChangedStateDTO : ReportModificationDTO
    {
        public TaskState newState { get; set; }
        public TaskState previousState { get; set; }

        public ReportChangedStateDTO(string staffMemberID, string reportId, TaskState newState, TaskState previousState)
        {
            Date = DateTime.Now;
            this.newState = newState;
            this.previousState = previousState;
            StaffMemberID = staffMemberID;
            ReportId = reportId;
        }

        public override void ShowInfo()
        {
            Console.Write("\nDate " + Date + "\tstate changed: " + previousState + " -> " + newState);
        }
    }

    public abstract class TaskModificationDTO : IModificationDTO
    {
        public string TaskID { get; set; }
        public DateTime Date { get; set; }
        public string StaffMemberID { get; set; }

        public abstract void ShowInfo();
    }

    public class TaskChangedStateDTO : TaskModificationDTO
    {
        public TaskState newState { get; set; }
        public TaskState previousState { get; set; }
        
        public TaskChangedStateDTO(string staffMemberID, string taskId, TaskState newState, TaskState previousState)
        {
            Date = DateTime.Now;
            StaffMemberID = staffMemberID;
            TaskID = taskId;
            this.newState = newState;
            this.previousState = previousState;
        }

        public override void ShowInfo()
        {
            Console.Write("\nDate " + Date + "\tstate changed: " + previousState + " -> " + newState);
        }
    }

    public class TaskNewCommentDTO : TaskModificationDTO
    {
        public string comment { get; set; }

        public TaskNewCommentDTO(string memberId, string taskID, string comment)
        {
            Date = DateTime.Now;
            this.TaskID = taskID;
            this.StaffMemberID = memberId;
            this.comment = comment;
        }

        public override void ShowInfo()
        {
            Console.Write("\nDate " + Date + "\tAdd new text: " + comment);
        }
    }

    public class TaskNewResponsibleDTO : TaskModificationDTO
    {
        public string newResponsibleID { get; set; }
        public string previousResponsibleID { get; set; }

        public TaskNewResponsibleDTO(string taskId, string newResponsibleID, string previousResponsibleID)
        {
            Date = DateTime.Now;
            TaskID = taskId;
            this.newResponsibleID = newResponsibleID;
            this.previousResponsibleID = previousResponsibleID;
        }

        public override void ShowInfo()
        {
            Console.Write("\nDate " + Date + "\tResponsible changed: " + previousResponsibleID + " -> " + newResponsibleID);
        }
    }
}
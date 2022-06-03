using System;
using DAL;

namespace BLL
{
    public interface IReportDTO 
    {
        public string ID { get; }
        public string Text { get; }
        public string ResponsibleID { get; }
        public DateTime CreationDate { get; }
        public TaskState State { get; }

        void Redact(string text);
    }

    public abstract class ReportDTO : IReportDTO
    {
        public string ID { get; protected set; }
        public string Text { get; protected set; }
        public string ResponsibleID { get; protected set; }
        public DateTime CreationDate { get; protected set; }
        public TaskState State { get; protected set; }

        public void Redact(string text)
        {
            Text += " " + text;
        }
    }

    public class DayReportDTO : ReportDTO
    {
        public DayReportDTO(string responsibleID, string text)
        {
            ID = Guid.NewGuid().ToString();
            Text = text;
            this.State = TaskState.Open;
            CreationDate = DateTime.Now;
            ResponsibleID = responsibleID;
        }

        public DayReportDTO(DayReport report)
        {
            ID = report.ID;
            Text = report.Text;
            State = report.State;
            CreationDate = report.CreationDate;
            ResponsibleID = report.ResponsibleID;
        }
    }

    public class SprintReportDTO : ReportDTO
    {
        public SprintReportDTO(string responsibleID, string text)
        {
            ID = Guid.NewGuid().ToString();
            Text = text;
            this.State = TaskState.Open;
            CreationDate = DateTime.Now;
            ResponsibleID = responsibleID;
        }

        public SprintReportDTO(DraftReportDTO report)
        {
            Text = report.Text;
            this.State = report.State;
            CreationDate = report.CreationDate;
            ResponsibleID = report.ResponsibleID;
        }
    }

    public class DraftReportDTO : ReportDTO
    {
        public DraftReportDTO()
        {
            ID = Guid.NewGuid().ToString();
            this.State = TaskState.Open;
            CreationDate = DateTime.Now;
        }

        internal void ChangeState(TaskState resolved)
        {
            State = TaskState.Resolved;
        }
    }
}

using System;
using BLL;
using DAL;

namespace NEWW
{
    public interface IReportModel
    {
        public string Text { get; set; }
        public string ResponsibleID { get; set; }
        public DateTime CreationDate { get; set; }
        public TaskState State { get; set; }
    }

    public abstract class ReportModel
    {
        public string Text { get; protected set; }
        public string ResponsibleID { get; protected set; }
        public DateTime CreationDate { get; protected set; }
        public TaskState State { get; protected set; }
    }

    public class DayReportModel : ReportModel
    {
        public DayReportModel(string text, string memberID)
        {
            Text = text;
            ResponsibleID = memberID;
        }

        public DayReportModel(DayReportDTO report)
        {
            Text = report.Text;
            this.State = report.State;
            CreationDate = report.CreationDate;
            ResponsibleID = report.ResponsibleID;
        }
    }

    public class SprintReportModel : ReportModel
    {
        public SprintReportModel(string text, string memberID)
        {
            Text = text;
            ResponsibleID = memberID;
        }

        public SprintReportModel(DraftReportDTO report)
        {
            Text = report.Text;
            this.State = report.State;
            CreationDate = report.CreationDate;
            ResponsibleID = report.ResponsibleID;
        }
    }
}

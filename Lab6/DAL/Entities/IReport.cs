using System;

namespace DAL
{
    public interface IReport
    {
        public string ID { get; }
        public string Text { get; }
        public string ResponsibleID { get; }
        public DateTime CreationDate { get; }
        public TaskState State { get; }

    }

    public abstract class Report : IReport
    {
        public string ID { get; protected set; }
        public string Text { get; protected set; }
        public string ResponsibleID { get; protected set; }
        public DateTime CreationDate { get; protected set; }
        public TaskState State { get; protected set; }
    }

    public class DayReport : Report
    {
        public DayReport(string id, string text, TaskState state, string responsibleID, DateTime date)
        {
            ID = id;
            Text = text;
            this.State = state;
            CreationDate = date;
            ResponsibleID = responsibleID;
        }
    }

    public class SprintReport : Report
    {
        public SprintReport(string id, string text, TaskState state, string responsibleID, DateTime date)
        {
            ID = id;
            Text = text;
            this.State = state;
            CreationDate = date;
            ResponsibleID = responsibleID;
        }
    }

    public class DraftReport : Report
    {
        public DraftReport(string id, string text, TaskState state, string responsibleID, DateTime date)
        {
            ID = id;
            Text = text;
            this.State = state;
            CreationDate = date;
            ResponsibleID = responsibleID;
        }

    }



}

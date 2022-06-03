using System;
using System.Collections.Generic;
using DAL;

namespace BLL
{
    public interface IReportToDoService
    {
        void AddDayReport(ReportDTO report);
        void RedactDayReport(string memberID, string reportID, string newText);
        IReportDTO GetDayReport(string reportID);
        IEnumerable<IReportDTO> GetDayReportRedactedBy(string id);
        IEnumerable<IReportDTO> GetDayReportsOfEmployees(IEnumerable<string> employees);
    }

    public class ReportToDoService : IReportToDoService
    {
        History reports;
        //IUnitOfWork Database { get; set; }
        private IReportRepository<DayReport> Database = new ReportRepository(new Context("default"));

        public ReportToDoService(IUnitOfWork database)
        {
            //Database = database;
            reports = new History();
        }

        public void AddDayReport(ReportDTO report)
        {
            Database.AddReport(new DayReport(report.ID, report.Text, report.State, report.ResponsibleID, report.CreationDate));
            reports.modifications.AddLast(new ReportCreateddDTO(report.ID, report.ResponsibleID, report.Text));
            //Database.Save();
        }


        public IReportDTO GetDayReport(string reportID)
        {
            return new DayReportDTO(Database.Get(reportID));
        }

        public IEnumerable<IReportDTO> GetDayReportRedactedBy(string id)
        {
            var reports = Database.GetAll();
            var result = new List<ReportDTO>();
            foreach (var rep in reports)
            {
                if (rep.ResponsibleID == id)
                {
                    if (!result.Exists(r => r.ID == rep.ID)){
                        result.Add(new DayReportDTO(rep));
                    }
                }
            }
            return result;
        }

        public IEnumerable<IReportDTO> GetDayReportsOfEmployees(IEnumerable<string> employees)
        {
            var result = new List<ReportDTO>();
            foreach (var emp in employees)
            {
                var reports = GetDayReportRedactedBy(emp);
                foreach (var rep in reports)
                {
                    if (!result.Exists(r => r.ID == rep.ID))
                    {
                        result.Add((DayReportDTO)rep);
                    }
                }
            }
            if (result.Count > 0)
            {
                return result;
            }
            return null;
        }

        public void RedactDayReport(string memberID, string reportID, string newText)
        {
            var report = GetDayReport(reportID);
            report.Redact(newText);
            reports.modifications.AddLast(new ReportRedactedDTO(reportID, memberID, newText));
            Database.Update(ConverToDAL(report));
        }

        public DayReport ConverToDAL(IReportDTO report)
        {
            return new DayReport(report.ID, report.Text, report.State, report.ResponsibleID, report.CreationDate);
        }
    }
}



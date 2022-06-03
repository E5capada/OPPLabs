using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DAL
{
    public interface IReportRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(string id);

        void AddReport(T report);
        void Update(T report); //замена
    }

    public class ReportRepository : IReportRepository<DayReport>
    {
        //private Context db;
        Dictionary<string, DayReport> db = new Dictionary<string, DayReport>();

        public ReportRepository(Context db)
        {
            //this.db = db;
        }

        public void AddReport(DayReport report)
        {
            //db.Reports.Add((DayReport)report);
            if (db.ContainsKey(report.ID))
                throw new Exception("already in base!");

            db.Add(report.ID, report);
        }

        public DayReport Get(string id)
        {
            //return (DayReport)db.Reports.Where(r => r.ID == id);
            return db[id];
        }

        public IEnumerable<DayReport> GetAll()
        {
            //return db.Reports;
            return db.Values;
        }

        public void Update(DayReport report)
        {
            //db.Entry(report).State = EntityState.Modified;
            db[report.ID] = report;
        }
    }

}

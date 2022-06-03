using System;
using System.Threading.Tasks;

namespace DAL
{
    public interface IUnitOfWork
    {
        IStaffRepository<StaffMember> StaffMembers { get; }
        ITaskRepository<Task> Tasks { get; }
        IReportRepository<DayReport> Reports { get; }

        void Save();
    }

    public class EFUnitOfWork : IUnitOfWork
    {
        private Context db;
        private IStaffRepository<StaffMember> _StaffRepo;
        private ITaskRepository<Task> _TaskRepo;
        private IReportRepository<DayReport> _reportRepo;

        public EFUnitOfWork(string connectionString)
        {
            db = new Context(connectionString);
        }

        public IStaffRepository<StaffMember> StaffMembers
        {
            get
            {
                if (_StaffRepo == null)
                {
                    StaffRepository staffRepository = new StaffRepository(db);
                    _StaffRepo = staffRepository;
                }

                return _StaffRepo;
            }
        }

        public ITaskRepository<Task> Tasks
        {
            get
            {
                if (_TaskRepo == null)
                    _TaskRepo = new TaskRepository(db);
                return _TaskRepo;
            }
        }

        public IReportRepository<DayReport> Reports
        {
            get
            {
                if (_reportRepo == null)
                    _reportRepo = new ReportRepository(db);
                return _reportRepo;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

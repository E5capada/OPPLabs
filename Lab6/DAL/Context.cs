using System.Data.Entity;

namespace DAL
{
    public class Context : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<StaffMember> StaffMembers { get; set; }
        public DbSet<DayReport> Reports { get; set; }

        static Context()
        {
            Database.SetInitializer<Context>(new StoreDbInitializer());
        }

        public Context(string connectionString)
            : base(connectionString)
        {

        }
    }

    public class StoreDbInitializer : DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context db)
        {
            db.SaveChanges();
        }
    }


}

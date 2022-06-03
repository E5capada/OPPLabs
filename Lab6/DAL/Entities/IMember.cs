using System.Collections.Generic;

namespace DAL
{
    public interface IMember
    {
        public string ID { get; }
        public string Name { get; }

        public string WriteReport()
        {
            return Name + " write smt in report";
        }
    }

    public interface IStaffWithLeader
    {
        public string LeaderID { get; }

        void ChangeLeader(string leaderID);
    }

    public interface IStaffWithEmployees
    {
        public List<string> Employees { get; }

        public void AddEmployee(string employeeID);

        public void RemoveEmployee(string employeeID);
    }

    public interface IStaffMember : IStaffWithLeader, IMember, IStaffWithEmployees
    {
        public Staffstate State { get; }
    }

    public class StaffMember : IStaffMember
    {
        public string ID { get; private set; }

        public string Name { get; private set; }

        public string LeaderID { get; private set; }

        public Staffstate State { get; private set; }

        public List<string> Employees { get; private set; }

        public StaffMember(string id, string name, string leaderID, Staffstate state, List<string> employees)
        {
            ID = id;
            Name = name;
            LeaderID = leaderID;
            State = state;
            Employees = employees;
        }

        public void ChangeLeader(string leaderID)
        {
            LeaderID = leaderID;
        }

        public void AddEmployee(string empID)
        {

            if (Employees == null)
                Employees = new List<string>();
            Employees.Add(empID);
        }

        public void RemoveEmployee(string empID)
        {
            Employees.Remove(empID);
        }
    }



}

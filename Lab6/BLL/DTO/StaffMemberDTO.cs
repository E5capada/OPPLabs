using System;
using System.Collections.Generic;
using DAL;

namespace BLL
{
    public interface MemberDTO
    {
        public string ID { get; }
        public string Name { get; }

        public string WriteReport()
        {
            return Name + " write smt in report";
        }
    }

    public interface IStaffWithLeaderDTO
    {
        public string LeaderID { get; }

        void ChangeLeader(string leaderID);
    }

    public interface IStaffWithEmployeesDTO
    {
        public List<string> Employees { get; }

        public void AddEmployee(string employeeID);

        public void RemoveEmployee(string employeeID);
    }

    public interface IStaffMemberDTO : IStaffWithLeaderDTO, MemberDTO, IStaffWithEmployeesDTO
    {
        public Staffstate State { get; }
    }

    public class StaffMemberDTO : IStaffMemberDTO
    {
        public string ID { get; private set; }

        public string Name { get; private set; }

        public string LeaderID { get; private set; }

        public Staffstate State { get; private set; }

        public List<string> Employees { get; private set; }

        public StaffMemberDTO(string name, string leaderID, Staffstate state)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
            LeaderID = leaderID;
            State = state;
            Employees = new List<string>();
        }

        public StaffMemberDTO(StaffMember member)
        {
            ID = member.ID;
            Name = member.Name;
            LeaderID = member.LeaderID;
            State = member.State;
            Employees = member.Employees;
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
            if (Employees.Contains(empID)){
                Employees.Remove(empID);
            } 
        }
    }
}

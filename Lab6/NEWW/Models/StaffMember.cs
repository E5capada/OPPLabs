using System;
using System.Collections.Generic;
using BLL;
using DAL;

namespace NEWW
{
    public interface IMemberModel
    {
        public string Name { get; }
    }

    public interface IStaffWithLeaderModel
    {
        public string LeaderID { get; }
    }

    public interface IStaffWithEmployeesModel
    {
        public List<string> Employees { get; }
    }

    public interface IStaffMemberModel : IStaffWithLeaderModel, IMemberModel, IStaffWithEmployeesModel
    {
        public Staffstate State { get; }
    }

    public class StaffMemberModel : IStaffMemberModel
    {
        public string Name { get; private set; }

        public string LeaderID { get; private set; }

        public Staffstate State { get; private set; }

        public List<string> Employees { get; private set; }
        

        public StaffMemberModel(string name, Staffstate state)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Name was not inputed");

            Name = name;
            State = state;
        }

        public StaffMemberModel(string name, string leaderID, Staffstate state)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Name was not inputed");
            if (string.IsNullOrEmpty(leaderID))
            {
                throw new Exception("Need Leader");
            }

            Name = name;
            LeaderID = leaderID;
            State = state;
        }

        public StaffMemberModel(StaffMemberDTO member)
        {
            Name = member.Name;
            LeaderID = member.LeaderID;
            Employees = member.Employees;
            State = member.State;
        }
    }

}

using System;
using System.Collections.Generic;
using DAL;

namespace BLL
{
    public interface IStaffToDoService
    {
        //ADD
        void AddStaffMember(StaffMemberDTO member);

        //Get
        IEnumerable<IStaffMemberDTO> GetAllStaff();
        IStaffMemberDTO GetStaffMember(string memberID);
        IEnumerable<IStaffMemberDTO> GetEmployes(string memberID);

        //UPDATE
        void ChangeLeader(string memberID, string leaderID);
        void AddEmployee(string leaderID, string employeeID);
    }


    public class StaffToDoService : IStaffToDoService
    {
        //IUnitOfWork Database { get; set; }
        private IStaffRepository<StaffMember> Database = new StaffRepository(new Context("default"));

        public StaffToDoService(IUnitOfWork uow)
        {
            //Database = uow;
        }

        //ADD
        public void AddStaffMember(StaffMemberDTO member)
        {
            var memberNew = ConvertToDal(member);
            Database.AddStaffMember(memberNew);
            //Database.Save();
        }

        public StaffMember ConvertToDal(StaffMemberDTO member)
        {
            return new StaffMember(member.ID, member.Name, member.LeaderID, member.State, member.Employees);
        }

        //GET
        public IEnumerable<IStaffMemberDTO> GetAllStaff()
        {
            var staffsDTO = new List<IStaffMemberDTO>();
            var staffs = Database.GetAll();

            foreach (var staff in staffs)
            {
                staffsDTO.Add(new StaffMemberDTO(staff));
            }
            return staffsDTO;
        }

        public IStaffMemberDTO GetStaffMember(string memberID)
        {
            var staff = Database.Get(memberID);
            return new StaffMemberDTO(staff);
        }

        public IEnumerable<IStaffMemberDTO> GetEmployes(string leaderID)
        {
            var leader = GetStaffMember(leaderID);

            if (leader.Employees.Count > 0)
            {
                var employes = new List<IStaffMemberDTO>();
                foreach (var empID in leader.Employees)
                {
                    var emp = Database.Get(empID);
                    employes.Add(new StaffMemberDTO(emp));
                }
                return employes;
            }
            else throw new Exception("This member don not have employees");
        }

        //UPDATE
        public void ChangeLeader(string memberID, string leaderID)
        {
            StaffMemberDTO leader = (StaffMemberDTO)GetStaffMember(leaderID);
            StaffMemberDTO member = (StaffMemberDTO)GetStaffMember(memberID);
            StaffMemberDTO previousLeader = (StaffMemberDTO)GetStaffMember(member.LeaderID);

            if (member.State == Staffstate.SimpleStaff || member.State == Staffstate.Leader)
            {
                if (leader.State == Staffstate.Leader || leader.State == Staffstate.TeamLeader)
                { 
                    member.ChangeLeader(leader.ID);
                    Database.Update(ConvertToDal(member));

                    leader.AddEmployee(member.ID);
                    Database.Update(ConvertToDal(leader));

                    previousLeader.RemoveEmployee(member.ID);
                    Database.Update(ConvertToDal(previousLeader));
                    //Database.Save();
                }
                else throw new Exception(leader.Name + " can not have employess");
            }
            else throw new Exception(member.Name + " can not have leader");
        }

        public void AddEmployee(string leaderID, string employeeID)
        {
            var leader = (StaffMemberDTO)GetStaffMember(leaderID);
            var member = (StaffMemberDTO)GetStaffMember(employeeID);

            if (member.State == Staffstate.SimpleStaff || member.State == Staffstate.Leader)
            {
                if (leader.State == Staffstate.Leader || leader.State == Staffstate.TeamLeader)
                {
                    if (member.LeaderID != null)
                    {
                        StaffMemberDTO previousLeader = (StaffMemberDTO)GetStaffMember(member.LeaderID);
                        previousLeader.RemoveEmployee(member.ID);
                        Database.Update(ConvertToDal(previousLeader));
                    }
                        
                    if (member.LeaderID != leader.ID)
                        member.ChangeLeader(leader.ID);

                    leader.AddEmployee(member.ID);
                    Database.Update(ConvertToDal(member));
                    Database.Update(ConvertToDal(leader));
                    //Database.Save();
                }
                else throw new Exception(leader.Name + " can not have employess");
            }
            else throw new Exception(member.Name + " can not have leader");
        }
    }
}

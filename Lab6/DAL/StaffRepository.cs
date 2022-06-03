using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DAL
{
    public interface IStaffRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(string id);

        void AddStaffMember(T member);
        void Update(T memberID); //замена
    }

    public class StaffRepository : IStaffRepository<StaffMember>
    {
        //private Context db;
        Dictionary<string, StaffMember> db = new Dictionary<string, StaffMember>(); 

        public StaffRepository(Context db)
        {
            //this.db = db;
        }

        public void AddStaffMember(StaffMember member)
        {
            //db.StaffMembers.Add(member);
            if (db.ContainsKey(member.ID))
                throw new Exception("already in base!");

            db.Add(member.ID, member);
        }

        
        public StaffMember Get(string id)
        {
            //return db.StaffMembers.Find(id);
            if (db.ContainsKey(id))
                return db[id];
            else throw new Exception("Staff are not exist");
        }

        public IEnumerable<StaffMember> GetAll()
        {
            //return db.StaffMembers;
            return db.Values;
        }

        public IEnumerable<StaffMember> GetEmployes(string memberID)
        {
            /*var member = Get(memberID);
            if (member is IStaffWithEmployees)
                return db.StaffMembers.Where(o => o.LeaderID == member.ID).ToList();
            else throw new Exception("Employees not found");*/

            return db.Values.Where(s => s.LeaderID == memberID);
        }

        public void Update(StaffMember member)
        {
            //db.Entry(member).State = EntityState.Modified;
            db[member.ID] = member;
        }

    }

}

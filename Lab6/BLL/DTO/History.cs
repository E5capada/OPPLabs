using System.Collections.Generic;

namespace BLL
{
    public interface IHistory
    {
        void AddModification(ReportModificationDTO mod);
        void ShowModifications();
    }

    public class History : IHistory
    {
        public LinkedList<IModificationDTO> modifications { get; set; }

        public History()
        {
            this.modifications = new LinkedList<IModificationDTO>();
        }

        public void AddModification(ReportModificationDTO mod)
        {
            modifications.AddLast(mod);
        }

        public void ShowModifications()
        {
            foreach (var mod in modifications)
            {
                mod.ShowInfo();
            }
        }

       
    }
}
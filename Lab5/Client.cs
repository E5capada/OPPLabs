using System;
namespace OPPLab5_2
{
    public interface IClient
    {
        void SetNameAndSurname(string name, string surname);
        void SetAdress(string adress);
        void SetPasport(string passport);
    }

    public class Client : IClient
    {
        private string Name;
        private string Surname;
        private string Adress = "";
        private string Pasport = "";
        public bool IsReliable { get; private set; }
        public string ClientID { get; private set; }

        public void SetNameAndSurname(string name, string surname)
        {
            if (name != "")
            {
                Name = name;
            }
            else throw new FormatException("NAME IS EMPTY");

            if (surname != "")
            {
                Surname = surname;
            }
            else throw new FormatException("ADRESS IS EMPTY");

            ClientID = Guid.NewGuid().ToString();
        }

        private void checkReability()
        {
            if (Pasport != "" && Adress != "")
            {
                IsReliable = true;
            }
        } 

        public void SetAdress(string adress)
        {
            Adress = adress;
            checkReability();
        }

        public void SetPasport(string passport)
        {
            Pasport = passport;
            checkReability();
        }

        public void ShowClienttInfo()
        {
            Console.WriteLine("-----------------------------CientInfo-------------------------------");
            Console.WriteLine("Name: " + Name + "\tSurname: " + Surname);
            
            if (Adress != "")
            {
                Console.WriteLine("Adress: " + Adress);
            }
            if (Pasport != "")
            {
                Console.WriteLine("Pasport: " + Pasport);
            }
            Console.WriteLine("IsReliable: " + IsReliable);
        }
    }
}

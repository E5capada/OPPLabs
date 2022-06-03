using System;
namespace OPPLab5_2
{
    public interface IClientBuilder
    {
        void SetNameAndSurname(string name, string surname);
        void SetPassport(string passport);
        void SetAddress(string adress);
        Client GetClient();
    }

    class ClientBuilder : IClientBuilder
    {
        private Client _client = new Client();

        public void Reset()
        {
            this._client = new Client();
        }

        void IClientBuilder.SetAddress(string adress)
        {
            this._client.SetAdress(adress);
        }

        void IClientBuilder.SetNameAndSurname(string name, string surname)
        {
            this._client.SetNameAndSurname(name, surname);
        }

        void IClientBuilder.SetPassport(string passport)
        {
            this._client.SetPasport(passport);
        }

        public Client GetClient()
        {
            Client result = this._client;
            this.Reset();
            return result;
        }

    }

    public class Director
    {
        private IClientBuilder _builder;
        private static Director _instance;

        private Director() { }

        public static Director GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Director();
            }
            return _instance;
        }


        public IClientBuilder Builder
        {
            set { _builder = value; }
        }

        public void buildMinimalClient(string name, string surname)
        {
            this._builder.SetNameAndSurname(name, surname);
            
        }

        public void buildFullFeaturedClient(string name, string surname, string passport, string adress)
        {
            this._builder.SetNameAndSurname(name, surname);

            if (passport != "")
            {
                this._builder.SetPassport(passport);
            }
            if (adress != "")
            {
                this._builder.SetAddress(adress);
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace OPPLab5_2
{
    interface IBank
    {
        void AddMoney(string accountID, double sum);
        void Withdraw(string accountID, double sum, bool DenyOperation);
        void InterestAccural(DateTime date, string accountID);
    }

    //Receiver - получатель
    class Bank : IBank
    {
        public string Name { get; protected set; }
        public string BankID { get; protected set; }

        public double Procent { get; protected set; }
        public double commision { get; protected set; }
        public double WithdrawLimit { get; protected set; }

        IClientBuilder builder = new ClientBuilder();
        Director director = Director.GetInstance();

        IAccountBuilder depositBuilder = new DepositAccountBuilder();
        IAccountBuilder debitBuilder = new DebitAccountBuilder();
        IAccountBuilder creditBuilder = new CreditAccountBuilder();
        AccountsDirector AccountsDirector = AccountsDirector.GetInstance();

        Dictionary<string, Client> clients = new Dictionary<string, Client>();
        Dictionary<string, KeyValuePair <BankAccount, AccountHistory>> accounts = new Dictionary<string, KeyValuePair<BankAccount, AccountHistory>>();

        public Bank(string name, double procent, double commision, double withdrawLimit)
        {
            if (name != "")
            {
                Name = name;
                BankID = Guid.NewGuid().ToString();
            }
            else throw new Exception("NAME IS EMPTY");

            if (0 < procent)
            {
                Procent = procent / 365;
            }
            
            else throw new Exception("LIMIUT UNDER ZERO");

            if (commision > 0)
            {
                this.commision = commision;
            }
            else throw new Exception("COMMISSION BELOW ZERO");

            if (withdrawLimit > 0)
            {
                this.WithdrawLimit = withdrawLimit;
            }
            else throw new Exception("WITHDRAW LIMIT BELOW ZERO");
        }

        public string buildMinimalClient(string name, string surname)
        {
            director.Builder = builder;
            director.buildMinimalClient(name, surname);
            Client client = builder.GetClient();
            try
            {
                clients.Add(client.ClientID, client);
            }
            catch
            {
                throw new Exception("Client already exist");
            }
            return client.ClientID;
        }

        public string buildFullFeaturedClient(string name, string surname, string adress, string passport)
        {
            director.Builder = builder;
            director.buildFullFeaturedClient(name, surname, adress, passport);
            Client client = builder.GetClient();
            try
            {
                clients.Add(client.ClientID, client);
            }
            catch
            {
                throw new Exception("Client already exist");
            }
            return client.ClientID;
        }

        public string CreateDebitAccount(string ownerID, DateTime date, double balance)
        {
            if (FindClient(ownerID) != null)
            {
                AccountsDirector.Builder = debitBuilder;
                var account = AccountsDirector.CreateDebitAccount(ownerID, date, balance, Procent);
                try
                {
                    accounts.Add(account.AccountID, new KeyValuePair<BankAccount, AccountHistory>(account, new AccountHistory()));
                }
                catch
                {
                    throw new Exception("Account already exist");
                }
                return account.AccountID;
            }
            else throw new Exception("NO SUCH CLIENT IN BANK");
        }

        public string CreateDepositAccount(string ownerID, DateTime date, double balance, DateTime term)
        {
            if (FindClient(ownerID) != null)
            {
                AccountsDirector.Builder = depositBuilder;
                var account = AccountsDirector.CreateDepositAccount(ownerID, date, balance, term);
                try
                {
                    accounts.Add(account.AccountID, new KeyValuePair<BankAccount, AccountHistory>(account, new AccountHistory()));
                }
                catch
                {
                    throw new Exception("Account already exist");
                }
                return account.AccountID;
            }
            else throw new Exception("NO SUCH CLIENT IN BANK");
        }

        public string CreateCreditAccount(string ownerID, DateTime date, double balance, double limit)
        {
            if (FindClient(ownerID) != null)
            {
                AccountsDirector.Builder = creditBuilder;
                var account = AccountsDirector.CreateCreditAccount(ownerID, date, balance, limit);
                try
                {
                    accounts.Add(account.AccountID, new KeyValuePair<BankAccount, AccountHistory>(account, new AccountHistory()));
                }
                catch
                {
                    throw new Exception("Account already exist");
                }
                return account.AccountID;
            }
            else throw new Exception("NO SUCH CLIENT IN BANK");
        }

        public AccountHistory FindBankHistory(string accountID)
        {
            if (accounts.ContainsKey(accountID))
            {
                return accounts[accountID].Value;
            }
            else throw new Exception("SUCH ACCOUNT DOESN'T EXIST");
        }

        public Client FindClient(string clientID)
        {
            if (clients.ContainsKey(clientID))
            {
                return clients[clientID];
            }
            else throw new Exception("NO SUCH CLIENT IN THIS BANK");
        }

        public BankAccount FindAccount(string accountID)
        {
            if (accounts.ContainsKey(accountID))
            {
                return accounts[accountID].Key;
            }
            else return null;
        }

        public void AddMoney(string accountID, double sum)
        {
            if (accounts.ContainsKey(accountID))
            {
                accounts[accountID].Key.AddMoney(sum);
            }
        }

        public void InterestAccural(DateTime date, string accountID)
        {
            if (accounts.ContainsKey(accountID))
            {
                var account = accounts[accountID].Key;
                if (date.Day == account.CreationDate.Day && date.Date != account.CreationDate.Date )
                {
                    if (account is AccountWithInserts)
                    {
                        account.AddMoney(((AccountWithInserts)account).Accural);
                        ((AccountWithInserts)account).setAccural(0);
                    }
                    else if (account is CreditAccount)
                    {
                        if (account.Balance < 0)
                        {
                            account.WithdrawMoney(commision, true);
                        }
                    }
                }

                if (account is AccountWithInserts)
                {
                    ((AccountWithInserts)account).InsertAccural();
                }
            }
        }

        public void Withdraw(string accountID, double sum, bool DenyOperation)
        {
            if (accounts.ContainsKey(accountID))
            {
                var account = accounts[accountID].Key;
                if (clients[account.OwnerID].IsReliable)
                {
                    if (DenyOperation)
                    {
                        account.WithdrawMoney(sum, true);
                    }
                    else account.WithdrawMoney(sum, false);
                }
                else if (sum < WithdrawLimit)
                {
                    account.WithdrawMoney(sum, false);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace OPPLab5_2
{
    // Invoker - инициатор
    class BankManager
    {
        Dictionary<string, Bank> banks = new Dictionary<string, Bank>();

        CashDispenser cashDispenser = new CashDispenser();
        Dictionary<string, Stack<Transaction>> Transactions = new Dictionary<string, Stack<Transaction>>();

        private BankManager()
        { }

        private static BankManager _instance;

        public static BankManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new BankManager();
            }
            return _instance;
        }

        public string buildMinimalClient(string bankID, string name, string surname)
        {
            return banks[bankID].buildMinimalClient(name, surname);
        }

        public string buildFullFeaturedClient(string bankID, string name, string surname, string adress, string passport)
        {
            return banks[bankID].buildFullFeaturedClient(name, surname, adress, passport);
        }

        public string CreateBank(string name, double procent, double commision, double withdrawLimit)
        {
            Bank bank = new Bank(name, procent, commision, withdrawLimit);
            banks.Add(bank.BankID, bank);
            return bank.BankID;
        }

        public string CreateDebitAccount(string bankId, string ownerID, DateTime date, double balance)
        {
            string accountID = banks[bankId].CreateDebitAccount(ownerID, date, balance);
            try
            {
                Transactions.Add(accountID, new Stack<Transaction>());
            }
            catch
            {
                throw new Exception("Account Exist");
            }
            return accountID;
        }

        public string CreateDepositAccount(string bankId, string ownerID, DateTime date, double balance, DateTime term)
        {
            string accountID = banks[bankId].CreateDepositAccount(ownerID, date, balance, term);
            try
            {
                Transactions.Add(accountID, new Stack<Transaction>());
            }
            catch
            {
                throw new Exception("Account Exist");
            }
            return accountID;
            
        }

        public string CreateCreditAccount(string bankId, string ownerID, DateTime date, double balance, double limit)
        {
            string accountID = banks[bankId].CreateCreditAccount(ownerID, date, balance, limit);
            try
            {
                Transactions.Add(accountID, new Stack<Transaction>());
            }
            catch
            {
                throw new Exception("Account Exist");
            }
            return accountID;
        }

        public void AddClientInfo(string bankID, string clientID, string passport, string adress)
        {
            var client = banks[bankID].FindClient(clientID);
            if (client != null) 
            {
                client.SetAdress(adress);
                client.SetPasport(passport);
            }
            else throw new Exception("NO SUCH CLIENT IN BANK");
        }

        public void ShowClientInfo(string bankID, string clientID)
        {
            if (banks[bankID].FindClient(clientID) != null)
            {
                banks[bankID].FindClient(clientID).ShowClienttInfo();
            }
            else throw new Exception("NO SUCH CLIENT IN BANK");
        }

        private Bank FindAccountBank(string accountID)
        {
            foreach (var bank in banks)
            {
                if (bank.Value.FindAccount(accountID) != null)
                {
                    return bank.Value;
                }
            }
            throw new Exception("SUCH ACCOUN DOES NOT EXIST");
        }

        public void AddMoney(string accointID, DateTime date, double sum)
        {
            AddMoneyCommand addMoney = new AddMoneyCommand(FindAccountBank(accointID), sum, accointID);
            cashDispenser.SetCommand(addMoney);
            cashDispenser.ExucuteCommand();
            var bank = FindAccountBank(accointID);
            Transactions[accointID].Push(new Transaction(date, addMoney, accointID, accointID, addMoney.Type));
        }

        public void WithdrawMoney(string accointID, DateTime date, double sum)
        {
            WithdrawMomeyCommand withdrawMomey = new WithdrawMomeyCommand(FindAccountBank(accointID), sum, accointID);
            cashDispenser.SetCommand(withdrawMomey);
            cashDispenser.ExucuteCommand();
            Transactions[accointID].Push(new Transaction(date, withdrawMomey, accointID, accointID, withdrawMomey.Type));
        }

        public void Transfer(string senderID, string recipientID, DateTime date, double sum)
        {
            TransferCommand transfer = new TransferCommand(FindAccountBank(senderID), FindAccountBank(recipientID), senderID, recipientID, sum);
            cashDispenser.SetCommand(transfer);
            cashDispenser.ExucuteCommand();
            
            Transactions[senderID].Push(new Transaction(date, transfer, recipientID, senderID, transfer.Type));
            Transactions[recipientID].Push(new Transaction(date, transfer, senderID, recipientID, transfer.Type));
        }

        public void DenyOperation(string accountID)
        {
            var history = Transactions[accountID];
            if (history != null)
            {
                Transaction deniedCommand = history.Pop();
                deniedCommand.Command.Undo();
                if (deniedCommand.Sender != deniedCommand.Recipient)
                {
                    Transactions[deniedCommand.Recipient].Pop();
                }
            }
        }

        public void TimeMashine(DateTime Userdate, string accountID)
        {
            var bank = FindAccountBank(accountID);
            Transaction[] accountHistory = Transactions[accountID].ToArray();
            
            var account = bank.FindAccount(accountID);
            double currBalance = account.Balance;
            DateTime currDate = Userdate;
            while(currDate > account.CreationDate)
            {
                bank.InterestAccural(currDate, accountID);
                foreach (var transaction in accountHistory)
                {
                    if (transaction.time.Date == currDate.Date)
                    {
                        if (transaction.Command is AddMoneyCommand || (transaction.Recipient == accountID && transaction.Command is TransferCommand))
                        {
                            currBalance -= transaction.Command.Sum;
                        }
                        else if (transaction.Command is WithdrawMomeyCommand || (transaction.Sender == accountID && transaction.Command is TransferCommand))
                        {
                            currBalance += transaction.Command.Sum;
                        }
                    }
                }
                currDate = currDate.AddDays(-1);
            }
            Console.WriteLine("In " + Userdate + " balance will be: " + account.Balance + " accural: " + account.Accural);
            //account.ShowAccountBasicInfo();
        }

        public void ShowInfo(string accountID)
        {
            var bank = FindAccountBank(accountID);
            var account = bank.FindAccount(accountID);
            
            bank.FindClient(account.OwnerID).ShowClienttInfo();
            account.ShowAccountBasicInfo();

            if (Transactions.ContainsKey(accountID) && Transactions[accountID].Count != 0)
            {
                Console.WriteLine("Transaction histoty: ");
                Transaction[] history = Transactions[accountID].ToArray();
                foreach (var transaction in history)
                {
                    transaction.ShowTransactionInfo();
                }
            }
        }
    }
}

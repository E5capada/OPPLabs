using System;
using System.Collections.Generic;

namespace OPPLab5_2
{
    enum BankAcountType
    {
        Debit,
        Deposit,
        Credit
    }

    interface IBankAccount
    {
        void AddMoney(double sum);
        void WithdrawMoney(double sum, bool ignoreConditions);
    }

    //Originator
    abstract class BankAccount : IBankAccount
    {
        public string AccountID { get; protected set; }
        public string OwnerID { get; protected set; }
        public DateTime CreationDate { get; protected set; }
        public double Balance { get; protected set; }
        public double Accural { get; protected set; }
        public BankAcountType Type { get; protected set; }

        protected abstract void SetType();

        public void setBasicAccountInfo(string ownerID, DateTime date, double balance)
        {
            OwnerID = ownerID;
            CreationDate = date;
            if (balance >= 0)
            {
                Balance = balance;
            }
            else throw new Exception("BALANCE BELOW ZERO");
            
            AccountID = Guid.NewGuid().ToString();
            SetType();
        }

        public void AddMoney(double sum)
        {
            if (sum >= 0)
            {
                Balance += sum;
            }
            else throw new Exception("NEGATIVE ENTRIES");
        }

        public void ShowAccountBasicInfo()
        {
            Console.WriteLine("----------------------------AccountInfo-------------------------------");
            Console.WriteLine("AccountId: {0}", AccountID);
            Console.WriteLine("CreationDate: {0}", CreationDate);
            Console.WriteLine("Type: {0}", Type.ToString());
            Console.WriteLine("Balance: {0}", Balance);
            ShowAccountInfo();
        }

        protected abstract bool CanWithdrawMoney(double sum);

        public void WithdrawMoney(double sum, bool ignoreCondition)
        {
            if (CanWithdrawMoney(sum) || ignoreCondition)
            {
                Balance -= sum;
            }
            else throw new Exception("CAN'T WITHDRAW MONEY");
        }
        
        protected abstract void ShowAccountInfo();
        
        public AccountMemento SaveState()
        {
            return new AccountMemento(Balance, Accural);
        }

        public void RestoreAccountState(AccountMemento accountMemento)
        {
            this.Balance = accountMemento.Balance;
            this.Accural = accountMemento.Accural;
        }
    }

    //Memento
    class AccountMemento
    {
        public double Balance { get; private set; }
        public double Accural { get; private set; }

        public AccountMemento(double balance, double accural)
        {
            this.Balance = balance;
            this.Accural = accural;
        }
    }

    //CareTaker
    class AccountHistory
    {
        public Stack<AccountMemento> History { get; private set; }
        
        public AccountHistory()
        {
            History = new Stack<AccountMemento>();
        }
    }
}

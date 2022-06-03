using System;
namespace OPPLab5_2
{
    interface IAccountBuilder
    {
        void SetBasicAccountInfo(string ownerID, DateTime date, double balance);
        void SetProcent();
        void SetProcent(double procent);
        void SetLimit(double Limit);
        void SetTerm(DateTime date);
        BankAccount GetBankAccount();
    }

    class DebitAccountBuilder : IAccountBuilder
    {
        DebitAccount _debitAccount = new DebitAccount();

        public BankAccount GetBankAccount()
        {
            DebitAccount result = this._debitAccount;
            this.Reset();
            return result;
        }

        public void SetBasicAccountInfo(string ownerID, DateTime date, double balance)
        {
            this._debitAccount.setBasicAccountInfo(ownerID, date, balance);
        }

        public void SetProcent(double procent)
        {
            this._debitAccount.SetProcent(procent);
        }

        public void Reset()
        {
            this._debitAccount = new DebitAccount();
        }

        public void SetProcent()
        {
            throw new NotImplementedException();
        }

        public void SetLimit(double Limit)
        {
            throw new NotImplementedException();
        }

        public void SetTerm(DateTime date)
        {
            throw new NotImplementedException();
        }
    }

    class DepositAccountBuilder : IAccountBuilder
    {
        DepositAccount _depositAccount = new DepositAccount();

        public BankAccount GetBankAccount()
        {
            DepositAccount result = this._depositAccount;
            this.Reset();
            return result;
        }

        public void SetBasicAccountInfo(string ownerID, DateTime date, double balance)
        {
            this._depositAccount.setBasicAccountInfo(ownerID, date, balance);
        }

        public void SetProcent()
        {
            this._depositAccount.SetProcent();
        }

        public void Reset()
        {
            this._depositAccount = new DepositAccount();
        }

        public void SetProcent(double procent)
        {
            throw new NotImplementedException();
        }

        public void SetLimit(double Limit)
        {
            throw new NotImplementedException();
        }

        public void SetTerm(DateTime date)
        {
            this._depositAccount.SetTerm(date);
        }
    }

    class CreditAccountBuilder : IAccountBuilder
    {
        CreditAccount _creditAccount = new CreditAccount();

        public BankAccount GetBankAccount()
        {
            CreditAccount result = this._creditAccount;
            this.Reset();
            return result;
        }

        public void SetBasicAccountInfo(string ownerID, DateTime date, double balance)
        {
            this._creditAccount.setBasicAccountInfo(ownerID, date, balance);
        }

        public void Reset()
        {
            this._creditAccount = new CreditAccount();
        }

        public void SetProcent(double procent)
        {
            throw new NotImplementedException();
        }

        public void SetLimit(double limit)
        {
           this._creditAccount.setLimitAndCommission(limit);
        }

        public void SetProcent()
        {
            throw new NotImplementedException();
        }

        public void SetTerm(DateTime date)
        {
            throw new NotImplementedException();
        }
    }

    class AccountsDirector
    {
        private IAccountBuilder _builder;
        private static AccountsDirector _instance;

        public IAccountBuilder Builder
        {
            set { _builder = value; }
        }

        private AccountsDirector() { }

        public static AccountsDirector GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AccountsDirector();
            }
            return _instance;
        }

        public DebitAccount CreateDebitAccount(string ownerID, DateTime date, double balance, double procent)
        {
            this._builder.SetBasicAccountInfo(ownerID, date, balance);
            this._builder.SetProcent(procent);
            return (DebitAccount)_builder.GetBankAccount();
        }

        public DepositAccount CreateDepositAccount(string ownerID, DateTime date, double balance, DateTime term)
        {
            this._builder.SetBasicAccountInfo(ownerID, date, balance);
            this._builder.SetProcent();
            this._builder.SetTerm(term);
            return (DepositAccount)_builder.GetBankAccount();
        }

        public CreditAccount CreateCreditAccount(string ownerID, DateTime date, double balance, double limit)
        {
            this._builder.SetBasicAccountInfo(ownerID, date, balance);
            this._builder.SetLimit(limit);
            return (CreditAccount)_builder.GetBankAccount();
        }
    }
}
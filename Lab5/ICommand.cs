using System;
using System.Collections.Generic;

namespace OPPLab5_2
{
    enum CommandType
    {
        Adding,
        Withdrawing,
        Trasfer
    }

    abstract class Command
    {
        protected Bank bank;
        public double Sum { get; protected set; }
        public string AccountID { get; protected set; }
        public CommandType Type { get; protected set; }

        public abstract void Execute();

        public abstract void Undo();
    }

    class AddMoneyCommand : Command
    {
        public AddMoneyCommand(Bank bankSet, double sum, string accountID)
        {
            bank = bankSet;
            Sum = sum;
            AccountID = accountID;
            Type = CommandType.Adding;
        }

        public override void Execute()
        {
            bank.AddMoney(AccountID, Sum);
            var account = bank.FindAccount(AccountID);
            bank.FindBankHistory(AccountID).History.Push(account.SaveState());
        }

        public override void Undo()
        {
            bank.Withdraw(AccountID, Sum, true);
            bank.FindBankHistory(AccountID).History.Pop();
        }
    }

    class WithdrawMomeyCommand : Command
    {
        
        public WithdrawMomeyCommand(Bank bankSet, double sum, string accountID)
        {
            bank = bankSet;
            Sum = sum;
            AccountID = accountID;
            Type = CommandType.Withdrawing;
        }

        public override void Execute()
        {
            bank.Withdraw(AccountID, Sum, false);
            var account = bank.FindAccount(AccountID);
            bank.FindBankHistory(AccountID).History.Push(account.SaveState());
        }

        public override void Undo()
        {
            bank.AddMoney(AccountID, Sum);
            bank.FindBankHistory(AccountID).History.Pop();
        }
    }

    class TransferCommand : Command
    {
        private Bank bank_2;
        public string AccountID_2 { get; }
        public CommandType Type { get; }

        public TransferCommand(Bank bank1, Bank bank2, string AccountID1, string AccountID2, double sum)
        {
            bank = bank1;
            bank_2 = bank2;
            AccountID = AccountID1;
            AccountID_2 = AccountID2;
            Sum = sum;
            Type = CommandType.Trasfer;
        }

        public override void Execute()
        {
            bank.Withdraw(AccountID, Sum, false);
            var account1 = bank.FindAccount(AccountID);
            bank.FindBankHistory(AccountID).History.Push(account1.SaveState());

            bank_2.AddMoney(AccountID_2, Sum);
            var account2 = bank_2.FindAccount(AccountID_2);
            bank_2.FindBankHistory(AccountID_2).History.Push(account2.SaveState());
        }

        public override void Undo()
        {
            bank.AddMoney(AccountID, Sum);
            bank.FindBankHistory(AccountID).History.Pop();

            bank_2.Withdraw(AccountID_2, Sum, true);
            bank_2.FindBankHistory(AccountID_2).History.Pop();
        }
    }

    // Invoker - инициатор}
    class CashDispenser
    {
        Command command;

        public void SetCommand(Command com)
        {
            command = com;
        }

        public void ExucuteCommand()
        {
            command.Execute();
        }
        public void UndoCommand()
        {
            command.Undo();
        }
    }
}

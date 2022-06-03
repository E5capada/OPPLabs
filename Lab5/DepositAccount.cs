using System;
namespace OPPLab5_2
{
    class DepositAccount : AccountWithInserts
    {
        private DateTime Term { get; set; }

        public void SetTerm(DateTime date)
        {
            Term = date;
        }
        
        public void SetProcent()
        {
            if (Balance >= 0)
            {
                Balance = Balance;
            }
            else throw new Exception("BALANCE BELOW ZERO");

            if (0 <= Balance && Balance < 5000)
            {
                Procent = 0.03;
            }
            else if (5000 <= Balance && Balance < 100000)
            {
                Procent = 0.035;
            }
            else if (Balance >= 10000)
            {
                Procent = 0.04;
            }
        }

        protected override bool CanWithdrawMoney(double sum)
        {
            if (Balance >= sum && sum > 0 && DateTime.Now > Term)
            {
                return true;
            }
            else return false;
        }

        protected override void ShowAccountInfo()
        {
            Console.WriteLine("Term: {0}", Term);
            
        }

        protected override void SetType()
        {
            Type = BankAcountType.Deposit;
        }
    }
}
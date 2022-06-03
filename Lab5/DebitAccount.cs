using System;
namespace OPPLab5_2
{
    class DebitAccount : AccountWithInserts
    {
        public void SetProcent(double procent)
        {
            Procent = procent;
        }
        protected override void ShowAccountInfo()
        {
            Console.WriteLine("Accural: {0}", Accural);
            Console.WriteLine("Procent: {0}", Procent);
        }

        protected override bool CanWithdrawMoney(double sum)
        {
            if (Balance >= sum && sum > 0)
            {
                return true;
            }
            else return false;
        }

        protected override void SetType()
        {
            Type = BankAcountType.Debit;
        }
    }
}

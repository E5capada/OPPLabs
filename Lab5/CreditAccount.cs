using System;
namespace OPPLab5_2
{
    class CreditAccount : BankAccount
    {
        private double Limit;

        public void setLimitAndCommission(double limit)
        {
            Limit = limit;
        }

        protected override void ShowAccountInfo()
        {
            Console.WriteLine("Limit: {0}", Limit);
        }
    
        protected override bool CanWithdrawMoney(double sum)
        {
            if (sum > 0 && (Balance - sum) >= Limit && Balance > Limit)
            {
                return true;
            }
            else return false;
        }

        protected override void SetType()
        {
            Type = BankAcountType.Credit;
        }
    }
}
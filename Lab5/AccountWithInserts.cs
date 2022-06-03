namespace OPPLab5_2
{
    abstract class AccountWithInserts : BankAccount
    {
        public double Procent { get; protected set; }

        public void InsertAccural()
        {
            Accural += Procent * Balance;
        }

        public void setAccural(double accural)
        {
            Accural = accural;
        }
    }
}

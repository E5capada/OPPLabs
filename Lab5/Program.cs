using System;

namespace OPPLab5_2
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            BankManager myBankManager = BankManager.GetInstance();

            string BankSPB_ID = myBankManager.CreateBank("SPB bank", 3.65, 50, 5000);
            string BankVTB_ID = myBankManager.CreateBank("VTB bank", 5.15, 30, 10000);

            string client1 = myBankManager.buildFullFeaturedClient(BankSPB_ID, "Maria", "Ivanova", "13 507", "Tipanovo 5");
            //myBankManager.ShowClientInfo(BankSPB_ID, client1);
            string client2 = myBankManager.buildMinimalClient(BankSPB_ID, "Vladimir", "Petrov");
            //myBankManager.ShowClientInfo(BankSPB_ID, client2);
            string client3 = myBankManager.buildFullFeaturedClient(BankSPB_ID, "Elena", "Vasnetsova", "25 798", "");
            myBankManager.AddClientInfo(BankSPB_ID, client3, "", "Italyanskaya 9");
            //myBankManager.ShowClientInfo(BankSPB_ID, client3);

            string client4 = myBankManager.buildFullFeaturedClient(BankVTB_ID, "Victor", "Kalashnikov", "19 518", "Krasnaya 33");
            //myBankManager.ShowClientInfo(BankSPB_ID, client1);
            string client5 = myBankManager.buildMinimalClient(BankVTB_ID, "Dmitry", "Michailov");
            //myBankManager.ShowClientInfo(BankSPB_ID, client2);

            //Accounts of REALIBLE client:

            string client4_card1 = myBankManager.CreateDebitAccount(BankSPB_ID, client2, DateTime.Now, 5000);
   
            string client1_card1 = myBankManager.CreateDebitAccount(BankSPB_ID, client1, DateTime.Now, 20000);
            string client1_card2 = myBankManager.CreateDepositAccount(BankSPB_ID, client1, DateTime.Now, 15000, DateTime.Now.AddMonths(2));
            string client1_card3 = myBankManager.CreateCreditAccount(BankSPB_ID, client1, DateTime.Now, 10000, 27000);
            //myBankManager.ShowInfo(client1_card1);
            myBankManager.AddMoney(client1_card1, DateTime.Now, 1000);
            //myBankManager.DenyOperation(client1_card1);
            myBankManager.WithdrawMoney(client1_card1, DateTime.Now.AddDays(1), 4000);
            //myBankManager.DenyOperation(client1_card1);
            myBankManager.Transfer(client1_card1, client4_card1, DateTime.Now.AddDays(10), 5000);
            //myBankManager.DenyOperation(client1_card1);
            
            myBankManager.ShowInfo(client1_card1);

            //Accounts UNRELIABLE
            /*
             * string client5_card1 = myBankManager.CreateDebitAccount(BankSPB_ID, client5, DateTime.Now, 20000);
            string client5_card1 = myBankManager.CreateDepositAccount(BankSPB_ID, client5, DateTime.Now, 15000, DateTime.Now.AddMonths(2));
            string client5_card1 = myBankManager.CreateCreditAccount(BankSPB_ID, client5, DateTime.Now, 10000, 27000);

            myBankManager.AddMoney(client5_card1, DateTime.Now, 1000);
            myBankManager.WithdrawMoney(client5_card1, DateTime.Now.AddDays(1), 4000);
            myBankManager.Transfer(client1_card1, client5_card1, DateTime.Now.AddDays(10), 5000);
            */

            myBankManager.TimeMashine(DateTime.Now.AddDays(3), client1_card1);
            //myBankManager.TimeMashine(DateTime.Now.AddMonths(1), client1_card1);
            //myBankManager.TimeMashine(DateTime.Now.AddYears(1), client1_card1);
        }
    }
}

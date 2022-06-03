using System;

namespace OPPLab5_2
{
    class Transaction
    {
        public DateTime time { get; }
        public Command Command { get; }
        public string Sender { get; }
        public string Recipient { get; }
        public CommandType Type { get; }


        public Transaction(DateTime time, Command command, string recipient, string sender, CommandType type)
        {
            this.time = time;
            Command = command;
            Sender = sender;
            Recipient = recipient;
            Type = type;
        }

        public void ShowTransactionInfo()
        {
            Console.WriteLine("Date: " + time + "\tSum: " + Command.Sum + "\tcommand: " + Type);
        }
    }
}

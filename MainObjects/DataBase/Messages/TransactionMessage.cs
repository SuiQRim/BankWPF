using System;
using BankObjects.ClientPrefab;
using BankObjects.CardPrefab;

namespace BankObjects.EventMessages.Transaction
{
    public class TransactionMessage
    {
        public TransactionMessage(string ActionType, double Cash,
            Client Sender, Card CardSender, Client Recipient, Card CardRecipient, double CashBack, double Commission)
        {

            Id = IdCount++;
            this.Cash = Cash;
            this.ActionType = ActionType;
            this.Sender = Sender;
            this.CardSender = CardSender;
            this.Recipient = Recipient;
            this.CardRecipient = CardRecipient;
            this.Commission = Cash - Commission;
            this.CashBack = CashBack;
            this.Status = new TransactionInWaiting();
            this.TimeToSend = DateTime.Now;
        }

        public TransactionMessage(string ActionType, TransactionMessage TransactionMsg)
        {
            Id = IdCount++;
            this.ActionType = ActionType;
            this.Cash = TransactionMsg.Cash - TransactionMsg.Commission;
            this.Sender = TransactionMsg.Sender;
            this.CardSender = TransactionMsg.CardSender;
            this.Recipient = TransactionMsg.Recipient;
            this.CardRecipient = TransactionMsg.CardRecipient;
            this.Commission = TransactionMsg.Commission;
            this.Status = TransactionMsg.Status;
            this.TimeToSend = DateTime.Now;  
        }
        public TransactionMessage(TransactionMessage TransactionMsg)
        {
            Id = IdCount++;
            this.ActionType = "Возвращено";
            this.Cash = TransactionMsg.Cash + TransactionMsg.Commission;
            this.Sender = null;
            this.CardSender = null;
            this.Recipient =  TransactionMsg.Sender;
            this.CardRecipient = TransactionMsg.CardSender;
            this.Commission = TransactionMsg.Commission;
            this.Status = TransactionMsg.Status;
            this.TimeToSend = DateTime.Now;
        }
       

        private static int IdCount = 0;

        public int Id { get; protected set; }

        public string ActionType { get; set; }

        public TransactionStatus Status { get; set; }

        public DateTime TimeToSend { get; protected set; }

        public Client? Sender { get; private set; }

        public Card? CardSender { get; private set; }

        public Client? Recipient { get; private set; }

        public Card? CardRecipient { get; private set; } 

        public double Cash { get; set; }

        public double CashBack { get; set; }

        public double Commission { get;  set; }


        public override string ToString()
        {
            string text =
                $"|| Действие:      {ActionType,20} ||\n" +
                $"|| Отправитель    {Sender,20} ||\n" +
                $"|| С карты        {CardSender,20} ||\n\n" +
                $"|| Получатель     {Recipient,20} ||\n" +
                $"|| На карту       {CardRecipient,20} ||\n\n" +
                $"|| Сумма перевода {Cash,20} ||\n" +
                $"|| Коммиссия      {Commission,20} ||\n\n" +
                $"|| Статус         {Status,20} ||\n";
            return text;
        }

    }
}

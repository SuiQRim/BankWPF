using System;
using BankObjects.ClientPrefab;
using BankObjects.CardPrefab;

namespace BankObjects.DataBase.Messages
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
            Status = new TransactionInWaiting();
            TimeToSend = DateTime.Now;
        }

        public TransactionMessage(string ActionType, TransactionMessage TransactionMsg)
        {
            Id = IdCount++;
            this.ActionType = ActionType;
            Cash = TransactionMsg.Cash - TransactionMsg.Commission;
            Sender = TransactionMsg.Sender;
            CardSender = TransactionMsg.CardSender;
            Recipient = TransactionMsg.Recipient;
            CardRecipient = TransactionMsg.CardRecipient;
            Commission = TransactionMsg.Commission;
            Status = TransactionMsg.Status;
            TimeToSend = DateTime.Now;
        }
        public TransactionMessage(TransactionMessage TransactionMsg)
        {
            Id = IdCount++;
            ActionType = "Возвращено";
            Cash = TransactionMsg.Cash + TransactionMsg.Commission;
            Sender = null;
            CardSender = null;
            Recipient = TransactionMsg.Sender;
            CardRecipient = TransactionMsg.CardSender;
            Commission = TransactionMsg.Commission;
            Status = TransactionMsg.Status;
            TimeToSend = DateTime.Now;
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

        public double Commission { get; set; }


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

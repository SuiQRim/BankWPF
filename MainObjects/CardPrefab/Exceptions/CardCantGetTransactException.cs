using System;
using BankObjects.DataBase.Messages;


namespace BankObjects.CardPrefab.Exceptions
{
    internal class CardCantGetTransactException : Exception
    {
        public CardCantGetTransactException(Card Card, TransactionMessage TransactionMessage) 
        {
            SourceCard = Card;
            this.TransactionMessage = TransactionMessage;
        }

        private readonly Card SourceCard;

        private readonly TransactionMessage TransactionMessage;

        public override string Message => Msg();
           
        private string Msg()
        {
            string msg = $"Данному типу карт запрещено совершать тип транзакций \"{TransactionMessage.ActionType}\"" 
                + MsgSource() + MsgSourceType();

            return msg;
        }

        private string MsgSource() => $"\nОшибка вызвана {SourceCard}";

        private string MsgSourceType() => $"\nТип {SourceCard.GetType()}";
    }
}

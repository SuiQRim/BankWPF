using System;

namespace BankObjects.CardPrefab.Exceptions
{
    internal class NotEnoughFundsException : Exception
    {
        public NotEnoughFundsException(Card card, double Cash)
        {
            this.CardSource = card;
            this.Cash = Cash;
        }

        private readonly Card CardSource;

        private readonly double Cash;

        public override string Message => $"На карте {CardSource} не хватает " +
            $"средств для перевода на сумму {Cash:N}₽\n";
    }
}

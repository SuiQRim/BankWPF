using System;
using BankObjects.ClientPrefab.Agregates.Status;
using BankObjects.ClientPrefab.Agregates.Reputation;

namespace BankObjects.CardPrefab.CreditCard
{
    public static class CreditController
    {
        private static Random rnd = new();

        /// <summary>
        /// Высчитывает Макс кол-во кредитных карт
        /// </summary>
        /// <param name="status">Статус</param>
        /// <param name="reputation">Репутация</param>
        /// <returns></returns>
        public static int MaxCountCredit(ClientStatus status, ClientReputation reputation)
        {
            int a = Convert.ToInt32((status.Level * 1.6) - 1 + (1.6 * reputation.Level) - 3.2);

            if (a < 1) a = 1;

            return a;
        }

        /// <summary>
        /// Рандомно выбирает счёт кредита
        /// </summary>
        /// <param name="MaxCash"></param>
        /// <returns></returns>
        public static long CreditCardCashLimit(long MaxCash) => rnd.NextInt64(50000, MaxCash);


        /// <summary>
        /// Высчитывает процент
        /// </summary>
        /// <param name="reputation">Репутация</param>
        /// <returns></returns>
        public static double CreditPrecent(ClientReputation reputation, ClientStatus status)
            => 16 - (status.Level + reputation.Level * 1.5);

    }
}

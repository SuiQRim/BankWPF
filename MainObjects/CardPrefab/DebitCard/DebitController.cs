using System;
using BankObjects.ClientPrefab.Agregates.Status;
using BankObjects.ClientPrefab.Agregates.Reputation;
using System.Windows;

namespace BankObjects.CardPrefab.DebitCard
{
    public class DebitController
    {
        private static Random rnd = new();
        /// <summary>
        /// Рандомно выбирает счёт
        /// </summary>
        /// <param name="maxCash">Максимальный счёт клиента</param>
        /// <returns></returns>
        public static double DebitCardCash(long maxCash) => rnd.NextInt64(5000, maxCash);


        /// <summary>
        /// Рандомно считывает кешбек, с определённым процентом от счёта
        /// </summary>
        /// <param name="cash"></param>
        /// <returns></returns>
        public static double DebitCardCashBack(double cash)
        {
            double a = rnd.Next(2, 15);
            a /= 100;
            cash = Convert.ToInt64(cash * a);
            double cashBack = rnd.Next(100, Convert.ToInt32(cash));
            return cashBack;
        }

        /// <summary>
        /// Высчитывает макс. кол-во дебетовых карт
        /// </summary>
        /// <param name="status">Статус</param>
        /// <param name="reputation">Репутация</param>
        /// <returns></returns>
        public static int MaxCountDebit(ClientStatus status, ClientReputation reputation) =>
            Convert.ToInt32(status.Level + (1.5 * reputation.Level + 1) - 1.6);

        static public MessageBoxResult DestroyDebit(double cardBalance, double cardCashBack)
        {

            if (cardBalance != 0)
            {
                MessageBox.Show($"Для уничтожения карты нужно вывести весь баланс", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return MessageBoxResult.No;
            }

            if (cardCashBack != 0)
            {
                return MessageBox.Show($"При уничтожении карты Накопленный кешбэк будет утерян \n\nПродолжить?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Stop);
            }

            return MessageBoxResult.Yes;
        }
    }
}

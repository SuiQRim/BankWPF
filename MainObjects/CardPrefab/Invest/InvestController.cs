using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using BankObjects.ClientPrefab.Agregates.Status;
using BankObjects.ClientPrefab.Agregates.Reputation;

namespace BankObjects.CardPrefab.Invest
{
    public class InvestController
    {
        private static Random rnd = new();

        /// <summary>
        /// Симулирует то, что будет с инвестицией на протяжении всего срока и записывает каждый месяц
        /// </summary>
        /// <param name="capital">Взнос</param>
        /// <param name="precent">Процент годовых</param>
        /// <param name="monthCount">Срок в мес</param>
        /// <param name="isAccumulation">С капитализацией или нет</param>
        /// <returns></returns>
        public static ObservableCollection<InvestInfo> InvestHistory(double capital, double precent, int monthCount, int monthNow, bool isAccumulation)
        {

            ObservableCollection<InvestInfo> collection = new();
            Brush brush = Brushes.Green;
            double finalyPrecent = precent / (100 * 12);

            double sum;
            if (isAccumulation)
            {
                for (int i = 0; i < monthCount; i++)
                {
                    sum = capital * finalyPrecent;
                    capital += sum;

                    if (i > monthNow - 1)
                    {
                        brush = Brushes.Red;
                    }
                    collection.Add(new InvestInfo(capital, sum, i + 1, brush));
                    
                }
            }

            else
            {
                for (int i = 0; i < monthCount; i++)
                {

                    sum = capital * finalyPrecent;
                    if (i > monthNow - 1)
                    {
                        brush = Brushes.Red;
                    }
                    collection.Add(new InvestInfo(capital + sum * (i + 1), sum, i + 1, brush));
                }
            }

            return collection;
        }

        /// <summary>
        /// Высчитывает процент
        /// </summary>
        /// <param name="reputation">Репутация</param>
        /// <returns></returns>
        public static double InvestmenPrecent(ClientReputation reputation, ClientStatus status)
        {
            int i = 0;

            if (status.Level == 2) i = 2;

            return reputation.Level + 1 + i;
        }

        /// <summary>
        /// Симулирует то, что будет с инвестицией на протяжении всего срока и записывает только результат
        /// </summary>
        /// <param name="capital">Взнос</param>
        /// <param name="precent">Процент годовых</param>
        /// <param name="monthCount">Срок в мес</param>
        /// <param name="isAccumulation">С капитализацией или без</param>
        /// <returns></returns>
        public static InvestInfo InvestHistoryWithoutWrite(double capital, double precent,
            int monthCount, bool isAccumulation)
        {
            InvestInfo invest;
            double sum;

            //Если капитализация присутствует то используем одну формулу для расчёта результата,
            //а если отсутсвует, то другую

            if (isAccumulation) sum = capital * Math.Pow((1 + (precent / 100) / 12), monthCount);
            else sum = capital + (capital * (precent / 100) * (30 * monthCount)) / 360;

            invest = new InvestInfo(sum, 0, monthCount, Brushes.Red);

            return invest;
        }

        /// <summary>
        /// Рандомный баланс инвестиции
        /// </summary>
        /// <returns></returns>
        public static double CashInvestment() => rnd.NextInt64(500000);



    }
}

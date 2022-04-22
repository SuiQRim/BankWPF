using System;
using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using BankObjects.CardPrefab;
using BankObjects.CardPrefab.Invest;
using BankObjects.ClientPrefab.Agregates.Reputation;
using BankObjects.ClientPrefab.Agregates.Status;
using BankObjects.EventMessages;
using BankObjects.CardPrefab.CreditCard;
using BankObjects.CardPrefab.DebitCard;
using System.Data.SqlClient;

namespace BankObjects.ClientPrefab.Agregates
{
    internal static class ClientController
    {
        /// <summary>
        /// Добавляет кредитную карту
        /// </summary>
        /// <param name="сreditList">Список кредитных карт</param>
        /// <param name="competence">Ограничители</param>
        /// <param name="reputation">Репутация</param>
        /// <returns></returns>
        public static bool AddCreditValidator(
             ObservableCollection<Card> сreditList, ClientCompetence competence, ClientReputation reputation)
        {

            if (reputation.Level == 1)
            {
                MessageBox.Show("К сожалению новая кредитная карта отклонена... Ваша репутация слишком низкая.", "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }

            else if (competence.MaxCreditCard > сreditList.Count)
            {
                MessageBox.Show("Новая кредитная карта одобрена и добавлена!");
                return true;
            }

            else
            {
                CardCantBeCreated(competence.MaxCreditCard, "Кредитных");
                return false;
            }

        }

        /// <summary>
        /// Добавляет дебетовую карту
        /// </summary>
        /// <param name="debitList">Список дебетовых карт</param>
        /// <param name="competence">Ограничения</param>
        /// <returns></returns>
        public static bool AddDebitValidator(ObservableCollection<Card> debitList, 
            ClientCompetence competence)
        {
            if (competence.MaxDebitCard > debitList.Count)
            {
                MessageBox.Show("Новая дебетовая карта одобрена и добавлена!");
                return true;
            }

            else 
            {
                CardCantBeCreated(competence.MaxDebitCard, "Дебетовых");
                return false;
            }

        }


        /// <summary>
        /// Обновления данных всех карт в списке
        /// </summary>
        /// <param name="CardList">список</param>
        /// <returns></returns>
        public static ObservableCollection<Card> CardUpDate(ObservableCollection<Card> CardList)
        {

            foreach (Card card in CardList)
            {
                card.UpData();
            }
            return CardList;
        }

        /// <summary>
        /// Обновления данных всех инвестиций в списке
        /// </summary>
        /// <param name="InvestList">список инвестиций</param>
        /// <returns></returns>
        public static ObservableCollection<Card> InvestmentUpDate(ObservableCollection<Card> InvestList)
        {
            List<Investment> delitedInvest = new();

            foreach (Investment invest in InvestList)
            {
                if (!invest.isActivated)
                {
                    delitedInvest.Add(invest);
                    continue;
                }
                invest.UpData();
            }

            foreach (var item in delitedInvest)
            {
                InvestList.Remove(item);
            }

            return InvestList;
        }

        private static readonly Random rnd = new Random();

        public static ClientStatus RandomClientStatus() => 
            StatusFactory.GetStatusUsingLVL(rnd.Next(0, 3));

        public static ClientReputation RandomClientReputation() =>
            ReputatuonFactory.GetReputationUsingLVL(rnd.Next(0, 4));

        static public void CardCantBeCreated(int Max, string Type)
        {
            MessageBox.Show($"Вы не можете создать больше {Max} {Type} карт", "Внимание!",
                MessageBoxButton.OK, MessageBoxImage.Stop);
        }

    }
}

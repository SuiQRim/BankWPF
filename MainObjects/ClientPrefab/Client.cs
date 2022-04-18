using System;
using System.Collections.ObjectModel;
using NameGenerator.Generators;
using BankObjects.ClientPrefab.Agregates;
using BankObjects.ClientPrefab.Agregates.Status;
using BankObjects.ClientPrefab.Agregates.Reputation;
using InfoWork;
using BankObjects.EventMessages;
using BankObjects.CardPrefab;
using BankObjects.CardPrefab.DebitCard;
using BankObjects.CardPrefab.Invest;
using BankObjects.CardPrefab.CreditCard;
using System.Data.SqlClient;

namespace BankObjects.ClientPrefab
{
    public class Client
    {

        private static readonly string IdTag = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";


        public Client(BankEvents bankEvents) {

            RealNameGenerator NameRnd = new();

            PhoneNumber = "+7(***)(***)(**)(**)";
            AccountCreateDate = Convert.ToString(DateTime.Now.ToString("dd.MM.yyyy"));

            Name = NameRnd.Generate();
            MyEvents = EventData.GetEvents(4);

            AccountID = IDCreater.GetRandomChar(16, IdTag);
            Status = ClientController.RandomClientStatus();
            Reputation = ClientController.RandomClientReputation();
            Competence = new(Status, Reputation);
            AddCard += bankEvents.NewCard;

        }

        public Client(string Name, ClientStatus status, BankEvents bankEvents)
        {
            
            PhoneNumber = "+7(***)(***)(**)(**)";
            AccountCreateDate = Convert.ToString(DateTime.Now.ToString("dd.MM.yyyy"));

            this.Name = Name;
            MyEvents = EventData.GetEvents(4);

            AccountID = IDCreater.GetRandomChar(16, IdTag);
            Status = status;
            Reputation = ClientController.RandomClientReputation();
            Competence = new(Status, Reputation);
            AddCard += bankEvents.NewCard;
        }

        public Client(ClientSet clientSet, BankEvents bankEvents) 
        {
            AccountID = clientSet.AccountId;
            Name = clientSet.Name;
            PhoneNumber = clientSet.PhoneNumber;
            Status = ClientStatus.GetStatusUsingLVL(clientSet.Status);
            Reputation = ClientReputation.GetReputationUsingLVL(clientSet.Reputation);
            Competence = new(Status, Reputation);
            AccountCreateDate = clientSet.DateToCreate;
            AddCard += bankEvents.NewCard;

            UpLoadCardData(bankEvents);

            MyEvents = EventData.GetEvents(4);
        }

        //Имя
        public string Name { get; private set; }

        //Дата создания счета
        public string AccountCreateDate { get; private set; }

        //ID банк. счета
        public string AccountID { get; private set; }

        //Номер телефона
        public string PhoneNumber { get; private set; }

        
        //Список акивны кешбек акций
        public ObservableCollection<EventData> MyEvents { get; private set; }

        //Статус
        public ClientStatus Status { get; private set; }

        //Репутация
        public ClientReputation Reputation { get; private set; }

        //Ограничение по картам
        public ClientCompetence Competence { get; private set; }



        //Список дебетовых карт
        public ObservableCollection<Card> MyDebitCards { get; private set; } = new();

        //Список кредитных карт
        public ObservableCollection<Card> MyCreditCards { get; private set; } = new();

        //Список вложенных карт
        public ObservableCollection<Card> MyInvestments { get; private set; } = new();

        private void UpLoadCardData(BankEvents bankEvents) 
        {

            MyDebitCards = bankEvents.GetCardCollectionFromDataBase(this,"Debit");
            MyCreditCards = bankEvents.GetCardCollectionFromDataBase(this, "Credit");
            MyInvestments = bankEvents.GetCardCollectionFromDataBase(this, "Invest");

        }

        /// <summary>
        /// Обновляет данные о картах в списках
        /// </summary>
        public void UpDate() {

            MyEvents = EventData.GetEvents(4);
            MyDebitCards = ClientController.CardUpDate(MyDebitCards);
            MyCreditCards = ClientController.CardUpDate(MyCreditCards);
            MyInvestments = ClientController.InvestmentUpDate(MyInvestments);
        }

        private event Action<string,Client, Card> AddCard;

        /// <summary>
        /// Добавляет кредитную карту
        /// </summary>
        /// <param name="newCredit"></param>
        public void AddCreditCard(Credit newCredit)
        {
            bool CanCreate = ClientController.AddCreditValidator(MyCreditCards, Competence, Reputation);
            if (CanCreate)
            {
                MyCreditCards.Add(newCredit);
                AddCard?.Invoke("кредитная карта",this,newCredit);
            }
        }

        /// <summary>
        /// Добавляет дебетовую карту
        /// </summary>
        public void AddDebitCard(Debit newDebit)
        {
            bool CanCareate = ClientController.AddDebitValidator(MyDebitCards, Competence);
            if (CanCareate)
            {
                MyDebitCards.Add(newDebit);
                AddCard?.Invoke("дебетовая карта", this, newDebit);
            }
        }

        /// <summary>
        /// Добавляет инвестицию
        /// </summary>
        /// <param name="newInvest"></param>
        public void AddInvestment(Investment newInvest) { 

            bool CanCreate = true;

            if (CanCreate)
            {
                MyInvestments.Add(newInvest);
                AddCard?.Invoke("инвестиция", this, newInvest);
            }

        }

        public override string ToString() => Name ?? "Клиент не обнаружен";
       
    }
}

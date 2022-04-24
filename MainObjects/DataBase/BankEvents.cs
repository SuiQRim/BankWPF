using System.Collections.ObjectModel;
using BankObjects.CardPrefab;
using BankObjects.CardPrefab.DebitCard;
using BankObjects.CardPrefab.CreditCard;
using BankObjects.CardPrefab.Invest;
using BankObjects.ClientPrefab;
using System.Data.SqlClient;
using System.Linq;
using System.Diagnostics;
using Z.EntityFramework.Plus;
using BankObjects.DataBase.Messages;

namespace BankObjects.DataBase
{
    public class BankEvents
    {
        public BankEvents(SqlConnectionStringBuilder bankSystemSQLServer_Path)
        {
            BankDBContext = new(bankSystemSQLServer_Path.ConnectionString);
        }

        private static BankDBContext BankDBContext;

        //Хранилище событий о всех транзакциях
        public ObservableCollection<TransactionMessage> TransactionMessages { get; set; } = new();

        //Хранилище событий о открытиях\закрытиях счетов
        public ObservableCollection<AccountMessage> AccountMessages { get; set; } = new();



        public void Transaction(TransactionMessage transactionMessage, Card card)
        {

            //Изменяем в базе карт все изменяемые во время транзакций поля.
            switch (card)
            {
                case Debit:

                    BankDBContext.DebitCard.Where(c => c.CardId == card.CardId).
                        Update(c => new()
                        {
                            Balance = card.Balance,
                            CashBack = (card as Debit)!.CashBack
                        });

                    break;

                case Credit:
                    Credit credit = card as Credit;

                    BankDBContext.CreditCard.Where(c => c.CardId == card.CardId).
                        Update(c => new()
                        {
                            Balance = credit.Balance,
                            Debt = credit.Debt
                        });
                    break;

                case Investment:
                    Investment invest = card as Investment;
                    BankDBContext.Investment.Where(c => c.CardId == card.CardId).
                        Update(c => new()
                        {
                            Balance = invest.Balance,
                            AccumulationBalance = invest.AccumulationBalance,
                        });
                    break;
            }

            TransactionMessages.Add(transactionMessage);
        }

        public static async void CardUpData(Card card)
        {

            //Изменяем в базе карт все изменяемые во время транзакций поля.
            switch (card)
            {
                case Debit:

                    BankDBContext.DebitCard.Where(c => c.CardId == card.CardId).
                        Update(c => new()
                        {
                            Balance = card.Balance,
                            CashBack = (card as Debit).CashBack
                        });

                    break;

                case Credit:
                    Credit credit = card as Credit;

                    BankDBContext.CreditCard.Where(c => c.CardId == card.CardId).
                        Update(c => new()
                        {
                            Balance = credit.Balance,
                            MonthNow = credit.MonthNow,
                            Debt = credit.Debt,
                            IsFreez = credit.IsFreez,
                            IsReady = credit.IsReady
                        });

                    break;

                case Investment:
                    Investment invest = card as Investment;

                    BankDBContext.Investment.Where(c => c.CardId == card.CardId).
                        Update(c => new()
                        {
                            Balance = invest.Balance,
                            AccumulationBalance = invest.AccumulationBalance,
                            MonthNow = invest.MonthNow,
                            IsActivated = invest.isActivated,
                            IsReady = invest.isReady
                        });

                    break;
            }

        }

        /// <summary>
        /// Добавляет в базу нового клиента
        /// </summary>
        /// <param name="client"></param>
        public void NewClient(Client client)
        {

            BankDBContext.Client.Add(new ClientSet(client));
            BankDBContext.SaveChanges();

            AccountMessages.Add(new AccountMessage("Открыт банковский счет", client.AccountID, client.Name, ""));
        }

        /// <summary>
        /// Добавляет нову карту в базу данных
        /// </summary>
        /// <param name="actionType">Тип события</param>
        /// <param name="client">Клиент</param>
        /// <param name="card">Карта которая добавляется</param>
        public void NewCard(string actionType, Client client, Card card)
        {
            AccountMessage accountMessage = new("Оформлена " + actionType, client.AccountID, client.Name, card.CardId);

            switch (card)
            {
                case Debit:
                    BankDBContext.DebitCard.Add(new DebitSet(card as Debit));
                    break;

                case Credit:
                    BankDBContext.CreditCard.Add(new CreditSet(card as Credit));
                    break;

                case Investment:
                    BankDBContext.Investment.Add(new InvestmentSet(card as Investment));
                    break;
            }

            BankDBContext.SaveChanges();

            AccountMessages.Add(accountMessage);
        }

        /// <summary>
        /// Удаляет карту из базы данных
        /// </summary>
        /// <param name="actionType">Тип события</param>
        /// <param name="client">Клиент</param>
        /// <param name="card">Карта которая добавляется</param>
        public void CardDestroy(string actionType, Client client, Card card)
        {
            AccountMessage accountMessage = new("Закрыта " + actionType, client.AccountID, client.Name, card.CardId);

            BankDBContext.DebitCard.Where(c => c.CardId == card.CardId).
                Delete();

            AccountMessages.Add(accountMessage);
        }

        /// <summary>
        /// В базе данных у инвестиции меняет значения на активировано
        /// </summary>
        /// <param name="invest">Активированная инвестиция</param>
        public void InvestmentActivate(Card investment)
        {
            Investment invest = investment as Investment;

            AccountMessage accountMessage = new("Инвестиция активирована",
                invest.HolderID, invest.HolderName, invest.CardId);

            BankDBContext.Investment.Where(c => c.CardId == invest.CardId).
                Update(c => new()
                {
                    IsActivated = invest.isActivated,
                });

            AccountMessages.Add(accountMessage);
        }

        /// <summary>
        /// В базе данных у инвестиции меняет значения на завершено
        /// </summary>
        /// <param name="investment"></param>
        public void InvestmentFinish(Card investment)
        {
            Investment invest = investment as Investment;
            AccountMessage accountMessage = new("Инвестиция прошла срок",
                invest.HolderID, invest.HolderName, invest.CardId);

            BankDBContext.Investment.Where(c => c.CardId == invest.CardId).
                Update(c => new()
                {
                    Balance = invest.Balance,
                    AccumulationBalance = invest.AccumulationBalance,
                    MonthNow = invest.MonthNow,
                    IsActivated = invest.isActivated,
                    IsReady = invest.isReady
                });

            AccountMessages.Add(accountMessage);

        }

        /// <summary>
        /// Собирает из базы данных всех клиентов
        /// </summary>
        /// <param name="bankEvents"></param>
        /// <returns></returns>
        public ObservableCollection<Client> GetClientFromDataBase()
        {
            ObservableCollection<Client> clientList = new();

            var clientSets = BankDBContext.Client;
            foreach (var clientSet in clientSets)
            {
                clientList.Add(new Client(clientSet, this));
            }

            return clientList;
        }

        public static void IncreasBalance(Debit debit, double value)
        {
            debit.Balance += value;
            BankDBContext.DebitCard.Where(c => c.CardId == debit.CardId).
               Update(c => new() { Balance = debit.Balance });
        }

        public ObservableCollection<Card> GetCardCollectionFromDataBase(Client client, string TypeOfCard)
        {
            ObservableCollection<Card> cardCollection = new();

            switch (TypeOfCard)
            {
                case "Debit":
                    var debitSets = BankDBContext.DebitCard.Where(c => c.HolderId == client.AccountID);
                    foreach (var card in debitSets)
                    {
                        Debug.WriteLine($"{card.CardId} {card.HolderName} {card.HolderId}");
                        cardCollection.Add(new Debit(card, this));
                    }
                    break;

                case "Credit":
                    var creditSets = BankDBContext.CreditCard.Where(c => c.HolderId == client.AccountID);
                    foreach (var card in creditSets)
                    {
                        cardCollection.Add(new Credit(card, this));
                    }
                    break;

                case "Invest":
                    var investSets = BankDBContext.Investment.Where(c => c.HolderId == client.AccountID);
                    foreach (var card in investSets)
                    {
                        cardCollection.Add(new Investment(card, this));
                    }
                    break;
            }

            return cardCollection;
        }

    }
}

using System;
using System.Windows;
using System.Collections.ObjectModel;
using BankObjects.ClientPrefab.Agregates;
using BankObjects.CardPrefab.Exceptions;
using ViewModel;
using System.Data.SqlClient;
using BankObjects.DataBase.Messages;
using BankObjects.DataBase;


namespace BankObjects.CardPrefab
{
    abstract public class Card : VMNotifyPropertyChanged
    {
        /// <summary>
        /// Конструктор заполняющий только особую часть карты\вклада
        /// </summary>
        /// <param name="startId"></param>
        /// <param name="charCount"></param>
        public Card(string startId, int charCount, string HolderName, string HolderID, BankEvents bankEvents)
        {
            StartId = startId;
            CreateId(charCount);
            this.HolderID = HolderID;
            this.HolderName = HolderName;

            Balance = 0;
            CardCreateData = Convert.ToString(DateTime.Now.ToString("dd.MM.yyyy"));

            Transaction_Get_Event += bankEvents.Transaction;
            Transaction_Send_Event += bankEvents.Transaction;
            DBUpData += BankEvents.CardUpData;
        }

        /// <summary>
        /// Конструктор для автоматического созддания карты
        /// </summary>
        /// <param name="competence"></param>
        /// <param name="startId"></param>
        /// <param name="charCount"></param>
        public Card(ClientCompetence competence, string startId, int charCount, string HolderName, string HolderID, BankEvents bankEvents)
        {
            StartId = startId;

            CreateId(charCount);

            this.HolderID = HolderID;
            this.HolderName = HolderName;

            KitSart(competence);

            CardCreateData = Convert.ToString(DateTime.Now.ToString("dd.MM.yyyy"));

            Transaction_Get_Event += bankEvents.Transaction;
            Transaction_Send_Event += bankEvents.Transaction;
            DBUpData += BankEvents.CardUpData;
        }

        public Card(CardSet cardSet, BankEvents bankEvents)
        {
            CardId = cardSet.CardId;
            StartId = CardId;
            HolderName = cardSet.HolderName;
            HolderID = cardSet.HolderId;
            Balance = cardSet.Balance;
            CardCreateData = cardSet.DateTotCreate;
            Transaction_Get_Event += bankEvents.Transaction;
            Transaction_Send_Event += bankEvents.Transaction;
            DBUpData += BankEvents.CardUpData;
        }
        /// <summary>
        /// Метод использущийся в авто конструкторе для особых действий в наследуемых классах
        /// </summary>
        /// <param name="competence"></param>
        protected virtual void KitSart(ClientCompetence competence) { }


        /// <summary>
        /// Создает Id карты и скрытый Id
        /// </summary>
        /// <param name="charCount"></param>
        protected virtual void CreateId(int charCount) {

            CardId = StartId + "-";

            CardId += IDCreater.GetRandomChar(12, symbols);

            CardIdLastChars = IDCreater.LastFourChar(CardId);

        }


        /// <summary>
        /// Обновление внутренних данных карты
        /// </summary>
        public virtual void UpData() => DBUpData?.Invoke(this);


        private event Action<Card> DBUpData;


        protected string StartId;

        protected static readonly Random rnd = new();

        protected static readonly string symbols = "1234567890";


        public string CardId { get; protected set; } = "";
        public string CardIdLastChars { get; protected set; } = "";

        public string HolderName { get; protected set; }
        public string HolderID { get; protected set; }


        private double _Balance;
        public double Balance {

            get => _Balance;
            set => Set(ref _Balance, value);
        }

        public string CardCreateData { get; set; }


        protected abstract void TransactionCalculate(TransactionMessage transactionMessage);

        protected abstract TransactionMessage TransactionSetter(TransactionMessage transactionMessage);

        protected abstract void TransactionValidator(TransactionMessage transactionMessage);


        // Событие отправки транзакции
        private event Action<TransactionMessage, Card> Transaction_Send_Event;

        // Событие отправки транзакции
        private event Action<TransactionMessage, Card> Transaction_Get_Event;

        // Коллекция с Историей транзакций
        public ObservableCollection<TransactionMessage> TransactionMessages { get; private set; } = new();


        /// <summary>
        /// Работает с балансом при транзакции со стороны отправителя
        /// </summary>
        /// <param name="transactionMessage"></param>
        /// <returns></returns>
        public void TransactionSend(TransactionMessage transactionMessage) {

            try
            {
                TransactionValidator(transactionMessage);
            }
            catch (NotEnoughFundsException)
            {
                // Запрос отклонен, добавляем в историю
                transactionMessage.Status = new TransactionRejected();
                TransactionMessages.Add(transactionMessage);

                // Событие об отправке (неудавшейся)
                MessageBox.Show("Недостаточно средств на карте", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Transaction_Send_Event?.Invoke(transactionMessage, this);

                return;
            }

            TransactionCalculate(transactionMessage);

            // Запрос принят добавляем в историю
            transactionMessage.Status = new TransactionConfirmed();
            TransactionMessages.Add(transactionMessage);
            
            // Запускаем событие об отравке счета
            Transaction_Send_Event?.Invoke(transactionMessage, this);
            // Отправляем запрос транзакции к принимающей карте с новым экземплром сообщения
            transactionMessage.CardRecipient!.TransactionGet(new ("Принято", transactionMessage));
        }


        /// <summary>
        /// Работает с балансом при транзакции со стороны принимающего 
        /// </summary>
        /// <param name="transactionMessage"></param>
        /// <returns></returns>
        public void TransactionGet(TransactionMessage transactionMessage)
        {
            try
            {
                TransactionSetter(transactionMessage);
            }
            catch (CardCantGetTransactException)
            {
                // Возвращаем средства отправителю
                transactionMessage.CardSender!.TransactionGet(new TransactionMessage (transactionMessage));
                return;
            }

            transactionMessage.Status = new TransactionConfirmed();
            TransactionMessages.Add(transactionMessage);
            //Запускаем событие о принятии счета
            Transaction_Get_Event?.Invoke(transactionMessage, this);
        }

        public static double operator +(double sum, Card card1) => sum + card1.Balance;
        public static double operator +(Card card1, Card card2) => card1.Balance + card2.Balance;

        public override string ToString() {  return CardId ?? "Не обнаружено";  }
            
    }
}

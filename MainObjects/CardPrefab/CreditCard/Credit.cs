using BankObjects.EventMessages.Transaction;
using BankObjects.ClientPrefab.Agregates;
using BankObjects.EventMessages;
using BankObjects.CardPrefab.Exceptions;
using System.Data.SqlClient;

namespace BankObjects.CardPrefab.CreditCard
{
    public class Credit : Card
    {

        public Credit(ClientCompetence competence, string HolderName, string HolderID,
            BankEvents bankEvents) : base (competence, "8634", 12, HolderName, HolderID, bankEvents) 
        {
            MonthNow = 0;
        }

        public Credit(double value, double monthDebt, int monthCount ,double percent,
            string HolderName, string HolderID, BankEvents bankEvents) : base("8634", 12, HolderName, HolderID, bankEvents) {

            StartBalance = value;
            Balance = value;
            MonthDebt = monthDebt;
            Debt = 0;
            Precent = percent;
            MonthCount = monthCount;
           
        }

        public Credit(CreditSet creditSet, BankEvents bankEvents) : base (creditSet, bankEvents)
        {
            StartBalance = creditSet.StartBalance;
            Debt = creditSet.Debt;
            Precent = creditSet.Precent;
            MonthDebt = creditSet.MonthDebt;
            MonthCount = creditSet.MonthCount;
            MonthNow = creditSet.MonthNow;
            IsReady = creditSet.IsReady;
            IsFreez = creditSet.IsFreez;
        }

        /// <summary>
        /// Обновление внутренних данных карты
        /// </summary>
        public override void UpData()
        {
            if (IsReady) return;

            Debt += MonthDebt;
            MonthNow++;
            
            if (MonthNow == MonthCount) IsReady = true;

            base.UpData();
        }


        protected override void KitSart(ClientCompetence competence)
        {
            StartBalance = CreditController.CreditCardCashLimit(500000);
            Balance = StartBalance;
            MonthCount = 12;
            Precent = competence.CreditPercent;
            MonthDebt = Balance;

            double finalDebt = StartBalance + (StartBalance * (Precent / 100));
            MonthDebt = finalDebt / MonthCount;

        }

        //Пройден ли срок карты
        public bool IsReady { get; set; } = false;

        //Заморожена ли карта
        public bool IsFreez { get; set; } = false;

        //Стартовый баланс
        public double StartBalance { get; set; }

        //Долг
        public double _Debt;
        public double Debt {

            get => _Debt;
            set {
                if ( IsFreez && value < MonthDebt * 3) IsFreez = false;
                else if (!IsFreez && value >= MonthDebt * 3)IsFreez = true;

                _Debt = value;
            }
        }

        public double MonthDebt { get; set; }

        //Срок на который оформлен кредит
        public int MonthCount { get; set; }

        //Текущий месяц
        public int MonthNow { get; set; }

        //Процентная ставка
        public double Precent { get; set; }

        protected override void TransactionCalculate(TransactionMessage transactionMessage)
        {
            Balance -= transactionMessage.Cash;
        }

        protected override TransactionMessage TransactionSetter(TransactionMessage transactionMessage)
        {
            //Кредитная карта не может принимать счет
            throw new CardCantGetTransactException(this, transactionMessage);
        }

        protected override void TransactionValidator(TransactionMessage transactionMessage)
        {
            if (transactionMessage.Cash > Balance)
            {
                throw new NotEnoughFundsException(this, transactionMessage.Cash);
            }
        }
    }
}

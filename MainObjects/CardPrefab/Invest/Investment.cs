using System.Windows.Media;
using System;
using System.Collections.ObjectModel;
using BankObjects.ClientPrefab.Agregates;
using BankObjects.CardPrefab.Exceptions;
using System.Data.SqlClient;
using BankObjects.DataBase.Messages;
using BankObjects.DataBase;

namespace BankObjects.CardPrefab.Invest
{
    public class Investment : CardPrefab.Card
    {
        public Investment(ClientCompetence competence, string HolderName, string HolderID, BankEvents bankEvents)
            : base(competence, "", 10, HolderName, HolderID, bankEvents) {

            Investment_Finish += bankEvents.InvestmentFinish;
            Investment_Activate += bankEvents.InvestmentActivate;

        }

        public Investment(long startBalance, double percent,
            double clearProfit, int monthCount, bool isAccumulation, string HolderName, string HolderID, BankEvents bankEvents)
            : base("", 10, HolderName, HolderID, bankEvents) {

            IsAccumulation = isAccumulation;
            StartBalance = startBalance;
            Balance = 0;
            Precent = percent;
            Profit = InvestController.InvestHistory(StartBalance, Precent, monthCount, 0,IsAccumulation);
            MonthCount = monthCount;
            ClearProfit = clearProfit;
            ProfitPrecent = (Profit[Profit.Count - 1].Balance / StartBalance * 100) - 100;

            Investment_Finish += bankEvents.InvestmentFinish;
            Investment_Activate += bankEvents.InvestmentActivate;

        }

        public Investment(InvestmentSet investSet, BankEvents bankEvents) : base (investSet, bankEvents)
        {
            StartBalance = investSet.StartBalance;
            Precent = investSet.Precent;
            ProfitPrecent = investSet.ProfitPrecent;
            AccumulationBalance = investSet.AccumulationBalance;
            ClearProfit = investSet.ClearProfit;
            MonthNow = investSet.MonthNow;
            MonthCount = investSet.MonthCount;
            isReady = investSet.IsReady;
            isActivated = investSet.IsActivated;
            IsAccumulation = investSet.IsAccumulation;
            Profit = InvestController.InvestHistory(StartBalance, Precent, MonthCount, MonthNow, IsAccumulation);
        }

        protected override void KitSart(ClientCompetence competence)
        {
            IsAccumulation = true;
            StartBalance = InvestController.CashInvestment();
            Balance = StartBalance;
            Precent = competence.InvestPrecent;
            Profit = InvestController.InvestHistory(Balance, Precent, 12, 0, true);
            ClearProfit = Profit[Profit.Count - 1].Balance - Balance;
            ProfitPrecent = (Profit[Profit.Count - 1].Balance / Balance * 100) - 100;
            isActivated = true;
        }


        protected override void CreateId(int charCount)
        {
            CardId = IDCreater.GetRandomChar(symbols);
        }

        public ObservableCollection<InvestInfo>? Profit { get; set; }

        //Пройден ли срок инвестиции
        public bool isReady { get; private set; } = false;

        //Активирована ли инвестиция 
        public bool isActivated { get; private set; } = false;

        //Взнос
        public double StartBalance { get; set; }

        //Чистая прибыль
        public double ClearProfit { get; set; }

        //Присутсвует ли капитализация
        public bool IsAccumulation { get; set; }

        //Баланс при отсутвии капитализации
        private double _AccumulationBalance;
        public double AccumulationBalance
        {

            get => _AccumulationBalance;
            set => Set(ref _AccumulationBalance, value);
        }

        public int MonthNow { get; set; } = 0;

        public int MonthCount { get; set; }

        //Процент годовых
        public double Precent { get; set; }

        //Доходность в процентах
        public double ProfitPrecent { get; set; }


        
        /// <summary>
        /// Обновление внутренних данных карты
        /// </summary>
        public override void UpData()
        {
            if (isReady) return;
           
            MonthNow++;

            Profit![MonthNow - 1] = new InvestInfo(Profit[MonthNow - 1], Brushes.Green);

            if (!IsAccumulation)
            { 
                AccumulationBalance += Profit[MonthNow - 1].ClearCash;
            }
            else
            {
                Balance = Profit[MonthNow - 1].Balance;
            }


            if (MonthNow == Profit.Count )
            {
                isReady = true;
                Balance += AccumulationBalance;
                AccumulationBalance = 0;
                Investment_Finish?.Invoke(this);
            }
            base.UpData();
        }


        private event Action<Card> Investment_Finish;

        private event Action<Card> Investment_Activate;

        /// <summary>
        /// Активирует инвестицию
        /// </summary>
        public void ActivateInvest()
        {
            isActivated = true;
            Investment_Activate?.Invoke(this);
        }

        protected override void TransactionCalculate(TransactionMessage transactionMessage)
        {
            if (isReady)
            {
                transactionMessage.Cash = Balance;
                Balance = 0;
            }
            else if (!IsAccumulation && !isReady)
            {
                transactionMessage.Cash = AccumulationBalance;
                AccumulationBalance = 0;
            }
            
        }
        protected override TransactionMessage TransactionSetter(TransactionMessage transactionMessage)
        {
            Balance += transactionMessage.Cash;
            return transactionMessage;
        }

        protected override void TransactionValidator(TransactionMessage transactionMessage)
        {
            if (AccumulationBalance == 0 && !isReady)
            {
                throw new NotEnoughFundsException(this, transactionMessage.Cash);
            }
        } 
    }
}

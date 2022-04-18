using ViewModel;
using BankObjects.CardPrefab.Invest;
using System.Diagnostics;
using System.Threading;

namespace DataSource.Child
{
    public class NewInvestment : VMNotifyPropertyChanged
    {
        private void NewInvestTrigger(long value)
        {
            InvestInfo profit = InvestController.InvestHistoryWithoutWrite(Value,
                Precent, MounthCount, IsAccumulation);

            ProfitPersent = (profit.Balance / value * 100) - 100;

            ClearProfit = value * (ProfitPersent / 100);

            Profit = Value + ClearProfit;

            Debug.WriteLine("Вычисление");
            Thread.Sleep(5);
        }

        private long _Value;

        public long Value
        {
            get => _Value;
            set
            {
                Set(ref _Value, value);
                NewInvestTrigger(_Value);
            }
        }

        private double _Profit;

        public double Profit
        {
            get => _Profit;
            set => Set(ref _Profit, value);
        }

        private double _ClearProfit;

        public double ClearProfit
        {
            get => _ClearProfit;
            set => Set(ref _ClearProfit, value);
        }

        private double _ProfitPersent;

        public double ProfitPersent
        {
            get => _ProfitPersent;
            set => Set(ref _ProfitPersent, value);
        }

        private double _Precent;

        public double Precent
        {
            get => _Precent;
            set => Set(ref _Precent, value);
        }

        private int _MounthCount;

        public int MounthCount
        {
            get => _MounthCount;
            set
            {
                Set(ref _MounthCount, value);
                NewInvestTrigger(_Value);
            }
        }

        private bool _IsAccumulation;

        public bool IsAccumulation
        {
            get => _IsAccumulation;
            set
            {
                Set(ref _IsAccumulation, value);
                NewInvestTrigger(_Value);
            }
        }
    }
}

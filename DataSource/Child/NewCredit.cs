using ViewModel;

namespace DataSource.Child
{
    public class NewCredit : VMNotifyPropertyChanged
    {
        private void NewCredit_OnMonthChange_Trigger(int value, int pastValue)
        {

            if (value < 13 && pastValue > 12)
            {
                NewCreditCorrector(Value, 10000, 500000, ClientPercent, 10000);
            }

            else if ((value > 12 & value < 25) && (pastValue > 24 ^ pastValue < 13))
            {
                NewCreditCorrector(Value, 50000, 2000000, 6, 20000);
            }

            else if ((value > 24) && (pastValue < 25))
            {
                NewCreditCorrector(Value, 200000, 8000000, 5, 100000);
            }
        }

        public void NewCreditCorrector(double value, int minimum, int maximum, double percent, int sliderStep)
        {
            Precent = percent;
            Minimum = minimum;

            if (value < minimum ^ value > maximum)
            {
                Value = minimum;
            }

            Maximum = maximum;
            SliderStep = sliderStep;
        }

        private void NewCreditTrigger(long value)
        {
            double finalDebt = value + (value * (Precent / 100));
            MonthDebt = finalDebt / MonthCount;
        }


        private double _ClientPercent;
        public double ClientPercent
        {
            get => _ClientPercent;
            set => Set(ref _ClientPercent, value);

        }


        private long _Value;
        public long Value
        {
            get => _Value;
            set
            {
                NewCreditTrigger(value);
                Set(ref _Value, value);

            }
        }


        private int _MonthCount;
        public int MonthCount
        {
            get => _MonthCount;
            set
            {
                NewCredit_OnMonthChange_Trigger(value, MonthCount);

                Set(ref _MonthCount, value);

                NewCreditTrigger(Value);

            }
        }


        private int _Maximum = 500000;
        public int Maximum
        {
            get => _Maximum;
            set
            {
                Set(ref _Maximum, value);


            }
        }


        private int _SliderStep = 10000;
        public int SliderStep
        {
            get => _SliderStep;
            set
            {
                Set(ref _SliderStep, value);

            }
        }


        private int _Minimum = 10000;
        public int Minimum
        {
            get => _Minimum;
            set
            {
                Set(ref _Minimum, value);

            }
        }


        private double _Precent;
        public double Precent
        {
            get => _Precent;
            set => Set(ref _Precent, value);
        }


        private double _MonthDebt;
        public double MonthDebt
        {
            get => _MonthDebt;
            set => Set(ref _MonthDebt, value);
        }
    }
}

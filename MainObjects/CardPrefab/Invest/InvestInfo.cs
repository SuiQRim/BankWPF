using System.Windows.Media;

namespace BankObjects.CardPrefab.Invest
{
    public struct InvestInfo
    {
        public InvestInfo(double balance, double clearCash, int number, Brush statusColor)
        {

            Balance = balance;

            ClearCash = clearCash;

            MonthNumber = number;

            StatusColor = statusColor;
        }
        public InvestInfo(InvestInfo investInfo, Brush color)
        {

            Balance = investInfo.Balance;

            ClearCash = investInfo.ClearCash;

            MonthNumber = investInfo.MonthNumber;

            StatusColor = color;
        }

        public double Balance { get; set; }

        public double ClearCash { get; set; }

        public int MonthNumber { get; set; }

        public Brush StatusColor { get; set; }

    }
}
